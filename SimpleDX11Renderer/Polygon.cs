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
    /// Triangle made from 3 points in 3d space
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct Polygon
    {
        /// <summary>
        /// ID for the 3 vertecis that the polygon consists of
        /// </summary>
        public int[] VerticesIDs;

        /// <summary>
        /// Creates a polygon
        /// </summary>
        /// <param name="vertex1ID"> ID for 3D Point 1 </param>
        /// <param name="vertex2ID"> ID for 3D Point 2 </param>
        /// <param name="vertex3ID"> ID for 3D Point 3 </param>
        public Polygon(int vertex1ID, int vertex2ID, int vertex3ID)
        {
            VerticesIDs = new int[] { vertex1ID, vertex2ID, vertex3ID };
        }

        /// <summary>
        /// Creates a polygon
        /// </summary>
        /// <param name="verticesIDs"> IDs for 3D points, Must be 3 points</param>
        public Polygon(int[] verticesIDs)
        {
            if (verticesIDs.Length == 3)
            {
                VerticesIDs = verticesIDs;
            }

            else
            {
                VerticesIDs = new int[] { -1, -1, -1 };
            }
        }
    }
}