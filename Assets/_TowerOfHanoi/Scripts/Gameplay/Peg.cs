using UnityEngine;

namespace TowerOfHanoi.Gameplay
{
    public class Peg : MonoBehaviour
    {
        [SerializeField] private Transform _base;
        [SerializeField] private Transform _pole;

        public Transform Base { get => _base; }
        public Transform Pole { get => _pole; }
        public Vector3 RingsSpawnpoint { get; private set; }
        public Vector3 RingsHoverPoint { get; private set; }

        private void Start() => SetRingsSpawnpoint();

        private void SetRingsSpawnpoint()
        {
            float targetHeight = GameplayManager.Instance.PegsManager.BaseThickness + _base.position.y;

            RingsSpawnpoint = new Vector3(0, targetHeight, 0) + transform.position;
        }

        private void SetRingsHoverPoint()
        {
            float targetHeight = GameplayManager.Instance.PegsManager.PegHeight +
                GameplayManager.Instance.PegsManager.HoverPointOffset +
                GameplayManager.Instance.RingsManager.Thickness;

            RingsHoverPoint = new Vector3(0, targetHeight, 0) + _pole.position;
        }
    }
}