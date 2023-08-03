using System.Threading.Tasks;
using UnityEngine;

namespace TowerOfHanoi.Animation
{
    public class PegAnimator : MonoBehaviour
    {
        public Transform Base { get; set; }
        public Transform Pole { get; set; }
        public Vector3 TargetPostion { get; set; }
        public Vector3 PoleSize { get; set; }
        public Vector3 PoleLocalPosition { get; set; }
        public Vector3 BaseSize { get; set; }
        public Vector3 BaseLocalPosition { get; set; }

        private Transform _transform;

        private void Start() => _transform = transform;

        public async Task PlayTransformationSequence()
        {
            await MoveToAnimation(0.5f, _transform.localPosition + new Vector3(0f, 0.2f, 0f));
            await ResizeAnimation(0.25f, Base, BaseSize, BaseLocalPosition);
            await ResizeAnimation(0.25f, Pole, PoleSize, PoleLocalPosition);
            await MoveToAnimation(0.5f, TargetPostion);
        }

        private async Task FloatUpAnimation(float duration, float height)
        {
            float t = duration;
            while(duration > 0)
            {
                float y = Mathf.Lerp(_transform.localPosition.y, _transform.localPosition.y + height, (t - duration) / t);
                _transform.position += new Vector3(0f, y, 0f);
                duration -= Time.deltaTime;
                await Task.Yield();
            }
        }

        private async Task ResizeAnimation(float duration, Transform target, Vector3 targetSize, Vector3 targetPostion)
        {
            float t = duration;
            while (duration > 0)
            {
                float alpha = (t - duration) / t;
                target.localScale = Vector3.Lerp(target.localScale, targetSize, alpha);
                target.localPosition = Vector3.Lerp(target.localPosition, targetPostion, alpha);
                duration -= Time.deltaTime;
                await Task.Yield();
            }
        }

        private async Task MoveToAnimation(float duration, Vector3 targetPostion)
        {
            float t = duration;
            while (duration > 0)
            {
                _transform.localPosition = Vector3.Lerp(_transform.localPosition, targetPostion, (t - duration) / t);
                duration -= Time.deltaTime;
                await Task.Yield();
            }
        }
    }
}
