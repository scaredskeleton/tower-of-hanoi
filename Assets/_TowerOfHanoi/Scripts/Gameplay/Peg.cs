using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace TowerOfHanoi.Gameplay
{
    public class Peg : MonoBehaviour
    {
        [SerializeField] private Transform _base;
        [SerializeField] private Transform _pole;

        public Transform Base { get => _base; }
        public Transform Pole { get => _pole; }
        public Vector3 RingsStartPoint { get; private set; }
        public Vector3 RingsHoverPoint { get; private set; }
        public List<Ring> Rings { get; private set; } = new List<Ring>();

        public void UpdateRingsStartPoint()
        {
            float targetHeight = GameplayManager.Instance.PegsManager.BaseThickness
                - GameplayManager.Instance.RingsManager.Thickness
                + _base.localPosition.y;

            RingsStartPoint = transform.position + new Vector3(0, targetHeight, 0);

            SetRingsHoverPoint();
        }

        public Vector3 GetPlacePoint(bool isReturning = false, int index = 0)
        {
            int ringCount = Rings.Count;

            if (isReturning)
                ringCount -= 1;
            else if (index > 0)
                ringCount = index;

            float ringThickness = GameplayManager.Instance.RingsManager.Thickness;

            float placePointY = RingsStartPoint.y + ringThickness + (ringThickness * 2 * ringCount);
            Vector3 placePoint = new Vector3(RingsStartPoint.x, placePointY, RingsStartPoint.z);

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

        public async Task PlaceRing(Ring ring)
        {
            AddRing(ring);
            await ring.Drop();

            if (this == GameplayManager.Instance.PegsManager.GoalPeg || ring == GameplayManager.Instance.RingsManager.LastRing)
                GameplayManager.Instance.CheckIfFinished();
        }

        public void AddRing(Ring ring)
        {
            ring.CurrentPeg = this;
            Rings.Add(ring);
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