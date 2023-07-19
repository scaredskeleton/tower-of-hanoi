using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using TowerOfHanoi.Core;
using TowerOfHanoi.Utilities;
using UnityEngine;

namespace TowerOfHanoi.Gameplay
{
    public class RingsManager : MonoBehaviour
    {
        [SerializeField, Range(3, 8)] private int _count = 3;
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
                UpdateActiveRings();
            else
            {
                ClearRings();
                UpdateActiveRings();
            }
        }

        public async void UpdateActiveRings()
        {
            int startingIndex = _rings.Count - _count;
            int activeCount = ((GameData.GameLevel == 1) ? 0 : 1) * (_count - 1);
            Task[] activationTasks = new Task[(GameData.GameLevel == 1) ? 3 : 1];
            Task[] hoverTasks = new Task[activeCount];
            Ring activatedRing = null;
            for (int i = 0; i < _count; ++i)
            {
                int ringIndex = startingIndex + i;
                activatedRing = SetRingActive(ringIndex);
                GameplayManager.Instance.PegsManager.StartingPeg.AddRing(activatedRing);

                if (activatedRing.Index > startingIndex && activeCount > 0)
                    continue;
                else
                    activationTasks[i] = activatedRing.PlayActivationAnimation();
                    
                await Task.Delay(300);
            }

            var hoverCancellationSources = new CancellationTokenSource[hoverTasks.Length];
            for (int i = 0; i < hoverTasks.Length; ++i)
            {
                hoverCancellationSources[i] = new CancellationTokenSource();

                hoverTasks[i] = ActiveRings[ActiveRings.Count - 1 -i].PlayHoverAnimation(hoverCancellationSources[i]);
                await Task.Delay(100);
            }
            
            await Task.WhenAll(activationTasks);

            TaskUtilities.CancelTasks(hoverCancellationSources);

            await Task.WhenAll(hoverTasks);

            TaskUtilities.DisposeCancellationSources(hoverCancellationSources);
            
            GameplayManager.Instance.PegsManager.StartingPeg.Clear();


            Task[] transferTasks = new Task[activeCount];
            GameplayManager.Instance.PegsManager.StartingPeg.AddRing(activatedRing);
            for (int i = 0; i < transferTasks.Length; ++i)
            {
                Ring ring = ActiveRings[i + 1];
                GameplayManager.Instance.PegsManager.StartingPeg.AddRing(ring);
                transferTasks[i] = ring.PlayTransferAnimation();
                await Task.Delay(50);
            }

            await Task.WhenAll(transferTasks);

            DropRings();
        }

        private async void DropRings()
        {
            GameplayManager.Instance.PegsManager.StartingPeg.Clear();

            Task[] tasks = new Task[ActiveRings.Count];
            for (int i = 0; i < ActiveRings.Count; i++)
            {
                tasks[i] = GameplayManager.Instance.PegsManager.StartingPeg.PlaceRing(ActiveRings[i]);
                await Task.Delay(50);
            }

            await Task.WhenAll(tasks);
        }

        private Ring SetRingActive(int ringIndex)
        {
            Ring ring = _rings[ringIndex];
            ActiveRings.Add(ring);
            ring.SetActive();

            return ring;
        }

        private void ClearRings() => ActiveRings.Clear();

        public void UpdateRingsCount() => _count += GameData.GameLevel - 1;
    }
}