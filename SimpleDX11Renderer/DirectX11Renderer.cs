using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Diagnostics;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DirectInput;
using SharpDX.DirectSound;
using SharpDX.DXGI;
using SharpDX.IO;
using SharpDX.Mathematics.Interop;
using SharpDX.MediaFoundation;
using SharpDX.Multimedia;
using SharpDX.RawInput;
using SharpDX.Text;
using SharpDX.Win32;
using SharpDX.Windows;
using SharpDX.X3DAudio;
using SharpDX.XAPO;
using SharpDX.XAudio2;
using SharpDX.XInput;
using SharpDX.WIC;
using System.Runtime.InteropServices;

namespace SimpleDX11Renderer
{
    internal class DirectX11Renderer
    {
        /// <summary>
        /// Renderer
        /// </summary>
        private Renderer Parent;

        /// <summary>
        /// DirectX11 rednering device
        /// </summary>
        internal SharpDX.Direct3D11.Device device;

        /// <summary>
        /// DirectX11 rednering devicecontext
        /// </summary>
        private DeviceContext deviceContext;

        /// <summary>
        /// rendering swapchain
        /// </summary>
        private SwapChain swapChain;

        /// <summary>
        /// Description for the swapchain
        /// </summary>
        private SwapChainDescription swapChainDescription;

        /// <summary>
        /// Target for rendering
        /// </summary>
        private RenderTargetView renderTarget;

        /// <summary>
        /// Depth view for depth rendering
        /// </summary>
        private DepthStencilView depthView;

        DepthStencilState depthStencilState;
        DepthStencilState depthStencilStateOff;

        /// <summary>
        /// Description for the rasterizer
        /// </summary>
        private RasterizerStateDescription rasterizerStateDescription;

        /// <summary>
        /// GPU projection settings input
        /// </summary>
        private SharpDX.Direct3D11.Buffer constMatrixBuffer;

        /// <summary>
        /// GPU light settings input
        /// </summary>
        private SharpDX.Direct3D11.Buffer constLightBuffer;

        /// <summary>
        /// GPU camera settings input
        /// </summary>
        private SharpDX.Direct3D11.Buffer constCameraBuffer;

        /// <summary>
        /// Buffer for vertex input in drawing
        /// </summary>
        private SharpDX.Direct3D11.Buffer triangelVertBuffer;

        /// <summary>
        /// Buffer for triangle input in drawing
        /// </summary>
        private SharpDX.Direct3D11.Buffer triangleBuffer;

        /// <summary>
        /// View matrix for the camera
        /// </summary>
        public Matrix CameraViewMatrix;

        /// <summary>
        /// Projection matrix for the camera
        /// </summary>
        public Matrix CameraProjectionMatrix;

        //private Texture2D texRenderTargetTexture;
        //private RenderTargetView texRenderTargetView;
        //private ShaderResourceView texShaderResourceView;

        //private bool RenderToTexture;

        private List<Mesh> DepthlessMeshes = new List<Mesh>();
        private List<Material> DepthlessMaterials = new List<Material>();
        private List<Vector3[]> DepthlesPositions = new List<Vector3[]>();
        private List<Vector3[]> DepthlessRotations = new List<Vector3[]>();
        private List<Vector3[]> DepthlessScales = new List<Vector3[]>();

