using System.Linq;
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


        private Transform _transform;
        private bool _isSelected;
        private RingAnimator _animator;

        private void Start()
        {
            _transform = transform;
            _animator = GetComponent<RingAnimator>();
        }

        private void Update()
        {
            if (_isSelected)
            {
                MoveToMousePosition();
                FindNearestPeg();
            }
        }

        public void PlaceToNearestPeg()
        {
            CurrentPeg.RemoveRing(this);
            TargetPeg.PlaceRing(this);
            _isSelected = false;
        }

        public void ReturnToCurrentPeg()
        {
            CurrentPeg.ReturnRing(this);
            _isSelected = false;
        }

        public void Selected() => _isSelected = true;

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
            _animator.TargetPoint = CurrentPeg.GetHoverPoint();
            await _animator.PlayActivationAnimation();
        }

        private void MoveToMousePosition()
        {
            Camera camera = GameplayManager.Instance.GameplayCamera;
            float posZ = camera.WorldToScreenPoint(_transform.position).z;

            Vector3 screenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, posZ);
            Vector3 mousePos = camera.ScreenToWorldPoint(screenPos);

            _transform.position = mousePos;
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