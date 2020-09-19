using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
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

namespace SimpleDX11Renderer
{
    /// <summary>
    /// 3D point used for bulding meshes
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct Vertex
    {
        /// <summary>
        /// Vertex position in 3d space
        /// </summary>
        public Vector3 Position;

        /// <summary>
        /// Tecture Cordinates of this vertex
        /// </summary>
        public Vector2 UV;

        /// <summary>
        /// Normal direction of this vertex
        /// </summary>
        public Vector3 Normal;

        /// <summary>
        /// Vertex BiNormal
        /// </summary>
        public Vector3 BiNormal;

        /// <summary>
        /// Vertex Tangent
        /// </summary>
        public Vector3 Tangent;

        /// <summary>
        /// Create a geometry point in 3D space
        /// </summary>
        /// <param name="position"> Position of the point </param>
        public Vertex(Vector3 position)
        {
            Position = position;
            UV = new Vector2(0, 0);
            Normal = new Vector3(0);
            BiNormal = new Vector3(0);
            Tangent = new Vector3(0);
        }

        /// <summary>
        /// Create a geometry point in 3D space
        /// </summary>
        /// <param name="position"> Position of the point </param>
        /// <param name="uv"> texture cordinate of the point </param>
        public Vertex(Vector3 position, Vector2 uv)
        {
            Position = position;
            UV = uv;
            Normal = new Vector3(0);
            BiNormal = new Vector3(0);
            Tangent = new Vector3(0);
        }

        /// <summary>
        /// Create a geometry point in 3D space
        /// </summary>
        /// <param name="position"> Position of the point </param>
        /// <param name="uv"> texture cordinate of the point </param>
        /// <param name="normal"> Normal direction of the point </param>
        public Vertex(Vector3 position, Vector2 uv, Vector3 normal)
        {
            Position = position;
            UV = uv;
            Normal = normal;
            BiNormal = new Vector3(0);
            Tangent = new Vector3(0);
        }
    }
}