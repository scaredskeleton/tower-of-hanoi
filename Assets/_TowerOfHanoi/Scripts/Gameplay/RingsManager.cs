using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TowerOfHanoi.Core;
using TowerOfHanoi.Utilities;
using UnityEngine;

namespace TowerOfHanoi.Gameplay
{
    public class RingsManager : MonoBehaviour
    {
        public int Count { get => _count; }
        public float Thickness { get => _thickness; }
        public float MinRadius { get => _minRadius; }
        public float RadiusIncrement {  get => _radiusIncrement; }
        public List<Ring> ActiveRings { get; private set; } = new List<Ring>();
        public Ring LastRing { get => ActiveRings.Last(); }
        public Ring ActivatedRing { get; private set; }
        public CancellationTokenSource[] HoverCancellationSources { get; private set; }

        [SerializeField, Range(3, 8)] private int _count = 3;
        [SerializeField] private float _thickness;
        [SerializeField] private float _minRadius;
        [SerializeField] private float _radiusIncrement;
        [SerializeField] private List<Ring> _rings;

        private int _startingIndex;
        private int _activeCount;

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

        public void UpdateActiveRings()
        {
            _startingIndex = _rings.Count - _count;
            _activeCount = ((GameData.GameLevel == 1) ? 0 : 1) * (_count - 1);
            for (int i = 0; i < _count; ++i)
            {
                int ringIndex = _startingIndex + i;
                ActivatedRing = SetRingActive(ringIndex);
            }
        }

        public async Task PlayActivationAnimation()
        {
            GameplayManager.Instance.PegsManager.StartingPeg.Clear();

            Task[] activationTasks = new Task[(GameData.GameLevel == 1) ? 3 : 1];
            for (int i = 0; i < activationTasks.Length; ++i)
            {
                GameplayManager.Instance.PegsManager.StartingPeg.AddRing(ActiveRings[i]);
                if (ActiveRings[i].Index > _startingIndex && _activeCount > 0)
                    continue;
                else
                    activationTasks[i] = ActiveRings[i].PlayActivationAnimation();

                await Task.Delay(300);
            }

            await Task.WhenAll(activationTasks);
        }

        public async Task PlayHoveringAnimation()
        {
            Task[] hoverTasks = new Task[_activeCount];
            HoverCancellationSources = new CancellationTokenSource[hoverTasks.Length];
            for (int i = 0; i < hoverTasks.Length; ++i)
            {
                HoverCancellationSources[i] = new CancellationTokenSource();

                hoverTasks[i] = ActiveRings[ActiveRings.Count - 1 - i].PlayHoverAnimation(HoverCancellationSources[i]);
                await Task.Delay(100);
            }
        }

        public void FinishHover() => TaskUtilities.CancelTasks(HoverCancellationSources);

        public void DisposeHoverTokens() => TaskUtilities.DisposeCancellationSources(HoverCancellationSources);

        public async Task PlayTransferAnimation()
        {
            GameplayManager.Instance.PegsManager.StartingPeg.Clear();

            Task[] transferTasks = new Task[_activeCount];
            GameplayManager.Instance.PegsManager.StartingPeg.AddRing(ActivatedRing);
            for (int i = 0; i < transferTasks.Length; ++i)
            {
                Ring ring = ActiveRings[i + 1];
                GameplayManager.Instance.PegsManager.StartingPeg.AddRing(ring);
                transferTasks[i] = ring.PlayTransferAnimation();
                await Task.Delay(50);
            }

            await Task.WhenAll(transferTasks);
        }

        public async Task DropRings()
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

        public void UpdateRingsCount() => _count++;
    }
}