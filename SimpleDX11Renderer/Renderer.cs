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
    /// <summary>
    /// Supported rendering APIs
    /// </summary>
    public enum RenderingAPI
    {
        DirectX11
    }

    /// <summary>
    /// Properties of a rendering camera
    /// </summary>
    public struct CameraProperties
    {
        /// <summary>
        /// Is the camera perspective or orthographic
        /// true = perspective
        /// false = orthographic
        /// </summary>
        public bool Perspective;

        /// <summary>
        /// FOV of an perspective camera
        /// </summary>
        public float FOV;

        /// <summary>
        /// Width of an orthographic camera
        /// </summary>
        public float OrthographicWidth;

        /// <summary>
        /// Height of an orthographic camera
        /// </summary>
        public float OrthographicHeight;

        /// <summary>
        /// Positional value of a camera
        /// </summary>
        public Vector3 Position;

        /// <summary>
        /// Rotational value of a camera
        /// </summary>
        public Vector3 Rotation;

        /// <summary>
        /// Resolution of a camera on the X axis
        /// </summary>
        public int ResolutionX;

        /// <summary>
        /// Resolution of a camera on the Y axis
        /// </summary>
        public int ResolutionY;

        /// <summary>
        /// Minimum render distance of a camera
        /// </summary>
        public float NearPlane;

        /// <summary>
        /// Maximum render distance of a camera
        /// </summary>
        public float FarPlane;

        /// <summary>
        /// Properties for render camera
        /// </summary>
        /// <param name="FOV"> Field of view of perspectiv camera </param>
        /// <param name="ResolutionX"> Camera resolution on X axis </param>
        /// <param name="ResolutionY"> Camera resolution on Y axis </param>
        public CameraProperties(float FOV, int ResolutionX, int ResolutionY)
        {
            Perspective = true;
            this.FOV = FOV;
            OrthographicWidth = 1;
            OrthographicHeight = 1;
            Position = new Vector3(0);
            Rotation = new Vector3(0);
            this.ResolutionX = ResolutionX;
            this.ResolutionY = ResolutionY;
            NearPlane = 0.03f;
            FarPlane = 1000;
        }

        /// <summary>
        /// Properties for render camera
        /// </summary>
        /// <param name="OrthographicWidth"> Width of orthographic camera sensor </param>
        /// <param name="OrthographicHeight"> Height of orthographic camera sensor </param>
        /// <param name="ResolutionX"> Camera resolution on X axis </param>
        /// <param name="ResolutionY"> Camera resolution on Y axis </param>
        public CameraProperties(float OrthographicWidth, float OrthographicHeight, int ResolutionX, int ResolutionY)
        {
            Perspective = false;
            FOV = 60;
            this.OrthographicWidth = OrthographicWidth;
            this.OrthographicHeight = OrthographicHeight;
            Position = new Vector3(0);
            Rotation = new Vector3(0);
            this.ResolutionX = ResolutionX;
            this.ResolutionY = ResolutionY;
            NearPlane = 0.03f;
            FarPlane = 1000;
        }

        /// <summary>
        /// Properties for render camera
        /// </summary>
        /// <param name="FOV"> Field of view of perspectiv camera </param>
        /// <param name="ResolutionX"> Camera resolution on X axis </param>
        /// <param name="ResolutionY"> Camera resolution on Y axis </param>
        /// <param name="Position"> Camera Position </param>
        public CameraProperties(float FOV, int ResolutionX, int ResolutionY, Vector3 Position)
        {
            Perspective = true;
            this.FOV = FOV;
            OrthographicWidth = 1;
            OrthographicHeight = 1;
            this.Position = Position;
            Rotation = new Vector3(0);
            this.ResolutionX = ResolutionX;
            this.ResolutionY = ResolutionY;
            NearPlane = 0.03f;
            FarPlane = 1000;
        }

        /// <summary>
        /// Properties for render camera
        /// </summary>
        /// <param name="OrthographicWidth"> Width of orthographic camera sensor </param>
        /// <param name="OrthographicHeight"> Height of orthographic camera sensor </param>
        /// <param name="ResolutionX"> Camera resolution on X axis </param>
        /// <param name="ResolutionY"> Camera resolution on Y axis </param>
        /// <param name="Position"> Camera Position </param>
        public CameraProperties(float OrthographicWidth, float OrthographicHeight, int ResolutionX, int ResolutionY, Vector3 Position)
        {
            Perspective = false;
            FOV = 60;
            this.OrthographicWidth = OrthographicWidth;
            this.OrthographicHeight = OrthographicHeight;
            this.Position = Position;
            Rotation = new Vector3(0);
            this.ResolutionX = ResolutionX;
            this.ResolutionY = ResolutionY;
            NearPlane = 0.03f;
            FarPlane = 1000;
        }

        /// <summary>
        /// Properties for render camera
        /// </summary>
        /// <param name="FOV"> Field of view of perspectiv camera </param>
        /// <param name="ResolutionX"> Camera resolution on X axis </param>
        /// <param name="ResolutionY"> Camera resolution on Y axis </param>
        /// <param name="Position"> Camera Position </param>
        /// <param name="Rotation"> Camera Rotation </param>
        public CameraProperties(float FOV, int ResolutionX, int ResolutionY, Vector3 Position, Vector3 Rotation)
        {
            Perspective = true;
            this.FOV = FOV;
            OrthographicWidth = 1;
            OrthographicHeight = 1;
            this.Position = Position;
            this.Rotation = Rotation;
            this.ResolutionX = ResolutionX;
            this.ResolutionY = ResolutionY;
            NearPlane = 0.03f;
            FarPlane = 1000;
        }

        /// <summary>
        /// Properties for render camera
        /// </summary>
        /// <param name="OrthographicWidth"> Width of orthographic camera sensor </param>
        /// <param name="OrthographicHeight"> Height of orthographic camera sensor </param>
        /// <param name="ResolutionX"> Camera resolution on X axis </param>
        /// <param name="ResolutionY"> Camera resolution on Y axis </param>
        /// <param name="Position"> Camera Position </param>
        /// <param name="Rotation"> Camera Rotation </param>
        public CameraProperties(float OrthographicWidth, float OrthographicHeight, int ResolutionX, int ResolutionY, Vector3 Position, Vector3 Rotation)
        {
            Perspective = false;
            FOV = 60;
            this.OrthographicWidth = OrthographicWidth;
            this.OrthographicHeight = OrthographicHeight;
            this.Position = Position;
            this.Rotation = Rotation;
            this.ResolutionX = ResolutionX;
            this.ResolutionY = ResolutionY;
            NearPlane = 0.03f;
            FarPlane = 1000;
        }

        /// <summary>
        /// Properties for render camera
        /// </summary>
        /// <param name="FOV"> Field of view of perspectiv camera </param>
        /// <param name="ResolutionX"> Camera resolution on X axis </param>
        /// <param name="ResolutionY"> Camera resolution on Y axis </param>
        /// <param name="NearPlane"> Camera minimum render distance </param>
        /// <param name="FarPlane"> Camera maximum render distance </param>
        public CameraProperties(float FOV, int ResolutionX, int ResolutionY, float NearPlane, float FarPlane)
        {
            Perspective = true;
            this.FOV = FOV;
            OrthographicWidth = 1;
            OrthographicHeight = 1;
            Position = new Vector3(0);
            Rotation = new Vector3(0);
            this.ResolutionX = ResolutionX;
            this.ResolutionY = ResolutionY;
            this.NearPlane = NearPlane;
            this.FarPlane = FarPlane;
        }

        /// <summary>
        /// Properties for render camera
        /// </summary>
        /// <param name="OrthographicWidth"> Width of orthographic camera sensor </param>
        /// <param name="OrthographicHeight"> Height of orthographic camera sensor </param>
        /// <param name="ResolutionX"> Camera resolution on X axis </param>
        /// <param name="ResolutionY"> Camera resolution on Y axis </param>
        /// <param name="NearPlane"> Camera minimum render distance </param>
        /// <param name="FarPlane"> Camera maximum render distance </param>
        public CameraProperties(float OrthographicWidth, float OrthographicHeight, int ResolutionX, int ResolutionY, float NearPlane, float FarPlane)
        {
            Perspective = false;
            FOV = 60;
            this.OrthographicWidth = OrthographicWidth;
            this.OrthographicHeight = OrthographicHeight;
            Position = new Vector3(0);
            Rotation = new Vector3(0);
            this.ResolutionX = ResolutionX;
            this.ResolutionY = ResolutionY;
            this.NearPlane = NearPlane;
            this.FarPlane = FarPlane;
        }

        /// <summary>
        /// Properties for render camera
        /// </summary>
        /// <param name="FOV"> Field of view of perspectiv camera </param>
        /// <param name="ResolutionX"> Camera resolution on X axis </param>
        /// <param name="ResolutionY"> Camera resolution on Y axis </param>
        /// <param name="Position"> Camera Position </param>
        /// <param name="NearPlane"> Camera minimum render distance </param>
        /// <param name="FarPlane"> Camera maximum render distance </param>
        public CameraProperties(float FOV, int ResolutionX, int ResolutionY, Vector3 Position, float NearPlane, float FarPlane)
        {
            Perspective = true;
            this.FOV = FOV;
            OrthographicWidth = 1;
            OrthographicHeight = 1;
            this.Position = Position;
            Rotation = new Vector3(0);
            this.ResolutionX = ResolutionX;
            this.ResolutionY = ResolutionY;
            this.NearPlane = NearPlane;
            this.FarPlane = FarPlane;
        }

        /// <summary>
        /// Properties for render camera
        /// </summary>
        /// <param name="OrthographicWidth"> Width of orthographic camera sensor </param>
        /// <param name="OrthographicHeight"> Height of orthographic camera sensor </param>
        /// <param name="ResolutionX"> Camera resolution on X axis </param>
        /// <param name="ResolutionY"> Camera resolution on Y axis </param>
        /// <param name="Position"> Camera Position </param>
        /// <param name="NearPlane"> Camera minimum render distance </param>
        /// <param name="FarPlane"> Camera maximum render distance </param>
        public CameraProperties(float OrthographicWidth, float OrthographicHeight, int ResolutionX, int ResolutionY, Vector3 Position, float NearPlane, float FarPlane)
        {
            Perspective = false;
            FOV = 60;
            this.OrthographicWidth = OrthographicWidth;
            this.OrthographicHeight = OrthographicHeight;
            this.Position = Position;
            Rotation = new Vector3(0);
            this.ResolutionX = ResolutionX;
            this.ResolutionY = ResolutionY;
            this.NearPlane = NearPlane;
            this.FarPlane = FarPlane;
        }

        /// <summary>
        /// Properties for render camera
        /// </summary>
        /// <param name="FOV"> Field of view of perspectiv camera </param>
        /// <param name="ResolutionX"> Camera resolution on X axis </param>
        /// <param name="ResolutionY"> Camera resolution on Y axis </param>
        /// <param name="Position"> Camera Position </param>
        /// <param name="Rotation"> Camera Rotation </param>
        /// <param name="NearPlane"> Camera minimum render distance </param>
        /// <param name="FarPlane"> Camera maximum render distance </param>
        public CameraProperties(float FOV, int ResolutionX, int ResolutionY, Vector3 Position, Vector3 Rotation, float NearPlane, float FarPlane)
        {
            Perspective = true;
            this.FOV = FOV;
            OrthographicWidth = 1;
            OrthographicHeight = 1;
            this.Position = Position;
            this.Rotation = Rotation;
            this.ResolutionX = ResolutionX;
            this.ResolutionY = ResolutionY;
            this.NearPlane = NearPlane;
            this.FarPlane = FarPlane;
        }

        /// <summary>
        /// Properties for render camera
        /// </summary>
        /// <param name="OrthographicWidth"> Width of orthographic camera sensor </param>
        /// <param name="OrthographicHeight"> Height of orthographic camera sensor </param>
        /// <param name="ResolutionX"> Camera resolution on X axis </param>
        /// <param name="ResolutionY"> Camera resolution on Y axis </param>
        /// <param name="Position"> Camera Position </param>
        /// <param name="Rotation"> Camera Rotation </param>
        /// <param name="NearPlane"> Camera minimum render distance </param>
        /// <param name="FarPlane"> Camera maximum render distance </param>
        public CameraProperties(float OrthographicWidth, float OrthographicHeight, int ResolutionX, int ResolutionY, Vector3 Position, Vector3 Rotation, float NearPlane, float FarPlane)
        {
            Perspective = false;
            FOV = 60;
            this.OrthographicWidth = OrthographicWidth;
            this.OrthographicHeight = OrthographicHeight;
            this.Position = Position;
            this.Rotation = Rotation;
            this.ResolutionX = ResolutionX;
            this.ResolutionY = ResolutionY;
            this.NearPlane = NearPlane;
            this.FarPlane = FarPlane;
        }
    }

    /// <summary>
    /// 3D renderer
    /// </summary>
    public class Renderer
    {
        /// <summary>
        /// Is the renderer running
        /// </summary>
        private bool RendererRunning = false;

        /// <summary>
        /// Rendering API used for 3D rendering
        /// </summary>
        private RenderingAPI renderingAPI;

        /// <summary>
        /// Renderer for the DirectX 11 rendering API
        /// </summary>
        private DirectX11Renderer DX11Renderer;

        /// <summary>
        /// World position offset
        /// </summary>
        internal Vector3 WorldPositionOffset = new Vector3(0);

        /// <summary>
        /// World rotation offset
        /// </summary>
        internal Vector3 WorldRotationOffset = new Vector3(0);

        /// <summary>
        /// World scale multiplyer
        /// </summary>
        internal Vector3 WorldScaleOffset = new Vector3(1);

        /// <summary>
        /// Camera settings for the renderer
        /// </summary>
        internal CameraProperties RenderingCameraProperties = new CameraProperties();

        /// <summary>
        /// Has the renderer started rendering a frame
        /// </summary>
        internal bool frameStarted = false;

        /// <summary>
        /// Texture paths
        /// </summary>
        internal List<string> Textures = new List<string>();

        /// <summary>
        /// Imported textures for the DirectX 11 rendering API
        /// </summary>
        internal List<Texture2D> DirectX11ImportTextures = new List<Texture2D>();

        /// <summary>
        /// Known light sources to the renderer
        /// </summary>
        internal List<Light> Lights = new List<Light>();

        internal Mesh Triangle = new Mesh(
            new List<Vertex>()
            {
                new Vertex(new Vector3(-0.5f, -0.5f, 0), new Vector2(0, 0)),
                new Vertex(new Vector3(0f, 0.5f, 0), new Vector2(0.5f, 1)),
                new Vertex(new Vector3(0.5f, -0.5f, 0), new Vector2(1, 0))
            },

            new List<Polygon>
            {
                new Polygon(0, 1, 2)
            });

        internal Mesh Quadrangle = new Mesh(
            new List<Vertex>()
            {
                new Vertex(new Vector3(-0.5f, -0.5f, 0), new Vector2(0, 0)),
                new Vertex(new Vector3(-0.5f, 0.5f, 0), new Vector2(0, 1)),
                new Vertex(new Vector3(0.5f, 0.5f, 0), new Vector2(1, 1)),
                new Vertex(new Vector3(0.5f, -0.5f, 0), new Vector2(1, 0))
            }, 
            
            new List<Polygon>
            {
                new Polygon(0, 1, 2),
                new Polygon(0, 2, 3)
            });

        internal Mesh Plane = new Mesh(
            new List<Vertex>()
            {
                new Vertex(new Vector3(-0.5f, 0, -0.5f), new Vector2(0, 0)),
                new Vertex(new Vector3(-0.5f, 0, 0.5f), new Vector2(0, 1)),
                new Vertex(new Vector3(0.5f, 0, 0.5f), new Vector2(1, 1)),
                new Vertex(new Vector3(0.5f, 0, -0.5f), new Vector2(1, 0))
            }, 
            
            new List<Polygon>
            {
                new Polygon(0, 1, 2),
                new Polygon(0, 2, 3)
            });

        internal Mesh Cube = new Mesh(
            new List<Vertex>()
            {
                new Vertex(new Vector3(-0.5f, -0.5f, 0.5f), new Vector2(1, 1)),
                new Vertex(new Vector3(-0.5f, 0.5f, 0.5f), new Vector2(1, 0)),
                new Vertex(new Vector3(0.5f, 0.5f, 0.5f), new Vector2(0, 1)),
                new Vertex(new Vector3(0.5f, -0.5f, 0.5f), new Vector2(0, 0)),
                new Vertex(new Vector3(-0.5f, -0.5f, -0.5f), new Vector2(0, 0)),
                new Vertex(new Vector3(-0.5f, 0.5f, -0.5f), new Vector2(0, 1)),
                new Vertex(new Vector3(0.5f, 0.5f, -0.5f), new Vector2(1, 0)),
                new Vertex(new Vector3(0.5f, -0.5f, -0.5f), new Vector2(1, 1)),
                new Vertex(new Vector3(-0.5f, 0.5f, 0.5f), new Vector2(0, 1)),
                new Vertex(new Vector3(0.5f, 0.5f, -0.5f), new Vector2(1, 0)),
                new Vertex(new Vector3(-0.5f, 0.5f, -0.5f), new Vector2(0, 0)),
                new Vertex(new Vector3(0.5f, 0.5f, 0.5f), new Vector2(1, 1)),
                new Vertex(new Vector3(-0.5f, -0.5f, 0.5f), new Vector2(0, 1)),
                new Vertex(new Vector3(0.5f, -0.5f, -0.5f), new Vector2(1, 0)),
                new Vertex(new Vector3(-0.5f, -0.5f, -0.5f), new Vector2(0, 0)),
                new Vertex(new Vector3(0.5f, -0.5f, 0.5f), new Vector2(1, 1))
            },

            new List<Polygon>()
            {
                new Polygon(0, 2, 1),
                new Polygon(0, 3, 2),
                new Polygon(3, 6, 2),
                new Polygon(3, 7, 6),
                new Polygon(7, 5, 6),
                new Polygon(7, 4, 5),
                new Polygon(4, 1, 5),
                new Polygon(4, 0, 1),
                new Polygon(8, 9, 10),
                new Polygon(8, 11, 9),
                new Polygon(12, 14, 13),
                new Polygon(12, 13, 15)
            });

        internal Mesh Sphere = new Mesh();

        public Renderer()
        {
            Triangle.GenerateNormals();
            Quadrangle.GenerateNormals();
            Plane.GenerateNormals();
            Cube.GenerateNormals();
            Sphere.GenerateNormals();
        }

        /// <summary>
        /// Set the rendering API that is supposed to be useded
        /// </summary>
        /// <param name="API"> The target rendering API </param>
        public bool SetRenderingAPI(RenderingAPI API)
        {
            if(RendererRunning)
            {
                StopRenderer();
            }

            renderingAPI = API;
            if (frameStarted)
            {
                RemoveFrame();
            }

            if (renderingAPI == RenderingAPI.DirectX11)
            {
                DX11Renderer = new DirectX11Renderer(this);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Setup/rebuild the renderer
        /// </summary>
        /// <param name="Handle"> Handle to render on to </param>
        public void StartRenderer(IntPtr Handle)
        {
            if(RendererRunning)
            {
                StopRenderer();
            }

            if(renderingAPI == RenderingAPI.DirectX11)
            {
                DX11Renderer.SetupRenderer(Handle);
            }

            RendererRunning = true;
        }

        /// <summary>
        /// Se if the renderer is active
        /// </summary>
        public bool IsRendererStarted()
        {
            return RendererRunning;
        }

        #region Camera
        /// <summary>
        /// Sets camera position
        /// </summary>
        /// <param name="position"> The desired position of the camera </param>
        public void SetCameraPosition(Vector3 position)
        {
            RenderingCameraProperties.Position = position;

            UppdateCamera();
        }

        /// <summary>
        /// Sets camera rotation
        /// </summary>
        /// <param name="rotation"> The desired rotation of the camera </param>
        public void SetCameraRotation(Vector3 rotation)
        {
            RenderingCameraProperties.Rotation = rotation;

            UppdateCamera();
        }

        /// <summary>
        /// Sets camera position and rotation
        /// </summary>
        /// <param name="position"> The desired position of the camera </param>
        /// <param name="rotation"> The desired rotation of the camera </param>
        public void SetCameraPositionAndRotation(Vector3 position, Vector3 rotation)
        {
            RenderingCameraProperties.Position = position;
            RenderingCameraProperties.Rotation = rotation;

            UppdateCamera();
        }

        /// <summary>
        /// Sets camera render resolution
        /// </summary>
        /// <param name="resolution"> The desired resolution of the camera </param>
        public void SetCameraResolution(Vector2 resolution)
        {
            RenderingCameraProperties.ResolutionX = (int)resolution.X;
            RenderingCameraProperties.ResolutionY = (int)resolution.Y;

            UppdateCamera();
        }

        /// <summary>
        /// Sets camera render resolution
        /// </summary>
        /// <param name="resolutionX"> The desired resolution of the camera on the X axis</param>
        /// <param name="resolutionY"> The desired resolution of the camera on the Y axis </param>
        public void SetCameraResolution(int resolutionX, int resolutionY)
        {
            RenderingCameraProperties.ResolutionX = resolutionX;
            RenderingCameraProperties.ResolutionY = resolutionY;

            UppdateCamera();
        }

        /// <summary>
        /// Sets camera into perspective mode and desired fov
        /// </summary>
        /// <param name="FOV"> The desired FOV of the camera </param>
        public void SetCameraPerspectiveMode(float FOV)
        {
            RenderingCameraProperties.Perspective = false;
            RenderingCameraProperties.FOV = FOV;

            UppdateCamera();
        }

        /// <summary>
        /// Sets camera into orthographic mode and desired size
        /// </summary>
        /// <param name="size"> The desired orthographic sensor size of the camera</param>
        public void SetCameraOrthographicMode(Vector2 size)
        {
            RenderingCameraProperties.Perspective = false;
            RenderingCameraProperties.OrthographicWidth = size.X;
            RenderingCameraProperties.OrthographicHeight = size.Y;

            UppdateCamera();
        }

        /// <summary>
        /// Sets camera into orthographic mode and desired size
        /// </summary>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        public void SetCameraOrthographicMode(float Width, float Height)
        {
            RenderingCameraProperties.Perspective = false;
            RenderingCameraProperties.OrthographicWidth = Width;
            RenderingCameraProperties.OrthographicHeight = Height;

            UppdateCamera();
        }

        /// <summary>
        /// Sets camera render distance (X = NearPlane, Y = FarPlane)
        /// </summary>
        /// <param name="planes"> The desired render distances of the camera </param>
        public void SetCameraRenderDistance(Vector2 planes)
        {
            RenderingCameraProperties.NearPlane = (int)planes.X;
            RenderingCameraProperties.FarPlane = (int)planes.Y;

            UppdateCamera();
        }

        /// <summary>
        /// Sets camera render distance
        /// </summary>
        /// <param name="nearPlane"> The desired near render distance of the camera </param>
        /// <param name="farPlane"> The desired far render distance of the camera </param>
        public void SetCameraRenderDistance(float nearPlane, float farPlane)
        {
            RenderingCameraProperties.NearPlane = nearPlane;
            RenderingCameraProperties.FarPlane = farPlane;

            UppdateCamera();
        }

        /// <summary>
        /// Sets camera properties
        /// </summary>
        /// <param name="cameraProperties"> desired camera properties</param>
        public void SetCameraProperties(CameraProperties cameraProperties)
        {
            RenderingCameraProperties = cameraProperties;

            UppdateCamera();
        }

        /// <summary>
        /// Get the camera position
        /// </summary>
        public Vector3 GetCameraPosition()
        {
            return RenderingCameraProperties.Position;
        }

        /// <summary>
        /// Get the camera rotation
        /// </summary>
        public Vector3 GetCameraRotation()
        {
            return RenderingCameraProperties.Rotation;
        }

        /// <summary>
        /// Get the camera render resolution
        /// </summary>
        public Vector2 GetCameraResolution()
        {
            return new Vector2(RenderingCameraProperties.ResolutionX, RenderingCameraProperties.ResolutionY);
        }

        /// <summary>
        /// Get to know if the camera is in perspective render mode
        /// </summary>
        public bool GetCameraPerspectiveMode()
        {
            return RenderingCameraProperties.Perspective;
        }

        /// <summary>
        /// Get the camera perspective render FOV
        /// </summary>
        public float GetCameraPerspectiveFOV()
        {
            return RenderingCameraProperties.FOV;
        }

        /// <summary>
        /// Get to know if the camera is in orthographic render mode
        /// </summary>
        public bool GetCameraOrthographicMode()
        {
            return !RenderingCameraProperties.Perspective;
        }

        /// <summary>
        /// Get the camera orthographic render size
        /// </summary>
        public Vector2 GetCameraOrthographicSize()
        {
            return new Vector2(RenderingCameraProperties.OrthographicWidth, RenderingCameraProperties.OrthographicHeight);
        }

        /// <summary>
        /// Get the camera render distance (X = NearPlane, Y = FarPlane)
        /// </summary>
        public Vector2 GetCameraRenderDistance()
        {
            return new Vector2(RenderingCameraProperties.NearPlane, RenderingCameraProperties.FarPlane);
        }

        /// <summary>
        /// Get the camera properties
        /// </summary>
        public CameraProperties GetCameraProperties()
        {
            return RenderingCameraProperties;
        }

        /// <summary>
        /// Uppdate render camera projection after render carmera value change
        /// </summary>
        private void UppdateCamera()
        {
            if (renderingAPI == RenderingAPI.DirectX11)
            {
                DX11Renderer.UppdateCamera();
            }
        }
        #endregion

        #region Texture
        /// <summary>
        /// Add a textrue to the renderer
        /// </summary>
        /// <param name="texture"> The texture path to be added </param>
        public void AddTexture(string texture)
        {
            Textures.Add(texture);

            if (renderingAPI == RenderingAPI.DirectX11)
            {
                DirectX11ImportTextures.Add(Convert.BitmapToDX11Texture(DX11Renderer.device, texture));
            }
        }

        /// <summary>
        /// Add textrues to the renderer
        /// </summary>
        /// <param name="textures"> The texture paths to be set </param>
        public void AddTextures(string[] textures)
        {
            Textures.AddRange(textures);

            for (int i = 0; i < textures.Length; i++)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DirectX11ImportTextures.Add(Convert.BitmapToDX11Texture(DX11Renderer.device, textures[i]));
                }
            }
        }

        /// <summary>
        /// Add textrues to the renderer
        /// </summary>
        /// <param name="textures"> The texture paths to be set </param>
        public void AddTextures(List<string> textures)
        {
            Textures.AddRange(textures);

            for (int i = 0; i < textures.Count; i++)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DirectX11ImportTextures.Add(Convert.BitmapToDX11Texture(DX11Renderer.device, textures[i]));
                }
            }
        }

        /// <summary>
        /// Sets the textures to be used by the renderer
        /// </summary>
        /// <param name="texture"> The texture paths to be set </param>
        public void SetTextures(string texture)
        {
            Textures.Clear();
            Textures.Add(texture);

            DirectX11ImportTextures = new List<Texture2D>();

            if (renderingAPI == RenderingAPI.DirectX11)
            {
                DirectX11ImportTextures.Add(Convert.BitmapToDX11Texture(DX11Renderer.device, texture));
            }
        }

        /// <summary>
        /// Sets the textures to be used by the renderer
        /// </summary>
        /// <param name="textures"> The texture paths to be set </param>
        public void SetTextures(string[] textures)
        {
            Textures.Clear();
            Textures.AddRange(textures);

            DirectX11ImportTextures = new List<Texture2D>();

            for (int i = 0; i < textures.Length; i++)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DirectX11ImportTextures.Add(Convert.BitmapToDX11Texture(DX11Renderer.device, textures[i]));
                }
            }
        }

        /// <summary>
        /// Sets the textures to be used by the renderer
        /// </summary>
        /// <param name="textures"> The texture paths to be set </param>
        public void SetTextures(List<string> textures)
        {
            Textures = textures;

            DirectX11ImportTextures = new List<Texture2D>();

            for (int i = 0; i < textures.Count; i++)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DirectX11ImportTextures.Add(Convert.BitmapToDX11Texture(DX11Renderer.device, textures[i]));
                }
            }
        }

        /// <summary>
        /// Removes a texture from the renderer
        /// </summary>
        /// <param name="ID"></param>
        public void RemoveTexture(int ID)
        {
            Textures.RemoveAt(ID);
            DirectX11ImportTextures.RemoveAt(ID);
        }

        /// <summary>
        /// Clears all textures from the renderer
        /// </summary>
        public void ClearTextures()
        {
            Textures.Clear();
            DirectX11ImportTextures.Clear();
        }
        #endregion

        #region Lights
        /// <summary>
        /// Set lightsources for the renderer
        /// </summary>
        /// <param name="lights"> The lightsources </param>
        public bool SetLights(List<Light> lights)
        {
            if (!frameStarted)
            {
                Lights.Clear();

                for (int i = 0; i < lights.Count; i++)
                {
                    Lights.Add(new Light(lights[i]));
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Add a lightsource to the renderer
        /// </summary>
        /// <param name="light"> The lightsource </param>
        public bool AddLight(Light light)
        {
            if (!frameStarted)
            {
                Lights.Add(new Light(light));

                return true;
            }

            return false;
        }

        /// <summary>
        /// Add lightsources to the renderer
        /// </summary>
        /// <param name="lights"> The lightsources  </param>
        public bool AddLight(List<Light> lights)
        {
            if (!frameStarted)
            {
                for (int i = 0; i < lights.Count; i++)
                {
                    Lights.Add(new Light(lights[i]));
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Add lightsources to the renderer
        /// </summary>
        /// <param name="lights"> The lightsources  </param>
        public bool AddLight(Light[] lights)
        {
            if (!frameStarted)
            {
                for (int i = 0; i < lights.Length; i++)
                {
                    Lights.Add(new Light(lights[i]));
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Uppdate a lightsources in the renderer
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="light"> The lightsource </param>
        public bool UppdateLight(int ID, Light light)
        {
            if (!frameStarted)
            {
                if (Lights.Count > ID)
                {
                    Lights[ID] = new Light(light);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Remove all lightsources from the renderer
        /// </summary>
        public bool ClearLights()
        {
            if (!frameStarted)
            {
                Lights.Clear();

                return true;
            }

            return false;
        }
        #endregion

        /// <summary>
        /// Tells the renderer to start a new frame
        /// </summary>
        public bool StartFrame()
        {
            if (!frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.StartFrame();
                    frameStarted = true;
                }
            }

            return false;
        }

        #region Draw Mesh
        /// <summary>
        /// Draw a mesh
        /// </summary>
        /// <param name="mesh"> Mesh to draw </param>
        public bool DrawMesh(Mesh mesh)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawMesh(mesh, new Material(), new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a mesh
        /// </summary>
        /// <param name="mesh"> Mesh to draw </param>
        /// <param name="material"> Material to draw mesh with </param>
        public bool DrawMesh(Mesh mesh, Material material)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawMesh(mesh, material,new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a mesh
        /// </summary>
        /// <param name="mesh"> Mesh to draw </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        public bool DrawMesh(Mesh mesh, Vector3 position)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawMesh(mesh, new Material(), new Vector3[] { position }, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a mesh
        /// </summary>
        /// <param name="mesh"> Mesh to draw </param>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        public bool DrawMesh(Mesh mesh, Material material, Vector3 position)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawMesh(mesh, material, new Vector3[] { position }, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a mesh
        /// </summary>
        /// <param name="mesh"> Mesh to draw </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        public bool DrawMesh(Mesh mesh, Vector3 position, Vector3 rotation)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawMesh(mesh, new Material(), new Vector3[] { position }, new Vector3[] { rotation }, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a mesh
        /// </summary>
        /// <param name="mesh"> Mesh to draw </param>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        public bool DrawMesh(Mesh mesh, Material material, Vector3 position, Vector3 rotation)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawMesh(mesh, material, new Vector3[] { position }, new Vector3[] { rotation }, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a mesh
        /// </summary>
        /// <param name="mesh"> Mesh to draw </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        public bool DrawMesh(Mesh mesh, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawMesh(mesh, new Material(), new Vector3[] { position }, new Vector3[] { rotation }, new Vector3[] { scale });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a mesh
        /// </summary>
        /// <param name="mesh"> Mesh to draw </param>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        public bool DrawMesh(Mesh mesh, Material material, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawMesh(mesh, material, new Vector3[] { position }, new Vector3[] { rotation }, new Vector3[] { scale });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a mesh
        /// </summary>
        /// <param name="mesh"> Mesh to draw </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        public bool DrawMesh(Mesh mesh, Vector3[] position)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawMesh(mesh, new Material(), position, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a mesh
        /// </summary>
        /// <param name="mesh"> Mesh to draw </param>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        public bool DrawMesh(Mesh mesh, Material material, Vector3[] position)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawMesh(mesh, material, position, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a mesh
        /// </summary>
        /// <param name="mesh"> Mesh to draw </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        public bool DrawMesh(Mesh mesh, Vector3[] position, Vector3[] rotation)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawMesh(mesh, new Material(), position, rotation, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a mesh
        /// </summary>
        /// <param name="mesh"> Mesh to draw </param>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        public bool DrawMesh(Mesh mesh, Material material, Vector3[] position, Vector3[] rotation)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawMesh(mesh, material, position, rotation, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a mesh
        /// </summary>
        /// <param name="mesh"> Mesh to draw </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        public bool DrawMesh(Mesh mesh, Vector3[] position, Vector3[] rotation, Vector3[] scale)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawMesh(mesh, new Material(), position, rotation, scale);
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a mesh
        /// </summary>
        /// <param name="mesh"> Mesh to draw </param>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        public bool DrawMesh(Mesh mesh, Material material, Vector3[] position, Vector3[] rotation, Vector3[] scale)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawMesh(mesh, material, position, rotation, scale);
                }
            }

            return false;
        }
        #endregion

        #region Draw Triangle
        /// <summary>
        /// Draw a triangle
        /// </summary>
        public bool DrawTriangle()
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawTriangle(new Material(),new Vector3[] { new Vector3(0) },new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a triangle
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        public bool DrawTriangle(Material material)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawTriangle(material,new Vector3[] { new Vector3(0) },new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a triangle
        /// </summary>
        /// <param name="position"></param>
        public bool DrawTriangle(Vector3 position)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawTriangle(new Material(), new Vector3[] { position },new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a triangle
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        public bool DrawTriangle(Material material, Vector3 position)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawTriangle(material, new Vector3[] { position },new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a triangle
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        public bool DrawTriangle(Vector3 position, Vector3 rotation)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawTriangle(new Material(), new Vector3[] { position }, new Vector3[] { rotation }, new Vector3[] { new Vector3(1) });
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
        public bool DrawTriangle(Material material, Vector3 position, Vector3 rotation)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawTriangle(material, new Vector3[] { position }, new Vector3[] { rotation }, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a triangle
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        public bool DrawTriangle(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawTriangle(new Material(), new Vector3[] { position }, new Vector3[] { rotation }, new Vector3[] { scale });
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
        public bool DrawTriangle(Material material, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawTriangle(material, new Vector3[] { position }, new Vector3[] { rotation }, new Vector3[] { scale });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a triangle
        /// </summary>
        /// <param name="position"></param>
        public bool DrawTriangle(Vector3[] position)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawTriangle(new Material(), position, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a triangle
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        public bool DrawTriangle(Material material, Vector3[] position)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawTriangle(material, position, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a triangle
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        public bool DrawTriangle(Vector3[] position, Vector3[] rotation)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawTriangle(new Material(), position, rotation, new Vector3[] { new Vector3(1) });
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
        public bool DrawTriangle(Material material, Vector3[] position, Vector3[] rotation)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawTriangle(material, position, rotation, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a triangle
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        public bool DrawTriangle(Vector3[] position, Vector3[] rotation, Vector3[] scale)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawTriangle(new Material(), position, rotation, scale);
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
        public bool DrawTriangle(Material material, Vector3[] position, Vector3[] rotation, Vector3[] scale)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawTriangle(material, position, rotation, scale);
                }
            }

            return false;
        }
        #endregion

        #region Draw Quad
        /// <summary>
        /// Draw a quad
        /// </summary>
        public bool DrawQuad()
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawQuad(new Material(),new Vector3[] { new Vector3(0) },new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a quad
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        public bool DrawQuad(Material material)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawQuad(material,new Vector3[] { new Vector3(0) },new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a quad
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        public bool DrawQuad(Vector3 position)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawQuad(new Material(), new Vector3[] { position },new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a quad
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        public bool DrawQuad(Material material, Vector3 position)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawQuad(material, new Vector3[] { position },new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a quad
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        public bool DrawQuad(Vector3 position, Vector3 rotation)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawQuad(new Material(), new Vector3[] { position }, new Vector3[] { rotation }, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a quad
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        public bool DrawQuad(Material material, Vector3 position, Vector3 rotation)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawQuad(material, new Vector3[] { position }, new Vector3[] { rotation }, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a quad
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        public bool DrawQuad(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawQuad(new Material(), new Vector3[] { position }, new Vector3[] { rotation }, new Vector3[] { scale });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a quad
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        public bool DrawQuad(Material material, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawQuad(material, new Vector3[] { position }, new Vector3[] { rotation }, new Vector3[] { scale });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a quad
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        public bool DrawQuad(Vector3[] position)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawQuad(new Material(), position, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a quad
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        public bool DrawQuad(Material material, Vector3[] position)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawQuad(material, position, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a quad
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        public bool DrawQuad(Vector3[] position, Vector3[] rotation)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawQuad(new Material(), position, rotation, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a quad
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        public bool DrawQuad(Material material, Vector3[] position, Vector3[] rotation)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawQuad(material, position, rotation, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a quad
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        public bool DrawQuad(Vector3[] position, Vector3[] rotation, Vector3[] scale)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawQuad(new Material(), position, rotation, scale);
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a quad
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        public bool DrawQuad(Material material, Vector3[] position, Vector3[] rotation, Vector3[] scale)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawQuad(material, position, rotation, scale);
                }
            }

            return false;
        }
        #endregion

        #region Draw Plane
        /// <summary>
        /// Draw a plane
        /// </summary>
        public bool DrawPlane()
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawPlane(new Material(),new Vector3[] { new Vector3(0) },new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a plane
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        public bool DrawPlane(Material material)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawPlane(material,new Vector3[] { new Vector3(0) },new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a plane
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        public bool DrawPlane(Vector3 position)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawPlane(new Material(), new Vector3[] { position },new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a plane
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        public bool DrawPlane(Material material, Vector3 position)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawPlane(material, new Vector3[] { position },new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a plane
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        public bool DrawPlane(Vector3 position, Vector3 rotation)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawPlane(new Material(), new Vector3[] { position }, new Vector3[] { rotation }, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a plane
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        public bool DrawPlane(Material material, Vector3 position, Vector3 rotation)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawPlane(material, new Vector3[] { position }, new Vector3[] { rotation }, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a plane
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        public bool DrawPlane(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawPlane(new Material(), new Vector3[] { position }, new Vector3[] { rotation }, new Vector3[] { scale });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a plane
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        public bool DrawPlane(Material material, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawPlane(material, new Vector3[] { position }, new Vector3[] { rotation }, new Vector3[] { scale });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a plane
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        public bool DrawPlane(Vector3[] position)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawPlane(new Material(), position, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a plane
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        public bool DrawPlane(Material material, Vector3[] position)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawPlane(material, position, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a plane
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        public bool DrawPlane(Vector3[] position, Vector3[] rotation)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawPlane(new Material(), position, rotation, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a plane
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        public bool DrawPlane(Material material, Vector3[] position, Vector3[] rotation)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawPlane(material, position, rotation, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a plane
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        public bool DrawPlane(Vector3[] position, Vector3[] rotation, Vector3[] scale)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawPlane(new Material(), position, rotation, scale);
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a plane
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        public bool DrawPlane(Material material, Vector3[] position, Vector3[] rotation, Vector3[] scale)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawPlane(material, position, rotation, scale);
                }
            }

            return false;
        }
        #endregion

        #region Draw Cube
        /// <summary>
        /// Draw a cube
        /// </summary>
        public bool DrawCube()
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawCube(new Material(),new Vector3[] { new Vector3(0) },new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a cube
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        public bool DrawCube(Material material)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawCube(material,new Vector3[] { new Vector3(0) },new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a cube
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        public bool DrawCube(Vector3 position)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawCube(new Material(), new Vector3[] { position },new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a cube
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        public bool DrawCube(Material material, Vector3 position)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawCube(material, new Vector3[] { position },new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a cube
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        public bool DrawCube(Vector3 position, Vector3 rotation)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawCube(new Material(), new Vector3[] { position }, new Vector3[] { rotation }, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a cube
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        public bool DrawCube(Material material, Vector3 position, Vector3 rotation)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawCube(material, new Vector3[] { position }, new Vector3[] { rotation }, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a cube
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        public bool DrawCube(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawCube(new Material(), new Vector3[] { position }, new Vector3[] { rotation }, new Vector3[] { scale });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a cube
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        public bool DrawCube(Material material, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawCube(material, new Vector3[] { position }, new Vector3[] { rotation }, new Vector3[] { scale });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a cube
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        public bool DrawCube(Vector3[] position)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawCube(new Material(), position, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a cube
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        public bool DrawCube(Material material, Vector3[] position)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawCube(material, position, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a cube
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        public bool DrawCube(Vector3[] position, Vector3[] rotation)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawCube(new Material(), position, rotation, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a cube
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        public bool DrawCube(Material material, Vector3[] position, Vector3[] rotation)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawCube(material, position, rotation, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a cube
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        public bool DrawCube(Vector3[] position, Vector3[] rotation, Vector3[] scale)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawCube(new Material(), position, rotation, scale);
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a cube
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        public bool DrawCube(Material material, Vector3[] position, Vector3[] rotation, Vector3[] scale)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawCube(material, position, rotation, scale);
                }
            }

            return false;
        }
        #endregion

        #region Draw Sphere
        /// <summary>
        /// Draw a sphere
        /// </summary>
        public bool DrawSphere()
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawSphere(new Material(),new Vector3[] { new Vector3(0) },new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a sphere
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        public bool DrawSphere(Material material)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawSphere(material,new Vector3[] { new Vector3(0) },new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a sphere
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        public bool DrawSphere(Vector3 position)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawSphere(new Material(), new Vector3[] { position },new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a sphere
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        public bool DrawSphere(Material material, Vector3 position)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawSphere(material, new Vector3[] { position },new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a sphere
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        public bool DrawSphere(Vector3 position, Vector3 rotation)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawSphere(new Material(), new Vector3[] { position }, new Vector3[] { rotation }, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a sphere
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        public bool DrawSphere(Material material, Vector3 position, Vector3 rotation)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawSphere(material, new Vector3[] { position }, new Vector3[] { rotation }, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a sphere
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        public bool DrawSphere(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawSphere(new Material(), new Vector3[] { position }, new Vector3[] { rotation }, new Vector3[] { scale });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a sphere
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        public bool DrawSphere(Material material, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawSphere(material, new Vector3[] { position }, new Vector3[] { rotation }, new Vector3[] { scale });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a sphere
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        public bool DrawSphere(Vector3[] position)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawSphere(new Material(), position, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a sphere
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        public bool DrawSphere(Material material, Vector3[] position)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawSphere(material, position, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a sphere
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        public bool DrawSphere(Vector3[] position, Vector3[] rotation)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawSphere(new Material(), position, rotation, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a sphere
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        public bool DrawSphere(Material material, Vector3[] position, Vector3[] rotation)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawSphere(material, position, rotation, new Vector3[] { new Vector3(1) });
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a sphere
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        public bool DrawSphere(Vector3[] position, Vector3[] rotation, Vector3[] scale)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawSphere(new Material(), position, rotation, scale);
                }
            }

            return false;
        }

        /// <summary>
        /// Draw a sphere
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        public bool DrawSphere(Material material, Vector3[] position, Vector3[] rotation, Vector3[] scale)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawSphere(material, position, rotation, scale);
                }
            }

            return false;
        }
        #endregion

        public bool DrawIcon()
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    return DX11Renderer.DrawIcon();
                }
            }

            return false;
        }

        #region Draw Mesh Depthless
        /// <summary>
        /// Draw a mesh depthless
        /// </summary>
        /// <param name="mesh"> Mesh to draw </param>
        public void DrawMeshDepthless(Mesh mesh)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawMeshDepthless(mesh, new Material(), new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a mesh depthless
        /// </summary>
        /// <param name="mesh"> Mesh to draw </param>
        /// <param name="material"> Material to draw mesh with </param>
        public void DrawMeshDepthless(Mesh mesh, Material material)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawMeshDepthless(mesh, material, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a mesh depthless
        /// </summary>
        /// <param name="mesh"> Mesh to draw </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        public void DrawMeshDepthless(Mesh mesh, Vector3 position)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawMeshDepthless(mesh, new Material(), new Vector3[] { position }, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a mesh depthless
        /// </summary>
        /// <param name="mesh"> Mesh to draw </param>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        public void DrawMeshDepthless(Mesh mesh, Material material, Vector3 position)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawMeshDepthless(mesh, material, new Vector3[] { position }, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a mesh depthless
        /// </summary>
        /// <param name="mesh"> Mesh to draw </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        public void DrawMeshDepthless(Mesh mesh, Vector3 position, Vector3 rotation)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawMeshDepthless(mesh, new Material(), new Vector3[] { position }, new Vector3[] { rotation }, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a mesh depthless
        /// </summary>
        /// <param name="mesh"> Mesh to draw </param>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        public void DrawMeshDepthless(Mesh mesh, Material material, Vector3 position, Vector3 rotation)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawMeshDepthless(mesh, material, new Vector3[] { position }, new Vector3[] { rotation }, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a mesh depthless
        /// </summary>
        /// <param name="mesh"> Mesh to draw </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        public void DrawMeshDepthless(Mesh mesh, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawMeshDepthless(mesh, new Material(), new Vector3[] { position }, new Vector3[] { rotation }, new Vector3[] { scale });
                }
            }
        }

        /// <summary>
        /// Draw a mesh depthless
        /// </summary>
        /// <param name="mesh"> Mesh to draw </param>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        public bool DrawMeshDepthless(Mesh mesh, Material material, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawMeshDepthless(mesh, material, new Vector3[] { position }, new Vector3[] { rotation }, new Vector3[] { scale });

                    return true;
                }
            }

            return false;
        }

        public void DrawMeshDepthless(Mesh mesh, Vector3[] position)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawMeshDepthless(mesh, new Material(), position, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a mesh depthless
        /// </summary>
        /// <param name="mesh"> Mesh to draw </param>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        public void DrawMeshDepthless(Mesh mesh, Material material, Vector3[] position)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawMeshDepthless(mesh, material, position, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a mesh depthless
        /// </summary>
        /// <param name="mesh"> Mesh to draw </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        public void DrawMeshDepthless(Mesh mesh, Vector3[] position, Vector3[] rotation)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawMeshDepthless(mesh, new Material(), position, rotation, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a mesh depthless
        /// </summary>
        /// <param name="mesh"> Mesh to draw </param>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        public void DrawMeshDepthless(Mesh mesh, Material material, Vector3[] position, Vector3[] rotation)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawMeshDepthless(mesh, material, position, rotation, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a mesh depthless
        /// </summary>
        /// <param name="mesh"> Mesh to draw </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        public void DrawMeshDepthless(Mesh mesh, Vector3[] position, Vector3[] rotation, Vector3[] scale)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawMeshDepthless(mesh, new Material(), position, rotation, scale);
                }
            }
        }

        /// <summary>
        /// Draw a mesh depthless
        /// </summary>
        /// <param name="mesh"> Mesh to draw </param>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        public bool DrawMeshDepthless(Mesh mesh, Material material, Vector3[] position, Vector3[] rotation, Vector3[] scale)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawMeshDepthless(mesh, material, position, rotation, scale);

                    return true;
                }
            }

            return false;
        }
        #endregion

        #region Draw Triangle Depthless
        /// <summary>
        /// Draw a triangle depthless
        /// </summary>
        public void DrawTriangleDepthless()
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawTriangleDepthless(new Material(), new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a triangle depthless
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        public void DrawTriangleDepthless(Material material)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawTriangleDepthless(material, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a triangle depthless
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        public void DrawTriangleDepthless(Vector3 position)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawTriangleDepthless(new Material(), new Vector3[] { position }, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a triangle depthless
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        public void DrawTriangleDepthless(Material material, Vector3 position)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawTriangleDepthless(material, new Vector3[] { position }, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a triangle depthless
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        public void DrawTriangleDepthless(Vector3 position, Vector3 rotation)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawTriangleDepthless(new Material(), new Vector3[] { position }, new Vector3[] { rotation }, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a triangle depthless
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        public void DrawTriangleDepthless(Material material, Vector3 position, Vector3 rotation)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawTriangleDepthless(material, new Vector3[] { position }, new Vector3[] { rotation }, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a triangle depthless
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        public void DrawTriangleDepthless(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawTriangleDepthless(new Material(), new Vector3[] { position }, new Vector3[] { rotation }, new Vector3[] { scale });
                }
            }
        }

        /// <summary>
        /// Draw a triangle depthless
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        public void DrawTriangleDepthless(Material material, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawTriangleDepthless(material, new Vector3[] { position }, new Vector3[] { rotation }, new Vector3[] { scale });
                }
            }
        }

        /// <summary>
        /// Draw a triangle depthless
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        public void DrawTriangleDepthless(Vector3[] position)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawTriangleDepthless(new Material(), position, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a triangle depthless
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        public void DrawTriangleDepthless(Material material, Vector3[] position)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawTriangleDepthless(material, position, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a triangle depthless
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        public void DrawTriangleDepthless(Vector3[] position, Vector3[] rotation)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawTriangleDepthless(new Material(), position, rotation, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a triangle depthless
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        public void DrawTriangleDepthless(Material material, Vector3[] position, Vector3[] rotation)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawTriangleDepthless(material, position, rotation, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a triangle depthless
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        public void DrawTriangleDepthless(Vector3[] position, Vector3[] rotation, Vector3[] scale)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawTriangleDepthless(new Material(), position, rotation, scale);
                }
            }
        }

        /// <summary>
        /// Draw a triangle depthless
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        public void DrawTriangleDepthless(Material material, Vector3[] position, Vector3[] rotation, Vector3[] scale)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawTriangleDepthless(material, position, rotation, scale);
                }
            }
        }
        #endregion

        #region Draw Quad Depthless
        /// <summary>
        /// Draw a quad depthless
        /// </summary>
        public void DrawQuadDepthless()
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawQuadDepthless(new Material(), new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a quad depthless
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        public void DrawQuadDepthless(Material material)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawQuadDepthless(material, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a quad depthless
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        public void DrawQuadDepthless(Vector3 position)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawQuadDepthless(new Material(), new Vector3[] { position }, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a quad depthless
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        public void DrawQuadDepthless(Material material, Vector3 position)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawQuadDepthless(material, new Vector3[] { position }, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a quad depthless
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        public void DrawQuadDepthless(Vector3 position, Vector3 rotation)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawQuadDepthless(new Material(), new Vector3[] { position }, new Vector3[] { rotation }, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a quad depthless
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        public void DrawQuadDepthless(Material material, Vector3 position, Vector3 rotation)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawQuadDepthless(material, new Vector3[] { position }, new Vector3[] { rotation }, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a quad depthless
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        public void DrawQuadDepthless(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawQuadDepthless(new Material(), new Vector3[] { position }, new Vector3[] { rotation }, new Vector3[] { scale });
                }
            }
        }

        /// <summary>
        /// Draw a quad depthless
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        public void DrawQuadDepthless(Material material, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawQuadDepthless(material, new Vector3[] { position }, new Vector3[] { rotation }, new Vector3[] { scale });
                }
            }
        }

        /// <summary>
        /// Draw a quad depthless
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        public void DrawQuadDepthless(Vector3[] position)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawQuadDepthless(new Material(), position, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a quad depthless
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        public void DrawQuadDepthless(Material material, Vector3[] position)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawQuadDepthless(material, position, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a quad depthless
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        public void DrawQuadDepthless(Vector3[] position, Vector3[] rotation)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawQuadDepthless(new Material(), position, rotation, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a quad depthless
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        public void DrawQuadDepthless(Material material, Vector3[] position, Vector3[] rotation)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawQuadDepthless(material, position, rotation, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a quad depthless
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        public void DrawQuadDepthless(Vector3[] position, Vector3[] rotation, Vector3[] scale)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawQuadDepthless(new Material(), position, rotation, scale);
                }
            }
        }

        /// <summary>
        /// Draw a quad depthless
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        public void DrawQuadDepthless(Material material, Vector3[] position, Vector3[] rotation, Vector3[] scale)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawQuadDepthless(material, position, rotation, scale);
                }
            }
        }
        #endregion

        #region Draw Plane Depthless
        /// <summary>
        /// Draw a plane depthless
        /// </summary>
        public void DrawPlaneDepthless()
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawPlaneDepthless(new Material(), new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a plane depthless
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        public void DrawPlaneDepthless(Material material)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawPlaneDepthless(material, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a plane depthless
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        public void DrawPlaneDepthless(Vector3 position)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawPlaneDepthless(new Material(), new Vector3[] { position }, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a plane depthless
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        public void DrawPlaneDepthless(Material material, Vector3 position)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawPlaneDepthless(material, new Vector3[] { position }, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a plane depthless
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        public void DrawPlaneDepthless(Vector3 position, Vector3 rotation)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawPlaneDepthless(new Material(), new Vector3[] { position }, new Vector3[] { rotation }, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a plane depthless
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        public void DrawPlaneDepthless(Material material, Vector3 position, Vector3 rotation)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawPlaneDepthless(material, new Vector3[] { position }, new Vector3[] { rotation }, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a plane depthless
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        public void DrawPlaneDepthless(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawPlaneDepthless(new Material(), new Vector3[] { position }, new Vector3[] { rotation }, new Vector3[] { scale });
                }
            }
        }

        /// <summary>
        /// Draw a plane depthless
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        public void DrawPlaneDepthless(Material material, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawPlaneDepthless(material, new Vector3[] { position }, new Vector3[] { rotation }, new Vector3[] { scale });
                }
            }
        }

        /// <summary>
        /// Draw a plane depthless
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        public void DrawPlaneDepthless(Vector3[] position)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawPlaneDepthless(new Material(), position, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a plane depthless
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        public void DrawPlaneDepthless(Material material, Vector3[] position)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawPlaneDepthless(material, position, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a plane depthless
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        public void DrawPlaneDepthless(Vector3[] position, Vector3[] rotation)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawPlaneDepthless(new Material(), position, rotation, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a plane depthless
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        public void DrawPlaneDepthless(Material material, Vector3[] position, Vector3[] rotation)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawPlaneDepthless(material, position, rotation, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a plane depthless
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        public void DrawPlaneDepthless(Vector3[] position, Vector3[] rotation, Vector3[] scale)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawPlaneDepthless(new Material(), position, rotation, scale);
                }
            }
        }

        /// <summary>
        /// Draw a plane depthless
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        public void DrawPlaneDepthless(Material material, Vector3[] position, Vector3[] rotation, Vector3[] scale)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawPlaneDepthless(material, position, rotation, scale);
                }
            }
        }
        #endregion

        #region Draw Cube Depthless
        /// <summary>
        /// Draw a cube depthless
        /// </summary>
        public void DrawCubeDepthless()
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawCubeDepthless(new Material(), new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a cube depthless
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        public void DrawCubeDepthless(Material material)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawCubeDepthless(material, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a cube depthless
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        public void DrawCubeDepthless(Vector3 position)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawCubeDepthless(new Material(), new Vector3[] { position }, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a cube depthless
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        public void DrawCubeDepthless(Material material, Vector3 position)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawCubeDepthless(material, new Vector3[] { position },new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a cube depthless
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        public void DrawCubeDepthless(Vector3 position, Vector3 rotation)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawCubeDepthless(new Material(), new Vector3[] { position }, new Vector3[] { rotation }, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a cube depthless
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        public void DrawCubeDepthless(Material material, Vector3 position, Vector3 rotation)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawCubeDepthless(material, new Vector3[] { position }, new Vector3[] { rotation }, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a cube depthless
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        public void DrawCubeDepthless(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawCubeDepthless(new Material(), new Vector3[] { position }, new Vector3[] { rotation }, new Vector3[] { scale });
                }
            }
        }

        /// <summary>
        /// Draw a cube depthless
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        public void DrawCubeDepthless(Material material, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawCubeDepthless(material, new Vector3[] { position }, new Vector3[] { rotation }, new Vector3[] { scale });
                }
            }
        }

        /// <summary>
        /// Draw a cube depthless
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        public void DrawCubeDepthless(Vector3[] position)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawCubeDepthless(new Material(), position, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a cube depthless
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        public void DrawCubeDepthless(Material material, Vector3[] position)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawCubeDepthless(material, position, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a cube depthless
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        public void DrawCubeDepthless(Vector3[] position, Vector3[] rotation)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawCubeDepthless(new Material(), position, rotation, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a cube depthless
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        public void DrawCubeDepthless(Material material, Vector3[] position, Vector3[] rotation)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawCubeDepthless(material, position, rotation, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a cube depthless
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        public void DrawCubeDepthless(Vector3[] position, Vector3[] rotation, Vector3[] scale)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawCubeDepthless(new Material(), position, rotation, scale);
                }
            }
        }

        /// <summary>
        /// Draw a cube depthless
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        public void DrawCubeDepthless(Material material, Vector3[] position, Vector3[] rotation, Vector3[] scale)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawCubeDepthless(material, position, rotation, scale);
                }
            }
        }
        #endregion

        #region Draw Sphere Depthless
        /// <summary>
        /// Draw a sphere depthless
        /// </summary>
        public void DrawSphereDepthless()
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawCubeDepthless(new Material(), new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a sphere depthless
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        public void DrawSphereDepthless(Material material)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawCubeDepthless(material, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a sphere depthless
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        public void DrawSphereDepthless(Vector3 position)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawCubeDepthless(new Material(), new Vector3[] { position }, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a sphere depthless
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        public void DrawSphereDepthless(Material material, Vector3 position)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawCubeDepthless(material, new Vector3[] { position }, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a sphere depthless
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        public void DrawSphereDepthless(Vector3 position, Vector3 rotation)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawCubeDepthless(new Material(), new Vector3[] { position }, new Vector3[] { rotation }, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a sphere depthless
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        public void DrawSphereDepthless(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawCubeDepthless(new Material(), new Vector3[] { position }, new Vector3[] { rotation }, new Vector3[] { scale });
                }
            }
        }

        /// <summary>
        /// Draw a sphere depthless
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        public void DrawSphereDepthless(Material material, Vector3 position, Vector3 rotation)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawCubeDepthless(material, new Vector3[] { position }, new Vector3[] { rotation }, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a sphere depthless
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        public void DrawSphereDepthless(Material material, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawCubeDepthless(material, new Vector3[] { position }, new Vector3[] { rotation }, new Vector3[] { scale });
                }
            }
        }

        /// <summary>
        /// Draw a sphere depthless
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        public void DrawSphereDepthless(Vector3[] position)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawCubeDepthless(new Material(), position, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a sphere depthless
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        public void DrawSphereDepthless(Material material, Vector3[] position)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawCubeDepthless(material, position, new Vector3[] { new Vector3(0) }, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a sphere depthless
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        public void DrawSphereDepthless(Vector3[] position, Vector3[] rotation)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawCubeDepthless(new Material(), position, rotation, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a sphere depthless
        /// </summary>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        public void DrawSphereDepthless(Vector3[] position, Vector3[] rotation, Vector3[] scale)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawCubeDepthless(new Material(), position, rotation, scale);
                }
            }
        }

        /// <summary>
        /// Draw a sphere depthless
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        public void DrawSphereDepthless(Material material, Vector3[] position, Vector3[] rotation)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawCubeDepthless(material, position, rotation, new Vector3[] { new Vector3(1) });
                }
            }
        }

        /// <summary>
        /// Draw a sphere depthless
        /// </summary>
        /// <param name="material"> Material to draw mesh with </param>
        /// <param name="position"> Position of the mesh to be drawn </param>
        /// <param name="rotation"> Rotation of the mesh to be drawn </param>
        /// <param name="scale"> Scale of the mesh to be drawn </param>
        public void DrawSphereDepthless(Material material, Vector3[] position, Vector3[] rotation, Vector3[] scale)
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.DrawCubeDepthless(material, position, rotation, scale);
                }
            }
        }
        #endregion

        public bool DrawIconDepthless()
        {
            return false;
        }

        public bool EndFrame()
        {
            if (frameStarted)
            {
                if (renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.EndFrame();
                    frameStarted = false;
                    return true;
                }
            }

            return false;
        }

        public bool RemoveFrame()
        {
            if (frameStarted)
            {
                frameStarted = false;
            }

            else
            {
                return false;
            }

            return true;
        }

        public void StopRenderer()
        {
            if(RendererRunning)
            {
                if(renderingAPI == RenderingAPI.DirectX11)
                {
                    DX11Renderer.StopRenderer();
                    RendererRunning = false;
                }
            }
        }

        public static Vector3 GetPosition(Vector3[] Positions, Vector3[] Rotations, Vector3[] Scales)
        {
            Matrix TransformMatrix = new Matrix();

            if (Positions.Length == Rotations.Length &&
                Positions.Length == Scales.Length)
            {
                for (int i = 0; i < Positions.Length; i++)
                {
                    TransformMatrix *= Matrix.RotationZ(-Rotations[i].Z / 180 * (float)Math.PI) * 
                                       Matrix.RotationX(-Rotations[i].X / 180 * (float)Math.PI) * 
                                       Matrix.RotationY(-Rotations[i].Y / 180 * (float)Math.PI) * 
                                       Matrix.Scaling(Scales[i]) * 
                                       Matrix.Translation(Positions[i]);
                }
            }


            return TransformMatrix.TranslationVector;
        }

        public static Vector3 GetRotation(Vector3[] Rotations)
        {
            Matrix TransformMatrix = new Matrix();

            for (int i = 0; i < Rotations.Length; i++)
            {
                TransformMatrix *= Matrix.RotationZ(-Rotations[i].Z / 180 * (float)Math.PI) *
                                   Matrix.RotationX(-Rotations[i].X / 180 * (float)Math.PI) *
                                   Matrix.RotationY(-Rotations[i].Y / 180 * (float)Math.PI);
            }

            TransformMatrix.Decompose(out Vector3 Scale, out SharpDX.Quaternion rotation, out Vector3 Position);

            return QuternionToEulerAngles(rotation);
        }

        internal static Vector3 QuternionToEulerAngles(SharpDX.Quaternion Rotation)
        {
            float X = Atan2(2 * (Rotation.W * Rotation.X + Rotation.Y * Rotation.Z), 1 - 2 * (Rotation.X * Rotation.X + Rotation.Y * Rotation.Y));
            float Y = (float)(Math.Asin(2 * (Rotation.W * Rotation.Y - Rotation.Z * Rotation.X)) * 180 / Math.PI);
            float Z = Atan2(2 * (Rotation.W * Rotation.Z + Rotation.Y * Rotation.X), 1 - 2 * (Rotation.Z * Rotation.Z + Rotation.Y * Rotation.Y));

            return new Vector3(X, Y, Z);
        }

        internal static float Atan2(float Y, float X)
        {
            if (X > 0)
            {
                return (float)(Math.Tanh(Y / X) * 180 / Math.PI);
            }

            if (X == 0)
            {
                if (Y > 0)
                {
                    return 90;
                }

                if (Y < 0)
                {
                    return -90;
                }

                return float.PositiveInfinity;
            }

            if (Y >= 0)
            {
                return (float)(Math.Tanh(Y / X) * 180 / Math.PI) + 180;
            }

            if (Y < 0)
            {
                return (float)(Math.Tanh(Y / X) * 180 / Math.PI) - 180;
            }

            return float.NaN;
        }
    }
}