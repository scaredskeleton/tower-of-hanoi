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
            float targetHeight = GameplayManager.Instance.PegsManager.BaseThickness
                - GameplayManager.Instance.RingsManager.Thickness
                + _base.localPosition.y;

            RingsSpawnpoint = transform.position + new Vector3(0, targetHeight, 0);

            SetRingsHoverPoint();
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

        public Vector3 GetHoverPoint()
        {
            int ringCount = Rings.Count;
            float ringThickness = GameplayManager.Instance.RingsManager.Thickness + 0.075f;
            float hoverPointY = RingsHoverPoint.y + ringThickness + (ringThickness * 2 * ringCount);
            Vector3 hoverPoint = new Vector3(RingsHoverPoint.x, hoverPointY, RingsHoverPoint.z);

            return hoverPoint;
        }

        public void PlaceRing(Ring ring)
        {
            //ring.transform.position = GetPlacePoint();
            ring.CurrentPeg = this;
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