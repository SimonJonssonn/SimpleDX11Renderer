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
    /// Types of light sources
    /// </summary>
    public enum LightSourceType
    {
        Directonal = 0,
        Point = 1
    }

    /// <summary>
    /// A light source
    /// </summary>
    public class Light
    {
        /// <summary>
        /// Type of light source
        /// </summary>
        public LightSourceType LightSourceType;

        /// <summary>
        /// Intensity of light source in LUX @ 1 m^2
        /// </summary>
        public float LightIntensity;

        /// <summary>
        /// Color of light source
        /// </summary>
        public Vector4 LightColor;

        /// <summary>
        /// Position of light source
        /// </summary>
        public Vector3 Position;

        /// <summary>
        /// Rotation of light source
        /// </summary>
        public Vector3 Rotation;

        /// <summary>
        /// Create a new light source
        /// </summary>
        public Light()
        {
            LightSourceType = LightSourceType.Directonal;
            LightIntensity = 10000;
            LightColor = new Vector4(255, 255, 255, 1);
            Position = new Vector3(0);
            Rotation = new Vector3(45, 45, 0);
        }

        /// <summary>
        /// Create a new light source
        /// </summary>
        /// <param name="lightSourceType"> The type of light source </param>
        public Light(LightSourceType lightSourceType)
        {
            LightSourceType = lightSourceType;

            if(lightSourceType == LightSourceType.Directonal)
            {
                LightIntensity = 17500;
                LightColor = new Vector4(255, 255, 255, 1);
                Position = new Vector3(0);
                Rotation = new Vector3(45, 45, 0);
            }

            else if(lightSourceType == LightSourceType.Point)
            {
                LightIntensity = 750;
                LightColor = new Vector4(255, 255, 255, 1);
                Position = new Vector3(0);
                Rotation = new Vector3(0);
            }
        }

        /// <summary>
        /// Create a new light source
        /// </summary>
        /// <param name="lightSourceType"> The type of light source </param>
        /// <param name="lightColor"> The color of the light source </param>
        public Light(LightSourceType lightSourceType, Vector4 lightColor)
        {
            LightSourceType = lightSourceType;
            LightColor = lightColor;

            if (lightSourceType == LightSourceType.Directonal)
            {
                LightIntensity = 17500;
                Position = new Vector3(0);
                Rotation = new Vector3(45, 45, 0);
            }

            else if (lightSourceType == LightSourceType.Point)
            {
                LightIntensity = 1000;
                Position = new Vector3(0);
                Rotation = new Vector3(0);
            }
        }

        /// <summary>
        /// Create a new light source
        /// </summary>
        /// <param name="lightSourceType"> The type of light source </param>
        /// <param name="lightIntensity"> The intensity of the light source in LUX @ 1m^2 </param>
        public Light(LightSourceType lightSourceType, float lightIntensity)
        {
            LightSourceType = lightSourceType;
            LightIntensity = lightIntensity;

            if (lightSourceType == LightSourceType.Directonal)
            {   
                LightColor = new Vector4(255, 255, 255, 1);
                Position = new Vector3(0);
                Rotation = new Vector3(45, 45, 0);
            }

            else if (lightSourceType == LightSourceType.Point)
            {
                LightColor = new Vector4(255, 255, 255, 1);
                Position = new Vector3(0);
                Rotation = new Vector3(0);
            }
        }

        /// <summary>
        /// Create a new light source
        /// </summary>
        /// <param name="lightSourceType"> The type of light source </param>
        /// <param name="position"> Position of the light source </param>
        /// <param name="rotation"> Rotation of the light source </param>
        public Light(LightSourceType lightSourceType, Vector3 position, Vector3 rotation)
        {
            LightSourceType = lightSourceType;
            LightColor = new Vector4(255, 255, 255, 0);

            Position = position;
            Rotation = rotation;

            if (lightSourceType == LightSourceType.Directonal)
            {
                LightIntensity = 17500;
            }

            else if (lightSourceType == LightSourceType.Point)
            {
                LightIntensity = 1000;
            }
        }

        /// <summary>
        /// Create a new light source
        /// </summary>
        /// <param name="lightSourceType"> The type of light source </param>
        /// <param name="lightColor"> The color of the light source </param>
        /// <param name="lightIntensity"> The intensity of the light source in LUX @ 1m^2 </param>
        public Light(LightSourceType lightSourceType, Vector4 lightColor, float lightIntensity)
        {
            LightSourceType = lightSourceType;
            LightColor = lightColor;
            LightIntensity = lightIntensity;

            if (lightSourceType == LightSourceType.Directonal)
            {
                Position = new Vector3(0);
                Rotation = new Vector3(45, 45, 0);
            }

            else if (lightSourceType == LightSourceType.Point)
            {
                Position = new Vector3(0);
                Rotation = new Vector3(0);
            }
        }

        /// <summary>
        /// Create a new light source
        /// </summary>
        /// <param name="lightSourceType"> The type of light source </param>
        /// <param name="lightColor"> The color of the light source </param>
        /// <param name="position"> Position of the light source </param>
        /// <param name="rotation"> Rotation of the light source </param>
        public Light(LightSourceType lightSourceType, Vector4 lightColor, Vector3 position, Vector3 rotation)
        {
            LightSourceType = lightSourceType;
            LightColor = lightColor;

            Position = position;
            Rotation = rotation;

            if (lightSourceType == LightSourceType.Directonal)
            {
                LightIntensity = 17500;
            }

            else if (lightSourceType == LightSourceType.Point)
            {
                LightIntensity = 1000;
            }
        }

        /// <summary>
        /// Create a new light source
        /// </summary>
        /// <param name="lightSourceType"> The type of light source </param>
        /// <param name="lightIntensity"> The intensity of the light source in LUX @ 1m^2 </param>
        /// <param name="position"> Position of the light source </param>
        /// <param name="rotation"> Rotation of the light source </param>
        public Light(LightSourceType lightSourceType, float lightIntensity, Vector3 position, Vector3 rotation)
        {
            LightSourceType = lightSourceType;
            LightIntensity = lightIntensity;

            Position = position;
            Rotation = rotation;

            if (lightSourceType == LightSourceType.Directonal)
            {
                LightColor = new Vector4(255, 255, 255, 1);
            }

            else if (lightSourceType == LightSourceType.Point)
            {
                LightColor = new Vector4(255, 255, 255, 1);
            }
        }

        /// <summary>
        /// Create a new light source
        /// </summary>
        /// <param name="lightSourceType"> The type of light source </param>
        /// <param name="lightColor"> The color of the light source </param>
        /// <param name="lightIntensity"> The intensity of the light source in LUX @ 1m^2 </param>
        /// <param name="position"> Position of the light source </param>
        /// <param name="rotation"> Rotation of the light source </param>
        public Light(LightSourceType lightSourceType, Vector4 lightColor, float lightIntensity, Vector3 position, Vector3 rotation)
        {
            LightSourceType = lightSourceType;
            LightIntensity = lightIntensity;
            LightColor = lightColor;

            Position = position;
            Rotation = rotation;
        }

        /// <summary>
        /// Make a Deep copy of a light source
        /// </summary>
        /// <param name="light"> Light source to copy </param>
        public Light(Light light)
        {
            LightSourceType = light.LightSourceType;
            LightIntensity = light.LightIntensity;
            LightColor = new Vector4(light.LightColor.X, light.LightColor.Y, light.LightColor.Z, light.LightColor.W);
            Position = new Vector3(light.Position.X, light.Position.Y, light.Position.Z);
            Rotation = new Vector3(light.Rotation.X, light.Rotation.Y, light.Rotation.Z);
        }
    }
}