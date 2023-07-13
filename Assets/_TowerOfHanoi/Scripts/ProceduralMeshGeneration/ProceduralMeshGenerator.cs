using System.Collections.Generic;
using UnityEngine;

namespace TowerOfHanoi.MeshGeneration
{
    public static class ProceduralMeshGenerator
    {
        public static Mesh Mesh;
        public static Vector3[] Vertices;
        public static int[] Triangles;

        public static Mesh GenerateRingMesh(int resolution, float outerRadius, float innerRadius)
        {
            Mesh = new Mesh();

            List<Vector3> vertices = new List<Vector3>();
            List<Vector3> outerVertices = GetCircumferenceVertices(resolution, outerRadius);
            vertices.AddRange(outerVertices);
            List<Vector3> innerVertices = GetCircumferenceVertices(resolution, innerRadius);
            vertices.AddRange(innerVertices);

            Vertices = vertices.ToArray();
            Triangles = DrawRingTriangles(Vertices);

            return DrawMesh();
        }

        public static Mesh GenerateCircle(int resolution, float radius)
        {
            Mesh = new Mesh();

            Vertices = GetCircumferenceVertices(resolution, radius).ToArray();
            Triangles = DrawingCircleTriangles(Vertices);

            return DrawMesh();
        }

        private static Mesh DrawMesh()
        {
            Mesh.Clear();
            Mesh.vertices = Vertices;
            Mesh.triangles = Triangles;

            return Mesh;
        }

        private static List<Vector3> GetCircumferenceVertices(int resolution, float radius)
        {
            List<Vector3> vertices = new List<Vector3>();
            float circumferenceVertexDistance = (float)1 / resolution;
            float TAU = 2 * Mathf.PI;
            float radianDistance = circumferenceVertexDistance * TAU;

            for (int i = 0; i < resolution; i++)
            {
                float currentRadian = radianDistance * i;
                float posX = Mathf.Cos(currentRadian) * radius;
                float posY = Mathf.Sin(currentRadian) * radius;
                
                vertices.Add(new Vector3(posX, posY, 0));

                Debug.Log(vertices[i]);
            }

            return vertices;
        }

        private static int[] DrawRingTriangles(Vector3[] vertices)
        {
            int sides = vertices.Length / 2;

            List<int> triangles = new List<int>();
            for (int i = 0; i < sides; i++)
            {
                int outerPoint = i;
                int innerPoint = i + sides;

                triangles.Add(outerPoint);
                triangles.Add(innerPoint);
                triangles.Add((i + 1) % sides);

                triangles.Add(outerPoint);
                triangles.Add(sides + ((sides + i - 1) % sides));
                triangles.Add(outerPoint + sides);
            }

            return triangles.ToArray();
        }

        private static int[] DrawingCircleTriangles(Vector3[] vertices)
        {
            int trianglesCount = vertices.Length - 2;
            List<int> triangles = new List<int>();
            for (int i = 0; i < trianglesCount; i++)
            {
                triangles.Add(0);
                triangles.Add(i + 2);
                triangles.Add(i + 1);
            }

            return triangles.ToArray();
        }
    }
}