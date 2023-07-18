using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TowerOfHanoi.Core;
using UnityEngine;

namespace TowerOfHanoi.Gameplay
{
    public class RingsManager : MonoBehaviour
    {
        [SerializeField, Range(3, 15)] private int _count = 3;
        [SerializeField] private float _thickness;
        [SerializeField] private float _minRadius;
        [SerializeField] private float _radiusIncrement;
        [SerializeField] private List<Ring> _rings;
        
        public int Count { get => _count; }
        public float Thickness { get => _thickness; }
        public float MinRadius { get => _minRadius; }
        public float RadiusIncrement {  get => _radiusIncrement; }
        public List<Ring> ActiveRings { get; private set; } = new List<Ring>();
        public Ring LastRing { get => ActiveRings.Last(); }

        private void Start()
        {
            for ( int i = 0; i < _rings.Count; i++ )
            {
                _rings[i].SetIndex(i);
            }
        }

        public void Initialize()
        {
            if (ActiveRings  == null || ActiveRings.Count == 0)
                SetActiveRings();
            else
            {
                ClearRings();
                SetActiveRings();
            }
        }

        public async void SetActiveRings()
        {
            int startingIndex = _rings.Count - _count;
            Task[] tasks = new Task[_count];
            for (int i = 0; i < _count; ++i)
            {
                int ringIndex = startingIndex + i;
                Ring ring = SetRingActive(ringIndex);
                GameplayManager.Instance.PegsManager.StartingPeg.PlaceRing(ring);
                tasks[i] = ring.PlayPlaceAnimation();
                await Task.Delay(300);
            }

            await Task.WhenAll(tasks);
        }

        private Ring SetRingActive(int ringIndex)
        {
            Ring ring = _rings[ringIndex];
            ring.SetActive();
            ActiveRings.Add(ring);

            return ring;
        }

        private void ClearRings() => ActiveRings.Clear();

        public void UpdateRingsCount() => _count += GameData.GameLevel - 1;
    }
}