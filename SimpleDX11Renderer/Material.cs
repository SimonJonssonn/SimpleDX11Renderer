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
using System.IO;

namespace SimpleDX11Renderer
{
    /// <summary>
    /// Material for rendering
    /// </summary>
    public class Material
    {
        /// <summary>
        /// Name of the material
        /// </summary>
        public string Name = "Material";

        /// <summary>
        /// Is the material affected by lightsources
        /// </summary>
        public bool AffectedByLight;

        #region Color
        /// <summary>
        /// Primary color channel of the material
        /// </summary>
        public int ColorChanel1;

        /// <summary>
        /// Secondary color channel of the material
        /// </summary>
        public int ColorChanel2;

        /// <summary>
        /// Color of the material if no texture exists
        /// </summary>
        public Vector4 NoTexColor;
        #endregion

        #region Normal
        /// <summary>
        /// Primary normal channel of the material
        /// </summary>
        public Normal NormalChannel1;

        /// <summary>
        /// Secondary normal channel of the material
        /// </summary>
        public Normal NormalChannel2;
        #endregion

        #region Specular
        /// <summary>
        /// Primary specular channel of the material
        /// </summary>
        public Specular SpecularChannel1;

        /// <summary>
        /// Secondary specular channel of the material
        /// </summary>
        public Specular SpecularChannel2;
        #endregion

        #region Reflection
        /// <summary>
        /// Primary reflection channel of the material
        /// </summary>
        public Reflection ReflectionChannel1;

        /// <summary>
        /// Secondary reflection channel of the material
        /// </summary>
        public Reflection ReflectionChannel2;
        #endregion

        #region Transparancy
        /// <summary>
        /// Primary transparancy channel of the material
        /// </summary>
        public Transparancy TransparancyChannel1;
        #endregion

        /// <summary>
        /// Blend Strenght betweenn channel 1 and channel 2
        /// </summary>
        public float BlendStrenght;

        /// <summary>
        /// Is the material in singel material mode
        /// </summary>
        public bool SingelMaterial;

        /// <summary>
        /// Create a new material
        /// </summary>
        public Material()
        {
            BlendStrenght = 1;
            SingelMaterial = true;
            AffectedByLight = true;

            ColorChanel1 = -1;
            ColorChanel2 = -1;
            NoTexColor = new Color4(255, 255, 255, 1);

            NormalChannel1 = new Normal(-1, 0);
            NormalChannel2 = new Normal(-1, 0);

            SpecularChannel1 = new Specular(-1);
            SpecularChannel2 = new Specular(-1);

            ReflectionChannel1 = new Reflection(new Color4(255, 255, 255, 1), 0);
            ReflectionChannel2 = new Reflection(new Color4(255, 255, 255, 1), 0);

            TransparancyChannel1 = new Transparancy(-1);
        }

        /// <summary>
        /// Create a new material
        /// </summary>
        /// <param name="affectedByLight"> Is the material affected by light sources</param>
        public Material(bool affectedByLight)
        {
            BlendStrenght = 1;
            SingelMaterial = true;
            AffectedByLight = affectedByLight;

            ColorChanel1 = -1;
            ColorChanel2 = -1;
            NoTexColor = new Color4(255, 255, 255, 1);

            NormalChannel1 = new Normal(-1, 0);
            NormalChannel2 = new Normal(-1, 0);

            SpecularChannel1 = new Specular(-1);
            SpecularChannel2 = new Specular(-1);

            ReflectionChannel1 = new Reflection(new Color4(255, 255, 255, 1), 0);
            ReflectionChannel2 = new Reflection(new Color4(255, 255, 255, 1), 0);

            TransparancyChannel1 = new Transparancy(-1);
        }

        /// <summary>
        /// Create a new material
        /// </summary>
        /// <param name="TextureID"> ID for texture 1 </param>
        public Material(int TextureID)
        {
            BlendStrenght = 1;
            SingelMaterial = true;
            AffectedByLight = true;

            ColorChanel1 = TextureID;
            ColorChanel2 = -1;
            NoTexColor = new Color4(255, 255, 255, 1);

            NormalChannel1 = new Normal(-1, 0);
            NormalChannel2 = new Normal(-1, 0);

            SpecularChannel1 = new Specular(-1);
            SpecularChannel2 = new Specular(-1);

            ReflectionChannel1 = new Reflection(new Color4(255, 255, 255, 1), 0);
            ReflectionChannel2 = new Reflection(new Color4(255, 255, 255, 1), 0);

            TransparancyChannel1 = new Transparancy(-1);
        }

