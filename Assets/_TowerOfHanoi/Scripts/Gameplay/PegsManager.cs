using System.Collections.Generic;
using UnityEngine;

namespace TowerOfHanoi.Gameplay
{
    public class PegsManager : MonoBehaviour
    {
        [SerializeField] private float _pegRadius;
        [SerializeField] private float _minBaseRadius;
        [SerializeField] private float _baseThickness;
        [SerializeField] private float _hoverPointOffset;
        [SerializeField] private List<Peg> _pegs;

        public float MinBaseRadius { get => _minBaseRadius; }
        public float BaseThickness { get => _baseThickness; }
        public float PegHeight { get; private set; }
        public float HoverPointOffset { get => _hoverPointOffset; }
        public List<Peg> Pegs { get => _pegs; }

        private void Start()
        {
            UpdatePegsProportions();
        }

        private void UpdatePegsProportions()
        {
            foreach (var peg in _pegs)
            {
                peg.Base.localScale = new Vector3(_minBaseRadius, _baseThickness, _minBaseRadius);
                peg.Base.localPosition = new Vector3(0, _baseThickness, 0);

                PegHeight = GameplayManager.Instance.RingsManager.Count * GameplayManager.Instance.RingsManager.Thickness + 0.2f;
                peg.Pole.localScale = new Vector3(_pegRadius, PegHeight, _pegRadius);

                float baseOffset = (_baseThickness * 2) + PegHeight;
                peg.Pole.localPosition = new Vector3(0, baseOffset, 0);
            }
        }
    }
}