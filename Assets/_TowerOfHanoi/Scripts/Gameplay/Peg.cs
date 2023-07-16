using System.Collections.Generic;
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
        public List<Ring> Rings { get; private set; } = new List<Ring>();

        public void UpdateRingsSpawnpoint()
        {
            float targetHeight = GameplayManager.Instance.PegsManager.BaseThickness + _base.position.y;

            RingsSpawnpoint = new Vector3(0, targetHeight, 0) + transform.position;
        }

        public Vector3 GetPlacePoint(bool isReturning = false)
        {
            int ringCount = Rings.Count;

            if (isReturning)
                ringCount -= 1;

            float ringThickness = GameplayManager.Instance.RingsManager.Thickness;

            float placePointY = RingsSpawnpoint.y + ringThickness + (ringThickness * 2 * ringCount);
            Vector3 placePoint = new Vector3(RingsSpawnpoint.x, placePointY, RingsSpawnpoint.z);

            return placePoint;
        }

        public void PlaceRing(Ring ring)
        {
            ring.transform.position = GetPlacePoint();
            ring.SetPeg(this);
            Rings.Add(ring);

            if (this == GameplayManager.Instance.PegsManager.GoalPeg || ring == GameplayManager.Instance.RingsManager.LastRing)
                GameplayManager.Instance.CheckIfFinished();
        }

        public void RemoveRing(Ring ring) => Rings.Remove(ring);

        public void ReturnRing(Ring ring) => ring.transform.position = GetPlacePoint(true);

        public void Clear() => Rings.Clear();

        private void SetRingsHoverPoint()
        {
            float targetHeight = GameplayManager.Instance.PegsManager.PegHeight +
                GameplayManager.Instance.PegsManager.HoverPointOffset +
                GameplayManager.Instance.RingsManager.Thickness;

            RingsHoverPoint = new Vector3(0, targetHeight, 0) + _pole.position;
        }
    }
}