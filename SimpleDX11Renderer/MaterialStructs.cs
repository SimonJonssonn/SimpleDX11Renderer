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

namespace SimpleDX11Renderer
{
    /// <summary>
    /// Specular Material Channel
    /// </summary>
    public struct Specular
    {
        /// <summary>
        /// Specular map, if == null each pixel is treated as 1 x specular strength
        /// </summary>
        public int SpecularMap;

        /// <summary>
        /// Strength of specularity
        /// </summary>
        public float SpecularStrength;

        /// <summary>
        /// Creates a new specular channel
        /// </summary>
        /// <param name="SpecularStrength"> Strength of specularity </param>
        public Specular(float SpecularStrength)
        {
            SpecularMap = -1;
            this.SpecularStrength = SpecularStrength;
        }

        /// <summary>
        /// Creates a new specular channel
        /// </summary>
        /// <param name="SpecularMap"> Specular Map </param>
        public Specular(int SpecularMap)
        {
            this.SpecularMap = SpecularMap;
            SpecularStrength = 1;
        }

        /// <summary>
        /// Creates a new specular channel
        /// </summary>
        /// <param name="SpecularMap"> Specular Map </param>
        /// <param name="SpecularStrength"> Strength of specularity </param>
        public Specular(int SpecularMap, float SpecularStrength)
        {
            this.SpecularMap = SpecularMap;
            this.SpecularStrength = SpecularStrength;
        }
    }

    /// <summary>
    /// Normal Material Channel
    /// </summary>
    public struct Normal
    {
        /// <summary>
        /// Normal map
        /// </summary>
        public int NormalMap;

        /// <summary>
        /// Strength of normal map
        /// </summary>
        public float NormalStrength;

        /// <summary>
        /// Creates a new normal channel
        /// </summary>
        /// <param name="NormalMap"> Normal map </param>
        public Normal(int NormalMap)
        {
            this.NormalMap = NormalMap;
            NormalStrength = 1;
        }

        /// <summary>
        /// Creates a new normal channel
        /// </summary>
        /// <param name="NormalMap"> Normal map </param>
        /// <param name="NormalStrength"> Strength of normal map </param>
        public Normal(int NormalMap, float NormalStrength)
        {
            this.NormalMap = NormalMap;
            this.NormalStrength = NormalStrength;
        }
    }

    /// <summary>
    /// Material Texture Blend Channel
    /// </summary>
    public struct TextureBlend
    {
        /// <summary>
        /// Alpha blend map
        /// </summary>
        public int AlphaBlendMap;

        /// <summary>
        /// Amount to overlay
        /// </summary>
        public float OverlayAmount;

        /// <summary>
        /// Types of blend operations
        /// </summary>
        public enum blendType
        {
            SigelMaterialMode,
            AlphaBlend,
            OverlayBlend
        }

        /// <summary>
        /// Chosen blend type
        /// </summary>
        public blendType BlendType;

        /// <summary>
        /// Creates a new texture blend channel
        /// </summary>
        /// <param name="AlphaBlendMap"> Alpha blend map </param>
        public TextureBlend(bool Enabled, int AlphaBlendMap)
        {
            if (Enabled)
            {
                this.AlphaBlendMap = AlphaBlendMap;
                OverlayAmount = 1;
                BlendType = blendType.AlphaBlend;
            }

            else
            {
                this.AlphaBlendMap = -1;
                OverlayAmount = 1;
                BlendType = blendType.SigelMaterialMode;
            }
        }

        /// <summary>
        /// Creates a new texture blend channel
        /// </summary>
        /// <param name="OverlayAmount"> Amount to overlay </param>
        public TextureBlend(bool Enabled, float OverlayAmount)
        {
            if (Enabled)
            {
                AlphaBlendMap = -1;
                this.OverlayAmount = OverlayAmount;
                BlendType = blendType.OverlayBlend;
            }

            else
            {
                AlphaBlendMap = -1;
                this.OverlayAmount = 1;
                BlendType = blendType.SigelMaterialMode;
            }
        }
    }

    /// <summary>
    /// Material Transparancy Channel
    /// </summary>
    public struct Transparancy
    {
        /// <summary>
        /// Transparancy map
        /// </summary>
        public int TransparancyMap;

        /// <summary>
        /// Strength of transparancy
        /// </summary>
        public float TransparancyStrength;

        /// <summary>
        /// Creates a new transparancy channel
        /// </summary>
        /// <param name="TransparancyStrength"> Strength of transparancy </param>
        public Transparancy(float TransparancyStrength)
        {
            TransparancyMap = -1;
            this.TransparancyStrength = TransparancyStrength;
        }

        /// <summary>
        /// Creates a new transparancy channel
        /// </summary>
        /// <param name="TransparancyMap"> Transparancy map </param>
        public Transparancy(int TransparancyMap)
        {
            this.TransparancyMap = TransparancyMap;
            TransparancyStrength = 1;
        }

        /// <summary>
        /// Creates a new transparancy channel
        /// </summary>
        /// <param name="TransparancyMap"> Transparancy map </param>
        /// <param name="TransparancyStrength"> Strength of transparancy </param>
        public Transparancy(int TransparancyMap, float TransparancyStrength)
        {
            this.TransparancyMap = TransparancyMap;
            this.TransparancyStrength = TransparancyStrength;
        }
    }

    /// <summary>
    /// Reflection Channel
    /// </summary>
    public struct Reflection
    {
        /// <summary>
        /// Reflection map
        /// </summary>
        public int ReflectionColorMap;

        /// <summary>
        /// Reflection Color
        /// </summary>
        public Color4 ReflectionColor;

        /// <summary>
        /// Strenght of reflection
        /// </summary>
        public float ReflectionStrength;

        /// <summary>
        /// Create a new reflection channel
        /// </summary>
        /// <param name="ReflectionColorMap"> Reflection map </param>
        public Reflection(int ReflectionColorMap)
        {
            this.ReflectionColorMap = ReflectionColorMap;
            ReflectionColor = new Color4(0, 0, 0, 0);
            ReflectionStrength = 1;
        }

        /// <summary>
        /// Create a new reflection channel
        /// </summary>
        /// <param name="ReflectionColor"> Reflection Color </param>
        public Reflection(Color4 ReflectionColor)
        {
            ReflectionColorMap = -1;
            this.ReflectionColor = ReflectionColor;
            ReflectionStrength = 1;
        }

        /// <summary>
        /// Create a new reflection channel
        /// </summary>
        /// <param name="ReflectionColorMap"> Reflection map </param>
        /// <param name="ReflectionStrength"> Strenght of reflection </param>
        public Reflection(int ReflectionColorMap, float ReflectionStrength)
        {
            this.ReflectionColorMap = ReflectionColorMap;
            ReflectionColor = new Color4(0, 0, 0, 0);
            this.ReflectionStrength = ReflectionStrength;
        }

        /// <summary>
        /// Create a new reflection channel
        /// </summary>
        /// <param name="ReflectionColor"> Reflection Color </param>
        /// <param name="ReflectionStrength"> Strenght of reflection </param>
        public Reflection(Color4 ReflectionColor, float ReflectionStrength)
        {
            ReflectionColorMap = -1;
            this.ReflectionColor = ReflectionColor;
            this.ReflectionStrength = ReflectionStrength;
        }
    }
}