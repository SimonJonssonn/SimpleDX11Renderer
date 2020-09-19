using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
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
using System.Windows.Forms;

namespace SimpleDX11Renderer
{
    public static class OBJimporter
    {
        private class OBJstruct
        {
            public List<Vector3> VertexPositions;
            public List<Vector2> VertexUVs;
            public List<Vector3> VertexNormals;
            public List<Vector3[]> Polygons;

            public OBJstruct()
            {
                VertexPositions = new List<Vector3>();
                VertexUVs = new List<Vector2>();
                VertexNormals = new List<Vector3>();
                Polygons = new List<Vector3[]>();
            }
        }

        public static Mesh[] ImportOBJ(string FilePath)
        {
            if (File.Exists(FilePath))
            {
                if (Path.GetExtension(FilePath).ToLower() == ".obj")
                {
                    using (StreamReader streamReader = new StreamReader(FilePath))
                    {
                        string Data = streamReader.ReadToEnd();
                        string[] FileLines = Data.Split("\r\n".ToCharArray());
                        List<OBJstruct> CleanFileLines = new List<OBJstruct>() { new OBJstruct() };

                        for (int i = 0; i < FileLines.Length; i++)
                        {
                            if (FileLines[i].StartsWith("v "))
                            {
                                CleanFileLines[CleanFileLines.Count - 1].VertexPositions.Add(Convert.OBJStringToVector3(FileLines[i].Substring(2)));
                            }

                            else if (FileLines[i].StartsWith("vt "))
                            {
                                CleanFileLines[CleanFileLines.Count - 1].VertexUVs.Add(Convert.OBJStringToVector2(FileLines[i].Substring(3)));
                            }

                            else if (FileLines[i].StartsWith("vn "))
                            {
                                CleanFileLines[CleanFileLines.Count - 1].VertexNormals.Add(Convert.OBJStringToVector3(FileLines[i].Substring(3)));
                            }

                            else if (FileLines[i].StartsWith("f "))
                            {
                                CleanFileLines[CleanFileLines.Count - 1].Polygons.AddRange(Convert.OBJStringToPolygonData(FileLines[i].Substring(2)));
                            }

                            else if (FileLines[i].StartsWith("g "))
                            {
                                CleanFileLines.Add(new OBJstruct());
                            }
                        }

                        for (int i = CleanFileLines.Count - 1; i >= 0; i--)
                        {
                            if (CleanFileLines[i].Polygons.Count == 0 &&
                                CleanFileLines[i].VertexPositions.Count == 0 &&
                                CleanFileLines[i].VertexNormals.Count == 0 &&
                                CleanFileLines[i].VertexUVs.Count == 0)
                            {
                                CleanFileLines.RemoveAt(i);
                            }
                        }

                        if (CleanFileLines.Count > 0)
                        {
                            Mesh[] meshes = new Mesh[CleanFileLines.Count];

                            for (int i = 0; i < CleanFileLines.Count; i++)
                            {
                                List<Vertex> vertices = new List<Vertex>();
                                List<Polygon> polygons = new List<Polygon>();

                                for (int u = 0; u < CleanFileLines[i].Polygons.Count; u++)
                                {
                                    for (int k = 0; k < 3; k++)
                                    {
                                        Vector3 Position = new Vector3();
                                        Vector2 UV = new Vector2();
                                        Vector3 Normal = new Vector3();

                                        if (CleanFileLines[i].Polygons[u][k].X >= 0)
                                        {
                                            if (CleanFileLines[i].Polygons[u][k].X < CleanFileLines[i].VertexPositions.Count)
                                            {
                                                Position = CleanFileLines[i].VertexPositions[(int)CleanFileLines[i].Polygons[u][k].X];
                                            }

                                            else
                                            {
                                                Debug.Error("Trying to get Vertex Position, got index out of range: " + CleanFileLines[i].Polygons[u][k].X + " > " + (CleanFileLines[i].VertexPositions.Count - 1));
                                            }
                                        }

                                        if (CleanFileLines[i].Polygons[u][k].Y >= 0)
                                        {
                                            if (CleanFileLines[i].Polygons[u][k].Y < CleanFileLines[i].VertexUVs.Count)
                                            {
                                                UV = CleanFileLines[i].VertexUVs[(int)CleanFileLines[i].Polygons[u][k].Y];
                                            }

                                            else
                                            {
                                                Debug.Error("Trying to get Vertex UV, got index out of range: " + CleanFileLines[i].Polygons[u][k].Y + " > " + (CleanFileLines[i].VertexUVs.Count - 1));
                                            }
                                        }

                                        if (CleanFileLines[i].Polygons[u][k].Z >= 0)
                                        {
                                            if (CleanFileLines[i].Polygons[u][k].Z < CleanFileLines[i].VertexNormals.Count)
                                            {
                                                Normal = CleanFileLines[i].VertexNormals[(int)CleanFileLines[i].Polygons[u][k].Z];
                                            }

                                            else
                                            {
                                                Debug.Error("Trying to get Vertex Normal, got index out of range: " + CleanFileLines[i].Polygons[u][k].Z + " > " + (CleanFileLines[i].VertexNormals.Count - 1));
                                            }
                                        }

                                        vertices.Add(new Vertex(Position, UV, Normal));
                                    }

                                    polygons.Add(new Polygon(u * 3, u * 3 + 1, u * 3 + 2));
                                }

                                meshes[i] = new Mesh(vertices, polygons);
                            }

                            return meshes;
                        }
                    }
                }

                else
                {
                    Debug.Error("Invalid File extension");
                }
            }

            else
            {
                Debug.Error("Invalid File Path");
            }

            return new Mesh[0];
        }