        /// <summary>
        /// Create a new material
        /// </summary>
        /// <param name="TextureID"> ID for texture 1 </param>
        /// <param name="affectedByLight"> Is the material affected by light sources</param>
        public Material(int TextureID, bool affectedByLight)
        {
            BlendStrenght = 1;
            SingelMaterial = true;
            AffectedByLight = affectedByLight;

            ColorChanel1 = TextureID;
            ColorChanel2 = -1;
            NoTexColor = new Color4(255, 255, 255, 1);

            NormalChannel1 = new Normal(-1, 0);
            NormalChannel2 = new Normal(-1, 0);

            SpecularChannel1 = new Specular(-1);
            SpecularChannel2 = new Specular(-1);

            ReflectionChannel1 = new Reflection(new Color4(255, 255, 255, 1), 0);
            ReflectionChannel2 = new Reflection(new Color4(255, 255, 255, 1), 0);

            TransparancyChannel1 = new Transparancy(-1);
        }

        /// <summary>
        /// Create a new material
        /// </summary>
        /// <param name="TextureID1"> ID for texture 1 </param>
        /// <param name="TextureID2"> ID for texture 2 </param>
        public Material(int TextureID1, int TextureID2)
        {
            this.BlendStrenght = 0.5f;
            SingelMaterial = false;
            AffectedByLight = true;

            ColorChanel1 = TextureID1;
            ColorChanel2 = TextureID2;
            NoTexColor = new Color4(255, 255, 255, 1);

            NormalChannel1 = new Normal(-1, 0);
            NormalChannel2 = new Normal(-1, 0);

            SpecularChannel1 = new Specular(-1);
            SpecularChannel2 = new Specular(-1);

            ReflectionChannel1 = new Reflection(new Color4(255, 255, 255, 1), 0);
            ReflectionChannel2 = new Reflection(new Color4(255, 255, 255, 1), 0);

            TransparancyChannel1 = new Transparancy(-1);
        }

        /// <summary>
        /// Create a new material
        /// </summary>
        /// <param name="TextureID1"> ID for texture 1 </param>
        /// <param name="TextureID2"> ID for texture 2 </param>
        /// <param name="affectedByLight"> Is the material affected by light sources</param>
        public Material(int TextureID1, int TextureID2, bool affectedByLight)
        {
            this.BlendStrenght = 0.5f;
            SingelMaterial = false;
            AffectedByLight = affectedByLight;

            ColorChanel1 = TextureID1;
            ColorChanel2 = TextureID2;
            NoTexColor = new Color4(255, 255, 255, 1);

            NormalChannel1 = new Normal(-1, 0);
            NormalChannel2 = new Normal(-1, 0);

            SpecularChannel1 = new Specular(-1);
            SpecularChannel2 = new Specular(-1);

            ReflectionChannel1 = new Reflection(new Color4(255, 255, 255, 1), 0);
            ReflectionChannel2 = new Reflection(new Color4(255, 255, 255, 1), 0);

            TransparancyChannel1 = new Transparancy(-1);
        }

        /// <summary>
        /// Create a new material
        /// </summary>
        /// <param name="TextureID1"> ID for texture 1 </param>
        /// <param name="TextureID2"> ID for texture 2 </param>
        /// <param name="BlendStrenght"> Blend strenght between chanels </param>
        public Material(int TextureID1, int TextureID2, int BlendStrenght)
        {
            this.BlendStrenght = BlendStrenght;
            SingelMaterial = false;
            AffectedByLight = true;

            ColorChanel1 = TextureID1;
            ColorChanel2 = TextureID2;
            NoTexColor = new Color4(255, 255, 255, 1);

            NormalChannel1 = new Normal(-1, 0);
            NormalChannel2 = new Normal(-1, 0);

            SpecularChannel1 = new Specular(-1);
            SpecularChannel2 = new Specular(-1);

            ReflectionChannel1 = new Reflection(new Color4(255, 255, 255, 1), 0);
            ReflectionChannel2 = new Reflection(new Color4(255, 255, 255, 1), 0);

            TransparancyChannel1 = new Transparancy(-1);
        }

