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
    /// Structure to be drawn
    /// </summary>
    public class Mesh
    {
        /// <summary>
        /// Polygons for the mesh
        /// </summary>
        private List<Polygon> Polygons = new List<Polygon>();

        /// <summary>
        /// Vertices for the mesh
        /// </summary>
        private List<Vertex> Vertices = new List<Vertex>();

        /// <summary>
        /// Vertecies for drawing
        /// </summary>
        private Vertex[] DrawVerts = new Vertex[0];

        public string Name = "Mesh";

        /// <summary>
        /// Create a blank mesh
        /// </summary>
        public Mesh()
        {

        }

        /// <summary>
        /// Create a new mesh
        /// </summary>
        /// <param name="vertices"> Vertices for the mesh </param>
        /// <param name="polygons"> Polygons for the mesh </param>
        public Mesh(List<Vertex> vertices, List<Polygon> polygons)
        {
            Polygons = polygons;
            Vertices = vertices;
        }

        /// <summary>
        /// Get the meshes polygons
        /// </summary>
        public List<Polygon> GetPolygons()
        {
            return Polygons;
        }

        /// <summary>
        /// Get the meshes vertices
        /// </summary>
        public List<Vertex> GetVertices()
        {
            return Vertices;
        }

        /// <summary>
        /// Set the meshes polygons
        /// </summary>
        /// <param name="polygons"> The polygons to be set </param>
        public void SetPolygons(List<Polygon> polygons)
        {
            Polygons = polygons;
            DrawVerts = new Vertex[0];
        }

        /// <summary>
        /// Set the meshes vertices
        /// </summary>
        /// <param name="vertices"> The vertices to be set </param>
        public void SetVertices(List<Vertex> vertices)
        {
            Vertices = vertices;
            DrawVerts = new Vertex[0];
        }

        /// <summary>
        /// Add a polygon to the mesh
        /// </summary>
        /// <param name="polygon"> The polygon to be added </param>
        public void AddPolygons(Polygon polygon)
        {
            Polygons.Add(polygon);
            DrawVerts = new Vertex[0];
        }

        /// <summary>
        /// Add a vertex to the mesh
        /// </summary>
        /// <param name="vertex"> The vertex to be added </param>
        public void AddVertices(Vertex vertex)
        {
            Vertices.Add(vertex);
            DrawVerts = new Vertex[0];
        }

        /// <summary>
        /// Add polygons to the mesh
        /// </summary>
        /// <param name="polygons"> The polygons to be added </param>
        public void AddPolygons(List<Polygon> polygons)
        {
            Polygons.AddRange(polygons);
            DrawVerts = new Vertex[0];
        }

        /// <summary>
        /// Add vertices to the mesh
        /// </summary>
        /// <param name="vertices"> The vertices to be added </param>
        public void AddVertices(List<Vertex> vertices)
        {
            Vertices.AddRange(vertices);
            DrawVerts = new Vertex[0];
        }

        /// <summary>
        /// Remove polygon from the mesh
        /// </summary>
        /// <param name="polygonID"> ID of the polygon </param>
        public void RemovePolygon(int polygonID)
        {
            Polygons.RemoveAt(polygonID);
            DrawVerts = new Vertex[0];
        }

        /// <summary>
        /// Remove vertex from the mesh
        /// </summary>
        /// <param name="vertexID"> ID of the vertex </param>
        public void RemoveVertex(int vertexID)
        {
            Vertices.RemoveAt(vertexID);
            DrawVerts = new Vertex[0];
        }

        /// <summary>
        /// Clear all polyogns from the mesh
        /// </summary>
        public void ClearPolygons()
        {
            Polygons.Clear();
            DrawVerts = new Vertex[0];
        }

        /// <summary>
        /// Clear all vertecies from the mesh
        /// </summary>
        public void ClearVertecies()
        {
            Vertices.Clear();
            DrawVerts = new Vertex[0];
        }

        public void GenerateNormals()
        {
            List<Vertex> NewVertices = new List<Vertex>();
            List<Polygon> NewPolygons = new List<Polygon>();

            if (Vertices.Count > 0)
            {
                for (int i = 0; i < Polygons.Count; i++)
                {
                    if (Polygons[i].VerticesIDs.Length == 3)
                    {
                        if (Vertices.Count > Polygons[i].VerticesIDs[0] &&
                        Vertices.Count > Polygons[i].VerticesIDs[1] &&
                        Vertices.Count > Polygons[i].VerticesIDs[2])
                        {
                            Vertex vertex1 = Vertices[Polygons[i].VerticesIDs[0]];
                            Vertex vertex2 = Vertices[Polygons[i].VerticesIDs[1]];
                            Vertex vertex3 = Vertices[Polygons[i].VerticesIDs[2]];

                            Vector3 PosDelta1 = vertex1.Position - vertex2.Position;
                            Vector3 PosDelta2 = vertex1.Position - vertex3.Position;

                            Vector2 UVDelta1 = vertex1.UV - vertex2.UV;
                            Vector2 UVDelta2 = vertex1.UV - vertex3.UV;

                            if (UVDelta1.X == 0)
                                UVDelta1.X = 0.0001f;

                            if (UVDelta1.Y == 0)
                                UVDelta1.Y = 0.0001f;

                            if (UVDelta2.X == 0)
                                UVDelta2.X = 0.0001f;

                            if (UVDelta2.Y == 0)
                                UVDelta2.Y = 0.0001f;


                            PosDelta1 /= UVDelta1.X;
                            PosDelta2 /= UVDelta2.X;

                            UVDelta1 /= UVDelta1.X;
                            UVDelta2 /= UVDelta2.X;

                            Vector3 Tangent = PosDelta2 - PosDelta1;
                            Vector2 TangentUVDelta = UVDelta2 - UVDelta1;

                            Tangent /= TangentUVDelta.Y;


                            PosDelta1 = vertex1.Position - vertex2.Position;
                            PosDelta2 = vertex1.Position - vertex3.Position;

                            Vector3 Normal = Vector3.Cross(PosDelta2, PosDelta1);

                            Normal.Normalize();
                            Tangent.Normalize();

                            Vector3 BiNormal = Vector3.Cross(Normal, Tangent);

                            vertex1.Normal = Normal;
                            vertex1.BiNormal = BiNormal;
                            vertex1.Tangent = Tangent;

                            vertex2.Normal = Normal;
                            vertex2.BiNormal = BiNormal;
                            vertex2.Tangent = Tangent;

                            vertex3.Normal = Normal;
                            vertex3.BiNormal = BiNormal;
                            vertex3.Tangent = Tangent;

                            NewVertices.AddRange(new Vertex[] { vertex1, vertex2, vertex3 });
                            NewPolygons.Add(new Polygon(i * 3, i * 3 + 1, i * 3 + 2));
                        }

                        else
                        {
                            Debug.Error("");
                        }
                    }
                }

                Vertices = NewVertices;
                Polygons = NewPolygons;
            }
        }

        /// <summary>
        /// Get the vertices in a format to be drawn
        /// </summary>
        internal Vertex[] GetDrawVertices()
        {
            if(DrawVerts.Length == 0)
            {
                Vertex[] vertices = new Vertex[Polygons.Count * 3];

                for (int i = 0; i < Polygons.Count; i++)
                {
                    vertices[i * 3] = Vertices[Polygons[i].VerticesIDs[0]];
                    vertices[i * 3 + 1] = Vertices[Polygons[i].VerticesIDs[1]];
                    vertices[i * 3 + 2] = Vertices[Polygons[i].VerticesIDs[2]];
                }

                return vertices;
            }

            return DrawVerts;
        }
    }
}