        public static Mesh[] ImportOBJ(string FilePath, string CopyDirectory)
        {
            if (File.Exists(FilePath))
            {
                if (Path.GetExtension(FilePath).ToLower() == ".obj")
                {
                    if (CopyDirectory[CopyDirectory.Length - 1] == '\\')
                    {
                        CopyDirectory += "\\";
                    }

                    if (Directory.Exists(CopyDirectory))
                    {
                        File.Copy(FilePath, CopyDirectory + Path.GetFileName(FilePath));

                        return ImportOBJ(CopyDirectory + Path.GetFileName(FilePath));
                    }

                    else
                    {
                        Debug.Error("Invalid Copy Path");
                    }
                }

                else
                {
                    Debug.Error("Invalid File extension");
                }
            }

            else
            {
                Debug.Error("Invalid File Path");
            }

            return new Mesh[0];
        }

        public static Mesh[] ImportOBJGUI()
        {
            string FilePath;

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = Environment.CurrentDirectory,
                Filter = "OBJ files (*.obj)|*.obj",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            openFileDialog.ShowDialog();

            try
            {
                using (StreamReader streamReader = new StreamReader(openFileDialog.OpenFile()))
                {
                    FilePath = openFileDialog.FileName;
                }

                return ImportOBJ(FilePath);
            }

            catch
            {
                return new Mesh[0];
            }
        }

        public static Mesh[] ImportOBJGUI(string CopyDirectory)
        {
            try
            {
                string FilePath;

                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    InitialDirectory = Environment.CurrentDirectory,
                    Filter = "OBJ files (*.obj)|*.obj",
                    FilterIndex = 1,
                    RestoreDirectory = true
                };

                openFileDialog.ShowDialog();

                using (StreamReader streamReader = new StreamReader(openFileDialog.OpenFile()))
                {
                    FilePath = openFileDialog.FileName;
                }

                return ImportOBJ(FilePath, CopyDirectory);
            }

            catch
            {
                return new Mesh[0];
            }
        }

        public static Bitmap ImportTextue(string FilePath)
        {
            if (File.Exists(FilePath))
            {
                if (Path.GetExtension(FilePath).ToLower() == ".png" ||
                    Path.GetExtension(FilePath).ToLower() == ".jpg" ||
                    Path.GetExtension(FilePath).ToLower() == ".bmp" ||
                    Path.GetExtension(FilePath).ToLower() == ".tif" || 
                    Path.GetExtension(FilePath).ToLower() == ".tiff")
                {
                    return (Bitmap)Image.FromFile(FilePath);
                }

                else
                {
                    Debug.Error("Invalid File extension");
                }
            }

            else
            {
                Debug.Error("Invalid File Path");
            }

            return new Bitmap(1, 1);
        }

        public static Bitmap ImportTextueGUI()
        {
            string FilePath;

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = Environment.CurrentDirectory,
                Filter = "OBJ files (*.obj)|*.obj",
                FilterIndex = 1,
                RestoreDirectory = true
            };
            openFileDialog.ShowDialog();

            using (StreamReader streamReader = new StreamReader(openFileDialog.OpenFile()))
            {
                FilePath = openFileDialog.FileName;
            }

            return ImportTextue(FilePath);
        }
    }
}