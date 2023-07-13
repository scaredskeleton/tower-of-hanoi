using TowerOfHanoi.MeshGeneration;
using UnityEngine;

namespace TowerOfHanoi.Gameplay
{
    public class RingsManager : MonoBehaviour
    {
        [SerializeField] private int _count;
        [SerializeField] private float _thickness;
        [SerializeField] private float _minRadius;
        
        private int _minRingsCount = 3;

        public float MinRadius { get => _minRadius; }

        public void GenerateRings()
        {
            for (int i = 0; i < _count; ++i)
            {
                CreateRingObject(i + 1);
            }
        }

        private void CreateRingObject(int ringCount)
        {
            Ring ring = new GameObject($"Ring {ringCount}").AddComponent<Ring>();
            ring.MeshFilter.mesh = ProceduralMeshGenerator.GenerateRingMesh(0,0,0);
        }
    }
}