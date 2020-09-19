using System;
using System.Collections.Generic;
using System.Text;
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
using System.IO;
using SharpDX.WIC;

namespace SimpleDX11Renderer
{
    internal static class Convert
    {
        /// <summary>
        /// Convert a string to a Vector3, used in OBJ importing
        /// </summary>
        /// <param name="data"> String to convert </param>
        internal static Vector3 OBJStringToVector3(string data)
        {
            string[] Parts;
            Vector3 vector3 = new Vector3(-1);

            if (!string.IsNullOrWhiteSpace(data))
            {
                if (data.Contains("/"))
                {
                    Parts = data.Replace(".", ",").Split("/".ToCharArray());

                    if (Parts.Length > 0 && Parts.Length <= 4)
                    {
                        if (float.TryParse(Parts[0], out float X))
                        {
                            if(Parts.Length > 1)
                            {
                                vector3.X = X;

                                if (float.TryParse(Parts[1], out float Y))
                                {
                                    vector3.Y = Y;

                                    if (Parts.Length > 2)
                                    {
                                        if (float.TryParse(Parts[2], out float Z))
                                        {
                                            vector3.Z = Z;
                                        }

                                        else
                                        {
                                            Debug.Error("Can not Parse Parts[2]: \"" + Parts[2] + "\"");
                                        }
                                    }
                                }

                                else
                                {
                                    Debug.Error("Can not Parse Parts[1]: \"" + Parts[1] + "\"");
                                }
                            }
                        }

                        else
                        {
                            Debug.Error("Can not Parse Parts[0]: \"" + Parts[0] + "\"");
                        }
                    }

                    else
                    {
                        Debug.Error("Data parts = " + Parts.Length + ", it is supposed to be = 3");
                    }
                }

                else if (data.Contains(" "))
                {
                    Parts = data.Replace(".", ",").Split(" ".ToCharArray());

                    if (Parts.Length == 3 || Parts.Length == 4)
                    {
                        if (float.TryParse(Parts[0], out float X))
                        {
                            if (float.TryParse(Parts[1], out float Y))
                            {
                                if (float.TryParse(Parts[2], out float Z))
                                {
                                    vector3 = new Vector3(X, Y, Z);
                                }

                                else
                                {
                                    Debug.Error("Can not Parse Parts[2]: \"" + Parts[2] + "\"");
                                }
                            }

                            else
                            {
                                Debug.Error("Can not Parse Parts[1]: \"" + Parts[1] + "\"");
                            }
                        }

                        else
                        {
                            Debug.Error("Can not Parse Parts[0]: \"" + Parts[0] + "\"");
                        }
                    }

                    else
                    {
                        Debug.Error("Data parts = " + Parts.Length + ", it is supposed to be = 3");
                    }
                }

                else
                {
                    Debug.Error("Data does not contain any known split character");
                }
            }

            else
            {
                Debug.Error("data is null or empty");
            }

            return vector3;
        }

        /// <summary>
        /// Convert a string to a Vector2, used in OBJ importing
        /// </summary>
        /// <param name="data"> String to convert </param>
        internal static Vector2 OBJStringToVector2(string data)
        {
            string[] Parts;
            Vector2 vector2 = new Vector2();

            if (!string.IsNullOrWhiteSpace(data))
            {
                if (data.Contains("/"))
                {
                    Parts = data.Replace(".", ",").Split("/".ToCharArray());

                    if (Parts.Length == 2 || Parts.Length == 3)
                    {
                        if (float.TryParse(Parts[0], out float X))
                        {
                            if (float.TryParse(Parts[1], out float Y))
                            {
                                vector2 = new Vector2(X, Y);
                            }

                            else
                            {
                                Debug.Error("Can not Parse Parts[1]: \"" + Parts[1] + "\"");
                            }
                        }

                        else
                        {
                            Debug.Error("Can not Parse Parts[0]: \"" + Parts[0] + "\"");
                        }
                    }

                    else
                    {
                        Debug.Error("Data parts = " + Parts.Length + ", it is supposed to be = 2");
                    }
                }

                else if (data.Contains(" "))
                {
                    Parts = data.Replace(".", ",").Split(" ".ToCharArray());

                    if (Parts.Length == 2 || Parts.Length == 3)
                    {
                        if (float.TryParse(Parts[0], out float X))
                        {
                            if (float.TryParse(Parts[1], out float Y))
                            {
                                vector2 = new Vector2(X, Y);
                            }

                            else
                            {
                                Debug.Error("Can not Parse Parts[1]: \"" + Parts[1] + "\"");
                            }
                        }

                        else
                        {
                            Debug.Error("Can not Parse Parts[0]: \"" + Parts[0] + "\"");
                        }
                    }

                    else
                    {
                        Debug.Error("Data parts = " + Parts.Length + ", it is supposed to be = 2");
                    }
                }

                else
                {
                    Debug.Error("Data does not contain any known split character");
                }
            }

            else
            {
                Debug.Error("data is null or empty");
            }

            return vector2;
        }

