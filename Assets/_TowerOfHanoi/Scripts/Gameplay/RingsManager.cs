using System.Collections.Generic;
using System.Linq;
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
                GameplayManager.Instance.PegsManager.StartingPeg.PlaceRing(CreateRingObject(i));
            }
        }

        public bool IsLegalMove(Ring ring)
        {
            if (ring.TargetPeg.Rings.Count == 0)
                return true;
            else if (ring.TargetPeg.Rings.LastOrDefault() == null)
                return false;
            else if (ring.Index == ring.TargetPeg.Rings.LastOrDefault().Index)
                return true;
            else if (ring.Index > ring.TargetPeg.Rings.LastOrDefault().Index)
                return true;
            else
                return false;
        }

        private Ring CreateRingObject(int ringCount)
        {
            Ring ring = Instantiate(_ringPrefab);

            ring.name = $"Ring #{ringCount + 1}";
            SetRingRadius(ring, _count - ringCount);
            ring.SetIndex(ringCount);
            Rings.Add(ring);

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