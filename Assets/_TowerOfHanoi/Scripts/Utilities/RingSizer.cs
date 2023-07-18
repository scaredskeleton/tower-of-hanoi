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

        private void OnValidate()
        {
            var renderer = GetComponent<SkinnedMeshRenderer>();
            UpdateRingSize(renderer);
            UpdateColliderAndBounds(renderer);
        }

        private void UpdateRingSize(SkinnedMeshRenderer renderer)
        {
            float outerRadiusNormalized = 1 - Normalize(_outerRadius, _minOuterRadius, _maxOuterRadius);
            renderer.SetBlendShapeWeight(2, outerRadiusNormalized * 100f);

            float innerRadiusNormalized = Normalize(_innerRadius, _minInnerRadius, _maxInnerRadius);
            renderer.SetBlendShapeWeight(1, innerRadiusNormalized * 100f);

            float thicknessNormalized = 1 - Normalize(_thickness, _minThickness, _maxThickness);
            renderer.SetBlendShapeWeight(0, thicknessNormalized * 100f);
        }

        private void UpdateColliderAndBounds(SkinnedMeshRenderer renderer)
        {
            float lenght = _outerRadius * 2f;
            Vector3 center = new Vector3(0, 0, _thickness / 2f);
            Vector3 size = new Vector3(lenght, lenght, _thickness); ;

            BoxCollider collider = GetComponent<BoxCollider>();
            collider.center = center;
            collider.size = size;

            Bounds bounds = renderer.localBounds;
            bounds.center = center;
            bounds.size = size;
            
            renderer.localBounds = bounds;
        }

        private float Normalize(float value, float min, float max) => (value - min) / (max - min);
    }
}