        /// <summary>
        /// Parses a string into polygon parts structured in Vector3[3] format, used in OBJ importing
        /// </summary>
        /// <param name="data"> String to convert </param>
        internal static List<Vector3[]> OBJStringToPolygonData(string data)
        {
            List<Vector3[]> PolygonData = new List<Vector3[]>();

            if (!string.IsNullOrWhiteSpace(data))
            {
                if (data.Contains(" "))
                {
                    string[] Parts = data.Replace(".", ",").Split(" ".ToCharArray());

                    if (Parts.Length >= 3)
                    {
                        for (int i = 0; i < Parts.Length - 2; i++)
                        {
                            Vector3[] TempPolygonData = new Vector3[3];

                            TempPolygonData[0] = OBJStringToVector3(Parts[0]) - new Vector3(1);
                            TempPolygonData[1] = OBJStringToVector3(Parts[i + 1]) - new Vector3(1);
                            TempPolygonData[2] = OBJStringToVector3(Parts[i + 2]) - new Vector3(1);

                            PolygonData.Add(TempPolygonData);
                        }
                    }

                    else
                    {
                        Debug.Error("Data parts = " + Parts.Length + ", it is supposed to be >= 3");
                    }
                }

                else
                {
                    Debug.Error("Data does not contain any known split character");
                }
            }

            else
            {
                Debug.Error("data is null or empty");
            }

            return PolygonData;
        }

        /// <summary>
        /// Import texture for DirectX11 renderer
        /// </summary>
        /// <param name="device"> DirectX11 target rendering device </param>
        /// <param name="TexturePath"> Source path for the texture </param>
        internal static Texture2D BitmapToDX11Texture(SharpDX.Direct3D11.Device device, string TexturePath)
        {
            ImagingFactory2 Fac = new ImagingFactory2();

            BitmapDecoder bitmapDecoder = new BitmapDecoder(Fac, TexturePath, DecodeOptions.CacheOnDemand
                );

            FormatConverter formatConverter = new FormatConverter(Fac);

            formatConverter.Initialize(
                bitmapDecoder.GetFrame(0),
                PixelFormat.Format32bppPRGBA,
                BitmapDitherType.None,
                null,
                0.0,
                BitmapPaletteType.Custom);

            using (var buffer = new DataStream(formatConverter.Size.Height * formatConverter.Size.Width * 4, true, true))
            {
                formatConverter.CopyPixels(formatConverter.Size.Width * 4, buffer);

                return new Texture2D(device, new Texture2DDescription()
                {
                    Width = formatConverter.Size.Width,
                    Height = formatConverter.Size.Height,
                    ArraySize = 1,
                    BindFlags = BindFlags.ShaderResource,
                    Usage = ResourceUsage.Immutable,
                    CpuAccessFlags = CpuAccessFlags.None,
                    Format = Format.R8G8B8A8_UNorm,
                    MipLevels = 1,
                    OptionFlags = ResourceOptionFlags.None,
                    SampleDescription = new SampleDescription(1, 0),
                }, new DataRectangle(buffer.DataPointer, formatConverter.Size.Width * 4));
            }
        }
    }
}