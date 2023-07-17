using UnityEngine;

namespace TowerOfHanoi.Utilities
{
    public class RingSizer : MonoBehaviour
    {
        [SerializeField, Range(0.24f, 3f)] private float _outerRadius;
        [SerializeField, Range(0.05f, 0.2f)] private float _innerRadius;
        [SerializeField, Range(0.1f, 1f)] private float _thickness;

        private float _minOuterRadius = 0.24f;
        private float _maxOuterRadius = 3f;
        private float _minInnerRadius = 0.05f;
        private float _maxInnerRadius = 0.2f;
        private float _minThickness = 0f;
        private float _maxThickness = 1f;

        private SkinnedMeshRenderer _renderer { get => GetComponent<SkinnedMeshRenderer>(); }
        private BoxCollider _collider { get => GetComponent<BoxCollider>(); }

        private void OnValidate()
        {
            UpdateRingSize();
            UpdateColliderSizeAndCenter();
        }

        private void UpdateRingSize()
        {
            float outerRadiusNormalized = 1 - Normalize(_outerRadius, _minOuterRadius, _maxOuterRadius);
            _renderer.SetBlendShapeWeight(2, outerRadiusNormalized * 100f);

            float innerRadiusNormalized = Normalize(_innerRadius, _minInnerRadius, _maxInnerRadius);
            Debug.Log(innerRadiusNormalized);
            _renderer.SetBlendShapeWeight(1, innerRadiusNormalized * 100f);

            float thicknessNormalized = 1 - Normalize(_thickness, _minThickness, _maxThickness);
            _renderer.SetBlendShapeWeight(0, thicknessNormalized * 100f);
        }

        private void UpdateColliderSizeAndCenter()
        {
            _collider.center = new Vector3(0, 0, _thickness / 2f);
        }

        private float Normalize(float value, float min, float max) => (value - min) / (max - min);
    }
}