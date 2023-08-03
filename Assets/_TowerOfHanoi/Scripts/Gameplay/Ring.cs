using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TowerOfHanoi.Animation;
using TowerOfHanoi.Utilities;
using UnityEngine;

namespace TowerOfHanoi.Gameplay
{
    [RequireComponent(typeof(SkinnedMeshRenderer))]
    [RequireComponent(typeof(RingSizer))]
    [RequireComponent(typeof(BoxCollider))]
    public class Ring : MonoBehaviour
    {
        public int Index { get; private set; }
        public Peg CurrentPeg { get; set; }
        public Peg TargetPeg { get; private set; }
        public bool Active { get; private set; }
        public bool CanBeSelected { get; private set; }

        private Transform _transform;
        private bool _isSelected;
        private RingAnimator _animator;

        private void Start()
        {
            _transform = transform;
            _animator = GetComponent<RingAnimator>();
            CanBeSelected = true;
        }

        public async void PlaceToNearestPeg()
        {
            CurrentPeg.RemoveRing(this);
            _isSelected = false;

            await PlayPlaceAnimation();

            await TargetPeg.PlaceRing(this);

            CanBeSelected = true;
        }

        public void ReturnToCurrentPeg()
        {
            CurrentPeg.ReturnRing(this);
            _isSelected = false;
        }

        public async void Selected()
        {
            if (!CanBeSelected)
                return;

            _isSelected = true;
            CanBeSelected = false;
            
            await PlaySelectionAnimation();
            
            while (_isSelected)
            {
                FindNearestPeg();
                MoveToMousePosition();
                await Task.Yield();
            }
        }

        public void SetIndex(int index) => Index = index;

        public bool IsToppestRing()
        {
            if (Equals(CurrentPeg.Rings.Last()))
                return true;
            else
                return false;
        }

        public void SetActive() => Active = true;

        private bool Equals(Ring last) => last.Index == Index;

        public async Task PlayPlaceAnimation()
        {
            _animator.HoverPoint = TargetPeg.RingsHoverPoint;
            await _animator.PlayPlaceSequence();
        }

        public async Task PlaySelectionAnimation()
        {
            _animator.HoverPoint = CurrentPeg.RingsHoverPoint;
            await _animator.PlaySelectionSequence();
        }

        public async Task PlayActivationAnimation()
        {
            _animator.HoverPoint = CurrentPeg.GetHoverPoint();
            await _animator.PlayActivationSequence();
        }

        public async Task PlayHoverAnimation(CancellationTokenSource source = null)
        {
            _animator.PlacePoint = CurrentPeg.GetHoverPoint();
            await _animator.PlayHoverSequence(source);
        }

        public async Task PlayTransferAnimation()
        {
            _animator.HoverPoint = CurrentPeg.GetHoverPoint();
            await _animator.PlayTransferSequence();
        }

        public async Task Drop()
        {
            _animator.PlacePoint = CurrentPeg.GetPlacePoint();
            _animator.HoverPoint = CurrentPeg.GetHoverPoint();
            await _animator.PlayDropSequence();
        }

        private void MoveToMousePosition()
        {
            Camera camera = GameplayManager.Instance.GameplayCamera;
            float posZ = camera.WorldToScreenPoint(TargetPeg.transform.position).z;

            Vector3 screenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, posZ);
            Vector3 mousePos = camera.ScreenToWorldPoint(screenPos);

            Vector3 target = new Vector3(mousePos.x, CurrentPeg.RingsHoverPoint.y + 0.1f, mousePos.z);
            _transform.position = Vector3.Lerp(_transform.position, target, 6 * Time.deltaTime);
        }

        private void FindNearestPeg()
        {
            foreach (Peg peg in GameplayManager.Instance.PegsManager.Pegs)
            {
                if (!TargetPeg)
                    TargetPeg = peg;
                else
                {
                    float pegDistance = Vector3.Distance(_transform.position, peg.transform.position);
                    float targetPegDistance = Vector3.Distance(_transform.position, TargetPeg.transform.position);

                    if (pegDistance < targetPegDistance)
                        TargetPeg = peg;
                }
            }
        }
    }
}