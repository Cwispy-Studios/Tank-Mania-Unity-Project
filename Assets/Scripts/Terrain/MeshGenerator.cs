<<<<<<< HEAD
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CwispyStudios.TankMania.Terrain {
    public static class MeshGenerator {
        
        public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier, AnimationCurve heightCurve,int levelOfDetail) {
            int width = heightMap.GetLength(0);
            int height = heightMap.GetLength(1);
            float topLeftX = (width - 1) / -2f;
            float topLeftZ = (height - 1) / 2f;

            int meshSimplificationIncrement = (levelOfDetail == 0)?1:levelOfDetail * 2;
            int verticesPerLine = (width - 1) / meshSimplificationIncrement + 1;

            MeshData meshData = new MeshData(verticesPerLine, verticesPerLine);
            int vertexIndex = 0;
            
            for (int y = 0; y < height; y += meshSimplificationIncrement) {
                for (int x = 0; x < width; x += meshSimplificationIncrement)
                {
                    meshData.vertices[vertexIndex] = new Vector3(topLeftX + x, heightCurve.Evaluate(heightMap[x,y]) * heightMultiplier, topLeftZ - y);
                    meshData.uvs[vertexIndex] = new Vector2(x / (float) width, y / (float) height);

                    if (x < width -1 && y < height -1) {
                        meshData.AddTriangle(vertexIndex, vertexIndex + verticesPerLine + 1, vertexIndex + verticesPerLine);
                        meshData.AddTriangle(vertexIndex + verticesPerLine + 1, vertexIndex, vertexIndex + 1);
=======
using UnityEngine;

namespace CwispyStudios.TankMania.Terrain 
{
    public static class MeshGenerator
    {
        public const int numSupportedLODs = 5;
        public const int numSupportedChunkSizes = 9;
        public const int numSupportedFlatshadedChunkSizes = 3;
        public static readonly int[] supportedChunkSizes = { 48, 72, 96, 120, 144, 168, 192, 216, 240 };
        public static readonly int[] supportedFlatshadedChunkSizes = { 48, 72, 96 };
        
        public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier, AnimationCurve _heightCurve,int levelOfDetail, bool useFlatShading)
        {
            AnimationCurve heightCurve = new AnimationCurve(_heightCurve.keys);
            
            int meshSimplificationIncrement = (levelOfDetail == 0)?1:levelOfDetail * 2;
            
            int borderedSize = heightMap.GetLength(0);
            int meshSize = borderedSize - 2 * meshSimplificationIncrement;
            int meshSizeUnsimplified = borderedSize - 2;
            
            float topLeftX = (meshSizeUnsimplified - 1) / -2f;
            float topLeftZ = (meshSizeUnsimplified - 1) / 2f;
            
            int verticesPerLine = (meshSize - 1) / meshSimplificationIncrement + 1;

            MeshData meshData = new MeshData(verticesPerLine, useFlatShading);

            int[,] vertexIndicesMap = new int[borderedSize, borderedSize];
            int meshVertexIndex = 0;
            int borderVertexIndex = -1;

            for (int y = 0; y < borderedSize; y += meshSimplificationIncrement)
            {
                for (int x = 0; x < borderedSize; x += meshSimplificationIncrement)
                {
                    bool isBorderVertex = y == 0 || y == borderedSize - 1 || x == 0 || x == borderedSize - 1;

                    if (isBorderVertex)
                    {
                        vertexIndicesMap[x, y] = borderVertexIndex;
                        borderVertexIndex--;
                    }
                    else
                    {
                        vertexIndicesMap[x, y] = meshVertexIndex;
                        meshVertexIndex++;
                    }
                }
            }

            for (int y = 0; y < borderedSize; y += meshSimplificationIncrement) {
                for (int x = 0; x < borderedSize; x += meshSimplificationIncrement)
                {
                    int vertexIndex = vertexIndicesMap[x, y];
                    Vector2 percent = new Vector2((x-meshSimplificationIncrement) / (float) meshSize, (y-meshSimplificationIncrement) / (float) meshSize);
                    float height = heightCurve.Evaluate(heightMap[x, y]) * heightMultiplier;
                    Vector3 vertexPosition = new Vector3(topLeftX + percent.x * meshSizeUnsimplified, height, topLeftZ - percent.y * meshSizeUnsimplified);

                    meshData.AddVertex(vertexPosition, percent, vertexIndex);
                    
                    if (x < borderedSize -1 && y < borderedSize -1)
                    {
                        int a = vertexIndicesMap[x, y];
                        int b = vertexIndicesMap[x + meshSimplificationIncrement, y];
                        int c = vertexIndicesMap[x, y + meshSimplificationIncrement];
                        int d = vertexIndicesMap[x + meshSimplificationIncrement, y + meshSimplificationIncrement];
                        meshData.AddTriangle(a,d,c);
                        meshData.AddTriangle(d,a,b);
>>>>>>> develop
                    }
                    
                    vertexIndex++;
                }
            }
<<<<<<< HEAD

            return meshData;
        }
        
=======
            
            meshData.FinalizeGeneration();

            return meshData;
        }
>>>>>>> develop
    }

    public class MeshData
    {
<<<<<<< HEAD
        public Vector3[] vertices;
        public int[] triangles;
        public Vector2[] uvs;

        private int triangleIndex;

        public MeshData(int meshWidth, int meshHeight)
        {
            vertices = new Vector3[meshWidth * meshHeight];
            uvs = new Vector2[meshWidth * meshHeight];
            triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
=======
        private Vector3[] vertices;
        private int[] triangles;
        private Vector2[] uvs;
        private Vector3[] bakedNormals;

        private Vector3[] borderVertices;
        private int[] borderTriangles;

        private int triangleIndex;
        private int borderTriangleIndex;

        private bool useFlatShading;

        public MeshData(int verticesPerLine, bool useFlatShading)
        {
            this.useFlatShading = useFlatShading;
            vertices = new Vector3[verticesPerLine * verticesPerLine];
            uvs = new Vector2[verticesPerLine * verticesPerLine];
            triangles = new int[(verticesPerLine - 1) * (verticesPerLine - 1) * 6];

            borderVertices = new Vector3[verticesPerLine * 4 + 4];
            borderTriangles = new int[24 * verticesPerLine];
        }

        public void AddVertex(Vector3 vertexPosition, Vector2 uv, int vertexIndex)
        {
            if (vertexIndex < 0) //Check if bordervertex
            {
                borderVertices[-vertexIndex - 1] = vertexPosition;
            }
            else 
            {
                vertices[vertexIndex] = vertexPosition;
                uvs[vertexIndex] = uv;
            }
>>>>>>> develop
        }

        public void AddTriangle(int a, int b, int c)
        {
<<<<<<< HEAD
            triangles[triangleIndex] = a;
            triangles[triangleIndex + 1] = b;
            triangles[triangleIndex + 2] = c;
            triangleIndex += 3;
=======
            if (a < 0 || b < 0 || c < 0) //Check whether the triangle belongs to the border
            {
                borderTriangles[borderTriangleIndex] = a;
                borderTriangles[borderTriangleIndex + 1] = b;
                borderTriangles[borderTriangleIndex + 2] = c;
                borderTriangleIndex += 3;
            }
            else //else its a regular triangle
            {
                triangles[triangleIndex] = a;
                triangles[triangleIndex + 1] = b;
                triangles[triangleIndex + 2] = c;
                triangleIndex += 3;
            }
            
        }

        private Vector3[] CalculateNormals()
        {
            Vector3[] vertexNormals = new Vector3[vertices.Length];
            int triangleCount = triangles.Length / 3;
            for (int i = 0; i < triangleCount; i++) {
                int normalTriangleIndex = i * 3;
                int vertexIndexA = triangles[normalTriangleIndex];
                int vertexIndexB = triangles[normalTriangleIndex + 1];
                int vertexIndexC = triangles[normalTriangleIndex + 2];

                Vector3 triangleNormal = SurfaceNormalFromIndices(vertexIndexA, vertexIndexB, vertexIndexC);
                vertexNormals[vertexIndexA] += triangleNormal;
                vertexNormals[vertexIndexB] += triangleNormal;
                vertexNormals[vertexIndexC] += triangleNormal;
            }
            
            int borderTriangleCount = borderTriangles.Length / 3;
            for (int i = 0; i < borderTriangleCount; i++) 
            {
                int normalTriangleIndex = i * 3;
                int vertexIndexA = borderTriangles[normalTriangleIndex];
                int vertexIndexB = borderTriangles[normalTriangleIndex + 1];
                int vertexIndexC = borderTriangles[normalTriangleIndex + 2];

                Vector3 triangleNormal = SurfaceNormalFromIndices(vertexIndexA, vertexIndexB, vertexIndexC);
                if (vertexIndexA >= 0)
                {
                    vertexNormals[vertexIndexA] += triangleNormal;
                }
                if (vertexIndexB>= 0)
                {
                    vertexNormals[vertexIndexB] += triangleNormal;   
                }
                if (vertexIndexC >= 0)
                {
                    vertexNormals[vertexIndexC] += triangleNormal;
                }
            }

            for (int i = 0; i < vertexNormals.Length; i++)
            {
                vertexNormals[i].Normalize();
            }

            return vertexNormals;
        }

        private Vector3 SurfaceNormalFromIndices(int indexA, int indexB, int indexC)
        {
            Vector3 pointA = (indexA <0)? borderVertices [-indexA-1] : vertices[indexA];
            Vector3 pointB = (indexB <0)? borderVertices [-indexB-1] : vertices[indexB];
            Vector3 pointC = (indexC <0)? borderVertices [-indexC-1] : vertices[indexC];

            Vector3 sideAB = pointB - pointA;
            Vector3 sideAC = pointC - pointA;
            return  Vector3.Cross(sideAB, sideAC).normalized;
        }

        public void FinalizeGeneration()
        {
            if (useFlatShading) {
                FlatShading();
            } else {
                BakeNormals();
            }
        }

        private void BakeNormals()
        {
            bakedNormals = CalculateNormals();
        }

        private void FlatShading()  // Optional shading method (2 triangles have 6 vertices instead of 4)
        {
            Vector3[] flatShadedVertices = new Vector3[triangles.Length];
            Vector2[] flatShadedUvs = new Vector2[triangles.Length];

            for (int i = 0; i < triangles.Length; i++)
            {
                flatShadedVertices[i] = vertices[triangles[i]];
                flatShadedUvs[i] = uvs[triangles[i]];
                triangles[i] = i;
            }

            vertices = flatShadedVertices;
            uvs = flatShadedUvs;
>>>>>>> develop
        }

        public Mesh CreateMesh()
        {
            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;
<<<<<<< HEAD
            mesh.RecalculateNormals();
=======
            if (useFlatShading) {
                mesh.RecalculateNormals();
            } else {
                mesh.normals = bakedNormals;
            }
>>>>>>> develop
            return mesh;
        }
    }
}