        [StructLayout(LayoutKind.Sequential)]
        private struct LightSource
        {
            public Vector4 LightColor;
            public Vector3 LightPositionDirection;
            public float LightBrightnes; //(in LUX lumen @ 1 m^2)
            public float LightType;
            public Vector3 padding1;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MatrixBuffer
        {
            public Matrix worldMatrix;
            public Matrix viewMatrix;
            public Matrix projectionMatrix;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct LightBuffer
        {
            public float useTex1;
            public float useTex2;
            public float useNorm1;
            public float useNorm2;
            public float useSpec1;
            public float useSpec2;
            public float useRef1;
            public float useRef2;
            public float useTransp;

            public float AffectedByLight;
            public float ActiveLights;
            public float Exposure;

            public Vector4 ambientColor;

            public LightSource lightSource1;
            public LightSource lightSource2;
            public LightSource lightSource3;
            public LightSource lightSource4;
            public LightSource lightSource5;
            public LightSource lightSource6;
            public LightSource lightSource7;
            public LightSource lightSource8;
            public LightSource lightSource9;
            public LightSource lightSource10;
            public LightSource lightSource11;
            public LightSource lightSource12;
            public LightSource lightSource13;
            public LightSource lightSource14;
            public LightSource lightSource15;
            public LightSource lightSource16;

            public Vector4 materialColor;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct CameraBuffer
        {
            public Vector3 cameraPosition;
            public float padding;
        }

        /// <summary>
        /// Create a new DirectX 11 renderer
        /// </summary>
        /// <param name="renderer"> The parrent renderer </param>
        public DirectX11Renderer(Renderer renderer)
        {
            Parent = renderer;
        }

        /// <summary>
        /// Setup/rebuild the renderer
        /// </summary>
        /// <param name="Handle"> Handle to render on to </param>
        internal bool SetupRenderer(IntPtr Handle)
        {
            if (DeviceCreation(Handle))
            {
                DepthSetup();
                TransparancySetup();
                SampelerSetup();
                ShaderSetup();

                return true;
            }

            return false;
        }

        /// <summary>
        /// Rebuild camera view and projection matrices
        /// </summary>
        internal void UppdateCamera()
        {
            float XrotSin = (float)Math.Sin(Parent.RenderingCameraProperties.Rotation.X / 180 * Math.PI);
            float XrotCos = (float)Math.Cos(Parent.RenderingCameraProperties.Rotation.X / 180 * Math.PI);

            float YrotSin = (float)Math.Sin(Parent.RenderingCameraProperties.Rotation.Y / 180 * Math.PI);
            float YrotCos = (float)Math.Cos(Parent.RenderingCameraProperties.Rotation.Y / 180 * Math.PI);

            float ZrotSin = (float)Math.Sin(Parent.RenderingCameraProperties.Rotation.Z / 180 * Math.PI);
            float ZrotCos = (float)Math.Cos(Parent.RenderingCameraProperties.Rotation.Z / 180 * Math.PI);


            Vector3 Direction = new Vector3(
                -YrotSin * XrotCos,
                XrotSin,
                YrotCos * XrotCos);

            //Z axis done
            Vector3 UpVector = new Vector3(
                -ZrotSin * YrotCos,
                ZrotCos,
                -ZrotSin * YrotSin);

            CameraViewMatrix = Matrix.LookAtLH(Parent.RenderingCameraProperties.Position, Parent.RenderingCameraProperties.Position + Direction, UpVector);
            CameraProjectionMatrix = Matrix.PerspectiveFovLH(Parent.RenderingCameraProperties.FOV * (float)Math.PI / 180, (float)Parent.RenderingCameraProperties.ResolutionX / (float)Parent.RenderingCameraProperties.ResolutionY, Parent.RenderingCameraProperties.NearPlane, Parent.RenderingCameraProperties.FarPlane);

            CameraViewMatrix.Transpose();
            CameraProjectionMatrix.Transpose();
        }

        internal bool StartFrame()
        {
            deviceContext.ClearRenderTargetView(renderTarget, new RawColor4(0, 0, 0, 1f));
            deviceContext.ClearDepthStencilView(depthView, DepthStencilClearFlags.Depth | DepthStencilClearFlags.Stencil, 1, 0);

            if (Parent.frameStarted)
            {
                return false;
            }

            return true;
        }

        internal bool StartFrame(out Texture2D RenderTexture)
        {
            deviceContext.ClearRenderTargetView(renderTarget, new RawColor4(0, 0, 0, 1f));
            deviceContext.ClearDepthStencilView(depthView, DepthStencilClearFlags.Depth | DepthStencilClearFlags.Stencil, 1, 0);

            if (Parent.frameStarted)
            {
                RenderTexture = new Texture2D(new IntPtr());
                return false;
            }

            RenderTexture = new Texture2D(new IntPtr()); //texRenderTargetTexture;
            return true;
        }

        /// <summary>
        /// Draw a mesh
        /// </summary>
        /// <param name="mesh"> Mesh to draw </param>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        internal bool DrawMesh(Mesh mesh, Material material, Vector3[] position, Vector3[] rotation, Vector3[] scale)
        {
            if(mesh != null)
            {
                if (mesh.GetVertices().Count > 0 &&
                    mesh.GetPolygons().Count > 0)
                {
                    #region Matrix Buffer
                    //Set worldmatrix values
                    Matrix worldMatrix = Matrix.RotationX(Parent.WorldRotationOffset.X / 180 * (float)Math.PI) * Matrix.RotationY(Parent.WorldRotationOffset.Y / 180 * (float)Math.PI) * Matrix.RotationZ(Parent.WorldRotationOffset.Z / 180 * (float)Math.PI) * Matrix.Scaling(Parent.WorldScaleOffset) * Matrix.Translation(Parent.WorldPositionOffset);

                    Matrix TransformMatrix = Matrix.RotationZ(-rotation[position.Length - 1].Z / 180 * (float)Math.PI) * Matrix.RotationX(-rotation[position.Length - 1].X / 180 * (float)Math.PI) * Matrix.RotationY(-rotation[position.Length - 1].Y / 180 * (float)Math.PI) * Matrix.Scaling(scale[position.Length - 1]) * Matrix.Translation(position[position.Length - 1]);

                    for (int i = position.Length - 2; i > 0; i--)
                    {
                        TransformMatrix *= Matrix.RotationZ(-rotation[i].Z / 180 * (float)Math.PI) * Matrix.RotationX(-rotation[i].X / 180 * (float)Math.PI) * Matrix.RotationY(-rotation[i].Y / 180 * (float)Math.PI) * Matrix.Scaling(scale[i]) * Matrix.Translation(position[i]);
                    }

                    worldMatrix *= TransformMatrix;
                    worldMatrix.Transpose();

                    //Map subresource
                    deviceContext.MapSubresource(constMatrixBuffer, MapMode.WriteDiscard, SharpDX.Direct3D11.MapFlags.None, out DataStream mappedResource);

                    //Create a new matrix buffer
                    MatrixBuffer matrixBuffer = new MatrixBuffer()
                    {
                        worldMatrix = worldMatrix,
                        viewMatrix = CameraViewMatrix,
                        projectionMatrix = CameraProjectionMatrix
                    };

                    //Write the matrix buffer to the mapped subresource
                    mappedResource.Write(matrixBuffer);

                    //Unlock the constant matrix buffer
                    deviceContext.UnmapSubresource(constMatrixBuffer, 0);

                    // Set the matrix buffer in the first slot for buffers in the vertex shader
                    deviceContext.VertexShader.SetConstantBuffer(0, constMatrixBuffer);
                    #endregion

                    #region Camera Buffer
                    //Map subresource
                    deviceContext.MapSubresource(constCameraBuffer, MapMode.WriteDiscard, SharpDX.Direct3D11.MapFlags.None, out mappedResource);

                    //Create a new camera buffer
                    CameraBuffer cameraBuffer = new CameraBuffer()
                    {
                        cameraPosition = Parent.RenderingCameraProperties.Position,
                        padding = 0.0f
                    };

                    //Write the camera buffer to the mapped subresource
                    mappedResource.Write(cameraBuffer);

                    //Unlock the constant camera buffer
                    deviceContext.UnmapSubresource(constCameraBuffer, 0);

                    // Set the camera buffer in the second slot for buffers in the vertex shader
                    deviceContext.VertexShader.SetConstantBuffer(1, constCameraBuffer);
                    #endregion

                    #region Light Buffer
                    //Map subresource
                    deviceContext.MapSubresource(constLightBuffer, MapMode.WriteDiscard, SharpDX.Direct3D11.MapFlags.None, out mappedResource);

                    //Create a new lightbuffer
                    LightBuffer lightBuffer;

                    //Set values for the mesh
                    lightBuffer = new LightBuffer()
                    {
                        useTex1 = 0,
                        useTex2 = 0,
                        useNorm1 = 0,
                        useNorm2 = 0,
                        useSpec1 = 0,
                        useSpec2 = 0,
                        useRef1 = 0,
                        useRef2 = 0,
                        useTransp = 0,

                        ActiveLights = 0,
                        Exposure = 1,

                        ambientColor = new Vector4(0, 0, 0, 1),

                        materialColor = material.NoTexColor
                    };

                    if (material.AffectedByLight)
                    {
                        lightBuffer.AffectedByLight = 1;
                    }

                    else
                    {
                        lightBuffer.AffectedByLight = 0;
                    }

                    int MaxLights = Parent.Lights.Count;

                    if (MaxLights > 16)
                    {
                        MaxLights = 16;
                    }

                    LightSource[] tempLightSources = new LightSource[16];

                    for (int i = 0; i < MaxLights; i++)
                    {
                        tempLightSources[i].LightColor = Parent.Lights[i].LightColor;
                        tempLightSources[i].LightBrightnes = Parent.Lights[i].LightIntensity;

                        if (Parent.Lights[i].LightSourceType == LightSourceType.Directonal)
                        {
                            Vector3 Direction = new Vector3((float)Math.Cos(Parent.Lights[i].Rotation.Y / 180 * Math.PI) * (float)Math.Cos(Parent.Lights[i].Rotation.X / 180 * Math.PI),
                                                            (float)-Math.Sin(Parent.Lights[i].Rotation.X / 180 * Math.PI),
                                                            (float)-Math.Sin(Parent.Lights[i].Rotation.Y / 180 * Math.PI) * (float)Math.Cos(Parent.Lights[i].Rotation.X / 180 * Math.PI));
                            Direction.Normalize();

                            tempLightSources[i].LightPositionDirection = Direction;
                            tempLightSources[i].LightType = 0;
                        }

                        else if (Parent.Lights[i].LightSourceType == LightSourceType.Point)
                        {
                            tempLightSources[i].LightPositionDirection = Parent.Lights[i].Position;
                            tempLightSources[i].LightType = 1;
                        }
                    }

                    lightBuffer.lightSource1 = tempLightSources[0];
                    lightBuffer.lightSource2 = tempLightSources[1];
                    lightBuffer.lightSource3 = tempLightSources[2];
                    lightBuffer.lightSource4 = tempLightSources[3];
                    lightBuffer.lightSource5 = tempLightSources[4];
                    lightBuffer.lightSource6 = tempLightSources[5];
                    lightBuffer.lightSource7 = tempLightSources[6];
                    lightBuffer.lightSource8 = tempLightSources[7];
                    lightBuffer.lightSource9 = tempLightSources[8];
                    lightBuffer.lightSource10 = tempLightSources[9];
                    lightBuffer.lightSource11 = tempLightSources[10];
                    lightBuffer.lightSource12 = tempLightSources[11];
                    lightBuffer.lightSource13 = tempLightSources[12];
                    lightBuffer.lightSource14 = tempLightSources[13];
                    lightBuffer.lightSource15 = tempLightSources[14];
                    lightBuffer.lightSource16 = tempLightSources[15];

                    //Check if the object has a materil and if true determen how it will be drawn
                    if (material != null)
                    {
                        if (Parent.Textures != null)
                        {
                            if (Parent.Textures.Count != 0)
                            {
                                if (material.ColorChanel1 >= 0)
                                {
                                    if (Parent.Textures.Count > material.ColorChanel1)
                                    {
                                        if (Parent.Textures[material.ColorChanel1] != null)
                                        {
                                            deviceContext.PixelShader.SetShaderResource(0, new ShaderResourceView(device, Parent.DirectX11ImportTextures[material.ColorChanel1]));
                                            lightBuffer.useTex1 = 1;
                                        }

                                        else
                                        {
                                            Debug.Error("Failed to set ColorChanel1, Texture " + material.ColorChanel1 + " = null in the DirectX 11 renderer");
                                        }
                                    }

                                    else
                                    {
                                        Debug.Error("Failed to set ColorChanel1, Texture " + material.ColorChanel1 + " does not exist in the DirectX 11 renderer");
                                    }
                                }

                                if (material.ColorChanel2 >= 0)
                                {
                                    if (Parent.Textures.Count > material.ColorChanel2)
                                    {
                                        if (Parent.Textures[material.ColorChanel2] != null)
                                        {
                                            deviceContext.PixelShader.SetShaderResource(1, new ShaderResourceView(device, Parent.DirectX11ImportTextures[material.ColorChanel2]));
                                            lightBuffer.useTex2 = 1;
                                        }

                                        else
                                        {
                                            Debug.Error("Failed to set ColorChanel2, Texture " + material.ColorChanel1 + " = null in the DirectX 11 renderer");
                                        }
                                    }

                                    else
                                    {
                                        Debug.Error("Failed to set ColorChanel2, Texture " + material.ColorChanel1 + " does not exist in the DirectX 11 renderer");
                                    }
                                }

                                if (material.NormalChannel1.NormalMap >= 0)
                                {
                                    if (Parent.Textures.Count > material.NormalChannel1.NormalMap)
                                    {
                                        if (Parent.Textures[material.NormalChannel1.NormalMap] != null)
                                        {
                                            deviceContext.PixelShader.SetShaderResource(2, new ShaderResourceView(device, Parent.DirectX11ImportTextures[material.NormalChannel1.NormalMap]));
                                            lightBuffer.useNorm1 = 1;
                                        }

                                        else
                                        {
                                            Debug.Error("Failed to set NormalChannel1, Texture " + material.NormalChannel1.NormalMap + " = null in the DirectX 11 renderer");
                                        }
                                    }

                                    else
                                    {
                                        Debug.Error("Failed to set NormalChannel1, Texture " + material.NormalChannel1.NormalMap + " does not exist in the DirectX 11 renderer");
                                    }
                                }

                                if (material.NormalChannel2.NormalMap >= 0)
                                {
                                    if (Parent.Textures.Count > material.NormalChannel2.NormalMap)
                                    {
                                        if (Parent.Textures[material.NormalChannel2.NormalMap] != null)
                                        {
                                            deviceContext.PixelShader.SetShaderResource(3, new ShaderResourceView(device, Parent.DirectX11ImportTextures[material.NormalChannel2.NormalMap]));
                                            lightBuffer.useNorm2 = 1;
                                        }

                                        else
                                        {
                                            Debug.Error("Failed to set NormalChannel2, Texture " + material.NormalChannel2.NormalMap + " = null in the DirectX 11 renderer");
                                        }
                                    }

                                    else
                                    {
                                        Debug.Error("Failed to set NormalChannel2, Texture " + material.NormalChannel2.NormalMap + " does not exist in the DirectX 11 renderer");
                                    }
                                }

                                if (material.SpecularChannel1.SpecularMap >= 0)
                                {
                                    if (Parent.Textures.Count > material.SpecularChannel1.SpecularMap)
                                    {
                                        if (Parent.Textures[material.SpecularChannel1.SpecularMap] != null)
                                        {
                                            deviceContext.PixelShader.SetShaderResource(4, new ShaderResourceView(device, Parent.DirectX11ImportTextures[material.SpecularChannel1.SpecularMap]));
                                            lightBuffer.useSpec1 = 1;
                                        }

                                        else
                                        {
                                            Debug.Error("Failed to set SpecularChannel1, Texture " + material.SpecularChannel1.SpecularMap + " = null in the DirectX 11 renderer");
                                        }
                                    }

                                    else
                                    {
                                        Debug.Error("Failed to set SpecularChannel1, Texture " + material.SpecularChannel1.SpecularMap + " does not exist in the DirectX 11 renderer");
                                    }
                                }

                                if (material.SpecularChannel2.SpecularMap >= 0)
                                {
                                    if (Parent.Textures.Count > material.SpecularChannel2.SpecularMap)
                                    {
                                        if (Parent.Textures[material.SpecularChannel2.SpecularMap] != null)
                                        {
                                            deviceContext.PixelShader.SetShaderResource(5, new ShaderResourceView(device, Parent.DirectX11ImportTextures[material.SpecularChannel2.SpecularMap]));
                                            lightBuffer.useSpec2 = 1;
                                        }

                                        else
                                        {
                                            Debug.Error("Failed to set SpecularChannel2, Texture " + material.SpecularChannel2.SpecularMap + " = null in the DirectX 11 renderer");
                                        }
                                    }

                                    else
                                    {
                                        Debug.Error("Failed to set SpecularChannel2, Texture " + material.SpecularChannel2.SpecularMap + " does not exist in the DirectX 11 renderer");
                                    }
                                }

                                if (material.ReflectionChannel1.ReflectionColorMap >= 0)
                                {
                                    if (Parent.Textures.Count > material.ReflectionChannel1.ReflectionColorMap)
                                    {
                                        if (Parent.Textures[material.ReflectionChannel1.ReflectionColorMap] != null)
                                        {
                                            deviceContext.PixelShader.SetShaderResource(6, new ShaderResourceView(device, Parent.DirectX11ImportTextures[material.ReflectionChannel1.ReflectionColorMap]));
                                            lightBuffer.useRef1 = 1;
                                        }

                                        else
                                        {
                                            Debug.Error("Failed to set ReflectionChannel1, Texture " + material.ReflectionChannel1.ReflectionColorMap + " = null in the DirectX 11 renderer");
                                        }
                                    }

                                    else
                                    {
                                        Debug.Error("Failed to set ReflectionChannel1, Texture " + material.ReflectionChannel1.ReflectionColorMap + " does not exist in the DirectX 11 renderer");
                                    }
                                }

                                if (material.ReflectionChannel2.ReflectionColorMap >= 0)
                                {
                                    if (Parent.Textures.Count > material.ReflectionChannel2.ReflectionColorMap)
                                    {
                                        if (Parent.Textures[material.ReflectionChannel2.ReflectionColorMap] != null)
                                        {
                                            deviceContext.PixelShader.SetShaderResource(7, new ShaderResourceView(device, Parent.DirectX11ImportTextures[material.ReflectionChannel2.ReflectionColorMap]));
                                            lightBuffer.useRef2 = 1;
                                        }

                                        else
                                        {
                                            Debug.Error("Failed to set ReflectionChannel2, Texture " + material.ReflectionChannel2.ReflectionColorMap + " = null in the DirectX 11 renderer");
                                        }
                                    }

                                    else
                                    {
                                        Debug.Error("Failed to set ReflectionChannel2, Texture " + material.ReflectionChannel2.ReflectionColorMap + " does not exist in the DirectX 11 renderer");
                                    }
                                }

                                if (material.TransparancyChannel1.TransparancyMap >= 0)
                                {
                                    if (Parent.Textures.Count > material.TransparancyChannel1.TransparancyMap)
                                    {
                                        if (Parent.Textures[material.TransparancyChannel1.TransparancyMap] != null)
                                        {
                                            deviceContext.PixelShader.SetShaderResource(8, new ShaderResourceView(device, Parent.DirectX11ImportTextures[material.TransparancyChannel1.TransparancyMap]));
                                            lightBuffer.useTransp = 1;
                                        }

                                        else
                                        {
                                            Debug.Error("Failed to set TransparancyChannel1, Texture " + material.TransparancyChannel1.TransparancyMap + " = null in the DirectX 11 renderer");
                                        }
                                    }

                                    else
                                    {
                                        Debug.Error("Failed to set TransparancyChannel1, Texture " + material.TransparancyChannel1.TransparancyMap + " does not exist in the DirectX 11 renderer");
                                    }
                                }
                            }

                            else
                            {
                                Debug.Error("Textures.Count = 0 in the DirectX 11 renderer");
                            }
                        }

                        else
                        {
                            Debug.Error("Textures = null in the DirectX 11 renderer");
                        }
                    }

                    else
                    {
                        Debug.Error("selected material = null in the DirectX 11 renderer");
                    }

                    //Write the camera buffer to the mapped subresource
                    mappedResource.Write(lightBuffer);

                    // Unlock the light buffer
                    deviceContext.UnmapSubresource(constLightBuffer, 0);

                    // Set the light buffer in the first slot for buffers in the pixel shader
                    deviceContext.PixelShader.SetConstantBuffer(0, constLightBuffer);

                    // Set the light buffer in the third slot for buffers in the vertex shader
                    deviceContext.VertexShader.SetConstantBuffer(2, constLightBuffer);
                    #endregion

                    #region Draw
                    Vertex[] DrawVerts = mesh.GetDrawVertices();
                    triangelVertBuffer = SharpDX.Direct3D11.Buffer.Create(device, BindFlags.VertexBuffer, data: DrawVerts);
                    triangleBuffer = SharpDX.Direct3D11.Buffer.Create(device, BindFlags.IndexBuffer, data: mesh.GetPolygons().ToArray());

                    deviceContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(triangelVertBuffer, Utilities.SizeOf<Vertex>(), 0));
                    deviceContext.InputAssembler.SetIndexBuffer(triangleBuffer, Format.R32_UInt, 0);

                    deviceContext.Draw(DrawVerts.Length, 0);

                    if (DrawVerts.Length != 0 /*&& mesh.Polygons.Count != 0*/)
                    {
                        triangelVertBuffer.Dispose();
                        triangleBuffer.Dispose();
                    }
                    #endregion

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a triangle
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        internal bool DrawTriangle(Material material, Vector3[] position, Vector3[] rotation, Vector3[] scale)
        {
            return DrawMesh(Parent.Triangle, material, position, rotation, scale);
        }

        /// <summary>
        /// Draw a quad
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        internal bool DrawQuad(Material material, Vector3[] position, Vector3[] rotation, Vector3[] scale)
        {
            return DrawMesh(Parent.Quadrangle, material, position, rotation, scale);
        }

        /// <summary>
        /// Draw a plane
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        internal bool DrawPlane(Material material, Vector3[] position, Vector3[] rotation, Vector3[] scale)
        {
            return DrawMesh(Parent.Plane, material, position, rotation, scale);
        }

        /// <summary>
        /// Draw a cube
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        internal bool DrawCube(Material material, Vector3[] position, Vector3[] rotation, Vector3[] scale)
        {
            return DrawMesh(Parent.Cube, material, position, rotation, scale);
        }

        /// <summary>
        /// Draw a sphere
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        internal bool DrawSphere(Material material, Vector3[] position, Vector3[] rotation, Vector3[] scale)
        {
            return DrawMesh(Parent.Sphere, material, position, rotation, scale);
        }

        internal bool DrawIcon()
        {
            return false;
        }

        /// <summary>
        /// Draw a mesh depthless
        /// </summary>
        /// <param name="mesh"> Mesh to draw </param>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        internal void DrawMeshDepthless(Mesh mesh, Material material, Vector3[] position, Vector3[] rotation, Vector3[] scale)
        {
            DepthlessMeshes.Add(mesh);
            DepthlessMaterials.Add(material);
            DepthlesPositions.Add(position);
            DepthlessRotations.Add(rotation);
            DepthlessScales.Add(scale);
        }

        /// <summary>
        /// Draw a triangle depthless
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        internal void DrawTriangleDepthless(Material material, Vector3[] position, Vector3[] rotation, Vector3[] scale)
        {
            DepthlessMeshes.Add(Parent.Triangle);
            DepthlessMaterials.Add(material);
            DepthlesPositions.Add(position);
            DepthlessRotations.Add(rotation);
            DepthlessScales.Add(scale);
        }

        /// <summary>
        /// Draw a quad depthless
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        internal void DrawQuadDepthless(Material material, Vector3[] position, Vector3[] rotation, Vector3[] scale)
        {
            DepthlessMeshes.Add(Parent.Quadrangle);
            DepthlessMaterials.Add(material);
            DepthlesPositions.Add(position);
            DepthlessRotations.Add(rotation);
            DepthlessScales.Add(scale);
        }

        /// <summary>
        /// Draw a plane depthless
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        internal void DrawPlaneDepthless(Material material, Vector3[] position, Vector3[] rotation, Vector3[] scale)
        {
            DepthlessMeshes.Add(Parent.Plane);
            DepthlessMaterials.Add(material);
            DepthlesPositions.Add(position);
            DepthlessRotations.Add(rotation);
            DepthlessScales.Add(scale);
        }

        /// <summary>
        /// Draw a cube depthless
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        internal void DrawCubeDepthless(Material material, Vector3[] position, Vector3[] rotation, Vector3[] scale)
        {
            DepthlessMeshes.Add(Parent.Cube);
            DepthlessMaterials.Add(material);
            DepthlesPositions.Add(position);
            DepthlessRotations.Add(rotation);
            DepthlessScales.Add(scale);
        }

        /// <summary>
        /// Draw a sphere depthless
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        internal void DrawSphereDepthless(Material material, Vector3[] position, Vector3[] rotation, Vector3[] scale)
        {
            DepthlessMeshes.Add(Parent.Sphere);
            DepthlessMaterials.Add(material);
            DepthlesPositions.Add(position);
            DepthlessRotations.Add(rotation);
            DepthlessScales.Add(scale);
        }

        internal bool DrawIconDepthless()
        {
            return false;
        }

        internal void EndFrame()
        {
            //TurnOffZbuffer();

            deviceContext.ClearDepthStencilView(depthView, DepthStencilClearFlags.Depth | DepthStencilClearFlags.Stencil, 1, 0);

            for (int i = 0; i < DepthlessMeshes.Count; i++)
            {
                DrawMesh(DepthlessMeshes[i], DepthlessMaterials[i], DepthlesPositions[i], DepthlessRotations[i], DepthlessScales[i]);
            }

            DepthlessMeshes.Clear();
            DepthlessMaterials.Clear();
            DepthlesPositions.Clear();
            DepthlessRotations.Clear();
            DepthlessScales.Clear();

            //TurnOnZbuffer();

            swapChain.Present(1, PresentFlags.None);
        }

        internal bool ReStartFrame()
        {
            return false;
        }

        private void TurnOffZbuffer()
        {
            deviceContext.OutputMerger.SetDepthStencilState(depthStencilStateOff, 1);
        }

        private void TurnOnZbuffer()
        {
            deviceContext.OutputMerger.SetDepthStencilState(depthStencilState, 1);
        }

        /// <summary>
        /// Create the device (Binding component in the rendering process)
        /// </summary>
        private bool DeviceCreation(IntPtr Handle)
        {
            swapChainDescription.IsWindowed = true;
            swapChainDescription.BufferCount = 1;
            swapChainDescription.OutputHandle = Handle;
            swapChainDescription.Flags = SwapChainFlags.None;
            swapChainDescription.Usage = Usage.RenderTargetOutput;
            swapChainDescription.SampleDescription = new SampleDescription(1, 0);
            swapChainDescription.ModeDescription = new ModeDescription(Parent.RenderingCameraProperties.ResolutionX, Parent.RenderingCameraProperties.ResolutionY, new Rational(60, 1), Format.R8G8B8A8_UNorm);
            swapChainDescription.SwapEffect = SwapEffect.Discard;

            try
            {
                //Creation of Device
                SharpDX.Direct3D11.Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.None, swapChainDescription, out device, out swapChain);

                //Set device context
                deviceContext = new DeviceContext(device);
                deviceContext = device.ImmediateContext;

                //Create Viewport
                ViewportF viewport = new ViewportF
                {
                    Width = Parent.RenderingCameraProperties.ResolutionX,
                    Height = Parent.RenderingCameraProperties.ResolutionY,
                    X = 0,
                    Y = 0,
                    MinDepth = 0,
                    MaxDepth = 1
                };

                //Set Viewport
                deviceContext.Rasterizer.SetViewport(viewport);

                //Decription of how the rasterizer should work
                rasterizerStateDescription = new RasterizerStateDescription()
                {
                    IsAntialiasedLineEnabled = false,
                    CullMode = CullMode.Back,
                    DepthBias = 0,
                    DepthBiasClamp = 0.0f,
                    IsDepthClipEnabled = true,
                    FillMode = FillMode.Solid,
                    IsFrontCounterClockwise = false,
                    IsMultisampleEnabled = false,
                    IsScissorEnabled = false,
                    SlopeScaledDepthBias = 0.0f
                };

                //Set rastericzer
                deviceContext.Rasterizer.State = new RasterizerState(device, rasterizerStateDescription);

                return true;
            }

            catch
            {
                Console.WriteLine("Failed to create a device");
                return false;
            }
        }

        /// <summary>
        /// Depth rendering setup
        /// </summary>
        private void DepthSetup()
        {
            #region Depth Descriptionns
            //Creates a description for depth rendering process
            DepthStencilStateDescription depthDesc = new DepthStencilStateDescription
            {
                IsDepthEnabled = true,
                DepthWriteMask = DepthWriteMask.All,
                DepthComparison = Comparison.LessEqual,
                IsStencilEnabled = true,
                StencilReadMask = 0xFF,
                StencilWriteMask = 0xFF,

                FrontFace =
                {
                    Comparison = Comparison.Always,
                    DepthFailOperation = StencilOperation.Increment,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.Keep
                },

                BackFace =
                {
                    Comparison = Comparison.Always,
                    DepthFailOperation = StencilOperation.Decrement,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.Keep
                }
            };

            DepthStencilStateDescription depthOffDesc = new DepthStencilStateDescription
            {
                IsDepthEnabled = false,
                DepthWriteMask = DepthWriteMask.All,
                DepthComparison = Comparison.LessEqual,
                IsStencilEnabled = true,
                StencilReadMask = 0xFF,
                StencilWriteMask = 0xFF,

                FrontFace =
                {
                    Comparison = Comparison.Always,
                    DepthFailOperation = StencilOperation.Increment,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.Keep
                },

                BackFace =
                {
                    Comparison = Comparison.Always,
                    DepthFailOperation = StencilOperation.Decrement,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.Keep
                }
            };

            //Aditional decription for how depth rendering should work
            DepthStencilViewDescription depthStencilViewDescription = new DepthStencilViewDescription
            {
                Format = Format.D24_UNorm_S8_UInt,
                Dimension = DepthStencilViewDimension.Texture2D,
                Texture2D = new DepthStencilViewDescription.Texture2DResource()
                {
                    MipSlice = 0
                }
            };
            #endregion

            //Depth buffer
            Texture2D depthBuffer = new Texture2D(device, new Texture2DDescription()
            {
                Format = Format.D24_UNorm_S8_UInt,
                ArraySize = 1,
                MipLevels = 1,
                Width = Parent.RenderingCameraProperties.ResolutionX,
                Height = Parent.RenderingCameraProperties.ResolutionY,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Default,
                BindFlags = BindFlags.DepthStencil,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None
            });

            //"view" dedicated to depth rendering
            depthView = new DepthStencilView(device, depthBuffer, depthStencilViewDescription);

            //Depth stensil state dedicated to depth rendering
            depthStencilState = new DepthStencilState(device, depthDesc);
            depthStencilStateOff = new DepthStencilState(device, depthOffDesc);

            //Creates the back buffer used for rendering
            Texture2D backBuffer = Texture2D.FromSwapChain<Texture2D>(swapChain, 0);

            //Creates the render target
            renderTarget = new RenderTargetView(device, backBuffer);

            //Sets outputs settings
            deviceContext.OutputMerger.SetTargets(depthView, renderTarget);

            TurnOnZbuffer();
        }

        /// <summary>
        /// Setup transparancy
        /// </summary>
        private void TransparancySetup()
        {
            //Create a blend state description and sets apropriet values
            BlendStateDescription blendDesc = new BlendStateDescription();
            blendDesc.RenderTarget[0].IsBlendEnabled = true;
            blendDesc.RenderTarget[0].SourceBlend = BlendOption.SourceAlpha;
            blendDesc.RenderTarget[0].DestinationBlend = BlendOption.InverseSourceAlpha;
            blendDesc.RenderTarget[0].BlendOperation = BlendOperation.Add;
            blendDesc.RenderTarget[0].SourceAlphaBlend = BlendOption.One;
            blendDesc.RenderTarget[0].DestinationAlphaBlend = BlendOption.InverseSourceAlpha;
            blendDesc.RenderTarget[0].AlphaBlendOperation = BlendOperation.Add;
            blendDesc.RenderTarget[0].RenderTargetWriteMask = ColorWriteMaskFlags.All;

            //Creates a blend state
            BlendState AlphaEnableBlendingState = new BlendState(device, blendDesc);

            //Applies the blend state
            deviceContext.OutputMerger.SetBlendState(AlphaEnableBlendingState, new Color4(0, 0, 0, 0), -1);
        }

        /// <summary>
        /// Used for mapping textures to objects with UV cordnates
        /// </summary>
        private void SampelerSetup()
        {
            SamplerStateDescription samplerStateDescription = new SamplerStateDescription
            {
                AddressU = TextureAddressMode.Wrap,
                AddressV = TextureAddressMode.Wrap,
                AddressW = TextureAddressMode.Wrap,
                Filter = Filter.MinMagMipLinear,
                MipLodBias = 0,
                MaximumAnisotropy = 1,
                ComparisonFunction = Comparison.Always,
                BorderColor = new Color4(0, 0, 0, 0),
                MinimumLod = 0,
                MaximumLod = float.MaxValue
            };

            //Creates a sampeler state
            SamplerState samplerState = new SamplerState(device, samplerStateDescription);

            //Sets the sampeler state
            deviceContext.PixelShader.SetSampler(0, samplerState);
        }

        /// <summary>
        /// Load shaders and shader resources
        /// </summary>
        private void ShaderSetup()
        {
            //Input structure for the rendering pipeline
            InputElement[] inputElements = new InputElement[]
            {
                new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0, InputClassification.PerVertexData, 0),
                new InputElement("TEXCORD", 0, Format.R32G32_Float, InputElement.AppendAligned, 0, InputClassification.PerVertexData, 0),
                new InputElement("NORMAL", 0, Format.R32G32B32_Float, InputElement.AppendAligned, 0, InputClassification.PerVertexData, 0),
                new InputElement("TANGENT", 0, Format.R32G32B32_Float, InputElement.AppendAligned, 0, InputClassification.PerVertexData, 0),
                new InputElement("BINORMAL", 0, Format.R32G32B32_Float, InputElement.AppendAligned, 0, InputClassification.PerVertexData, 0),
            };

            //Get the vertex/pixel shader data from the shader file
            CompilationResult vertexShaderBytesCode = ShaderBytecode.CompileFromFile("Shader.hlsl", "VS", "vs_5_0", ShaderFlags.Debug);
            CompilationResult pixelShaderBytesCode = ShaderBytecode.CompileFromFile("Shader.hlsl", "PS", "ps_5_0", ShaderFlags.Debug);

            //Generate a byte array from the vertex shader import file
            byte[] vertexShaderSignature = ShaderSignature.GetInputSignature(vertexShaderBytesCode);

            //Creates new Pixel and Vertex shaders
            VertexShader vertexShader = new VertexShader(device, vertexShaderBytesCode);
            PixelShader pixelShader = new PixelShader(device, pixelShaderBytesCode);

            //Creates buffers used in rendering
            constMatrixBuffer = new SharpDX.Direct3D11.Buffer(device, Utilities.SizeOf<MatrixBuffer>(), ResourceUsage.Dynamic, BindFlags.ConstantBuffer, CpuAccessFlags.Write, ResourceOptionFlags.None, 0);
            constLightBuffer = new SharpDX.Direct3D11.Buffer(device, Utilities.SizeOf<LightBuffer>(), ResourceUsage.Dynamic, BindFlags.ConstantBuffer, CpuAccessFlags.Write, ResourceOptionFlags.None, 0);
            constCameraBuffer = new SharpDX.Direct3D11.Buffer(device, Utilities.SizeOf<CameraBuffer>(), ResourceUsage.Dynamic, BindFlags.ConstantBuffer, CpuAccessFlags.Write, ResourceOptionFlags.None, 0);

            //Sets the shaders
            deviceContext.VertexShader.Set(vertexShader);
            deviceContext.PixelShader.Set(pixelShader);

            //Sets rendering mode and data input mode
            deviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
            deviceContext.InputAssembler.InputLayout = new InputLayout(device, vertexShaderSignature, inputElements);
        }

        internal void StopRenderer()
        {
            Dispose();
        }

        private void Dispose()
        {
            device.Dispose();
            deviceContext.Dispose();
            depthView.Dispose();
            constMatrixBuffer.Dispose();
            constLightBuffer.Dispose();
            constCameraBuffer.Dispose();
            triangelVertBuffer.Dispose();
            triangleBuffer.Dispose();
        }
    }
}