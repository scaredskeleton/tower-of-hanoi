using System.Collections.Generic;
using UnityEngine;

namespace TowerOfHanoi.Gameplay
{
    public class RingsManager : MonoBehaviour
    {
        [SerializeField, Range(3, 15)] private int _count = 3;
        [SerializeField] private float _thickness;
        [SerializeField] private float _minRadius;
        [SerializeField] private float _radiusIncrement;
        [SerializeField] private Ring _ringPrefab;
        
        public float Count { get => _count; }
        public float Thickness { get => _thickness; }
        public float MinRadius { get => _minRadius; }
        public List<Ring> Rings { get; private set; } = new List<Ring>();

        private void Start()
        {
            if (Rings != null || Rings.Count > 0)
                GenerateRings();
            else
            {
                ClearRings();
                GenerateRings();
            }
        }

        public void GenerateRings()
        {
            for (int i = 0; i < _count; ++i)
            {
                Rings.Add(CreateRingObject(i));
            }
        }

        private Ring CreateRingObject(int ringCount)
        {
            Vector3 spawnpoint = GameplayManager.Instance.PegsManager.Pegs[0].RingsSpawnpoint;
            spawnpoint.y += _thickness + (_thickness * 2 * ringCount);

            Ring ring = Instantiate(_ringPrefab, spawnpoint, Quaternion.identity);
            SetRingRadius(ring, _count - ringCount);

            return ring;
        }

        private void SetRingRadius(Ring ring, int index)
        {
            float radius = _minRadius + (_radiusIncrement * index);
            ring.transform.localScale = new Vector3(radius, _thickness, radius);
        }

        private void ClearRings()
        {
            foreach (Ring ring in Rings)
            {
                Destroy(ring.gameObject);
            }

            Rings.Clear();
        }
    }
}