        /// <summary>
        /// Create a new material
        /// </summary>
        /// <param name="TextureID1"> ID for texture 1 </param>
        /// <param name="TextureID2"> ID for texture 2 </param>
        /// <param name="BlendStrenght"> Blend strenght between chanels </param>
        /// <param name="affectedByLight"> Is the material affected by light sources</param>
        public Material(int TextureID1, int TextureID2, int BlendStrenght, bool affectedByLight)
        {
            this.BlendStrenght = BlendStrenght;
            SingelMaterial = false;
            AffectedByLight = true;

            ColorChanel1 = TextureID1;
            ColorChanel2 = TextureID2;
            NoTexColor = new Color4(255, 255, 255, 1);

            NormalChannel1 = new Normal(-1, 0);
            NormalChannel2 = new Normal(-1, 0);

            SpecularChannel1 = new Specular(-1);
            SpecularChannel2 = new Specular(-1);

            ReflectionChannel1 = new Reflection(new Color4(255, 255, 255, 1), 0);
            ReflectionChannel2 = new Reflection(new Color4(255, 255, 255, 1), 0);

            TransparancyChannel1 = new Transparancy(-1);
        }

        /// <summary>
        /// Create a new material
        /// </summary>
        /// <param name="Color"> Color of the material </param>
        public Material(Vector4 Color)
        {
            BlendStrenght = 1;
            SingelMaterial = true;
            AffectedByLight = true;

            ColorChanel1 = -1;
            ColorChanel2 = -1;
            NoTexColor = Color;

            NormalChannel1 = new Normal(-1, 0);
            NormalChannel2 = new Normal(-1, 0);

            SpecularChannel1 = new Specular(-1);
            SpecularChannel2 = new Specular(-1);

            ReflectionChannel1 = new Reflection(new Color4(255, 255, 255, 1), 0);
            ReflectionChannel2 = new Reflection(new Color4(255, 255, 255, 1), 0);

            TransparancyChannel1 = new Transparancy(-1);
        }

        /// <summary>
        /// Create a new material
        /// </summary>
        /// <param name="Color"> Color of the material </param>
        /// <param name="affectedByLight"> Is the material affected by light sources</param>
        public Material(Vector4 Color, bool affectedByLight)
        {
            BlendStrenght = 1;
            SingelMaterial = true;
            AffectedByLight = affectedByLight;

            ColorChanel1 = -1;
            ColorChanel2 = -1;
            NoTexColor = Color;

            NormalChannel1 = new Normal(-1, 0);
            NormalChannel2 = new Normal(-1, 0);

            SpecularChannel1 = new Specular(-1);
            SpecularChannel2 = new Specular(-1);

            ReflectionChannel1 = new Reflection(new Color4(255, 255, 255, 1), 0);
            ReflectionChannel2 = new Reflection(new Color4(255, 255, 255, 1), 0);

            TransparancyChannel1 = new Transparancy(-1);
        }

        /// <summary>
        /// Make a deep copy of a material
        /// </summary>
        /// <param name="material"> Material to copy </param>
        public Material(Material material)
        {
            BlendStrenght = material.BlendStrenght;
            SingelMaterial = material.SingelMaterial;
            AffectedByLight = material.AffectedByLight;

            ColorChanel1 = material.ColorChanel1;
            ColorChanel2 = material.ColorChanel2;
            NoTexColor = material.NoTexColor;

            NormalChannel1 = material.NormalChannel1;
            NormalChannel2 = material.NormalChannel2;

            SpecularChannel1 = material.SpecularChannel1;
            SpecularChannel2 = material.SpecularChannel2;

            ReflectionChannel1 = material.ReflectionChannel1;
            ReflectionChannel2 = material.ReflectionChannel2;

            TransparancyChannel1 = material.TransparancyChannel1;
        }
    }
}