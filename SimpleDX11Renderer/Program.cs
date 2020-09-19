using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
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
using System.Windows.Input;
using System.Reflection;

namespace SimpleDX11Renderer
{
    class Program
    {
        public static Renderer renderer = new Renderer();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Mesh mesh = OBJimporter.ImportOBJ(@"C:\Users\simon\source\repos\GameEngineV7\GameEngineV7\bin\Debug\EngineModels\Rotate.obj")[0];

            Form form = new Form();
            form.Width = 1280;
            form.Height = 720;
            form.SizeChanged += Form_SizeChanged;
        }

        private static void Form_SizeChanged(object sender, EventArgs e)
        {
            Form form = (Form)sender;
            renderer.SetCameraProperties(new CameraProperties(90, form.Width, form.Height, renderer.GetCameraPosition(), renderer.GetCameraRotation()));
            renderer.StartRenderer(form.Handle);
        }
    }
}