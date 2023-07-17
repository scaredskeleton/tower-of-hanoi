using System.Linq;
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
        public Peg CurrentPeg { get; private set; }
        public Peg TargetPeg { get; private set; }


        private Transform _transform;
        private bool _isSelected;

        private void Start()
        {
            _transform = transform;
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

        public void SetPeg(Peg peg) => CurrentPeg = peg;

        private bool Equals(Ring last) => last.Index == Index;

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