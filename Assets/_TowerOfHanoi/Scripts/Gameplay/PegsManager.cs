using System.Collections.Generic;
using UnityEngine;

namespace TowerOfHanoi.Gameplay
{
    public class PegsManager : MonoBehaviour
    {
        [SerializeField] private float _pegHeight;
        [SerializeField] private float _minBaseRadius;
        [SerializeField] private float _baseThickness;
        [SerializeField] private List<Peg> _pegs;

        public float MinBaseRadius { get => _minBaseRadius; }
        public float BaseThickness { get => _baseThickness; }
        public float PegHeight { get => _pegHeight; }

        private void Start()
        {
            UpdatePegsProportions();
        }

        private void UpdatePegsProportions()
        {
            float thicknessOffset = _baseThickness / 2;
            foreach (var peg in _pegs)
            {
                peg.Base.localScale = new Vector3(_minBaseRadius, _baseThickness, _minBaseRadius);
                peg.Base.localPosition = new Vector3(0, thicknessOffset, 0);

                float baseOffset = thicknessOffset + _baseThickness + _pegHeight;
                peg.Pole.localPosition = new Vector3(0, baseOffset, 0);
            }
        }
    }
}