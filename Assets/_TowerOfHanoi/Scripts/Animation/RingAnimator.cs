using Cinemachine;
using System.Threading.Tasks;
using UnityEngine;

namespace TowerOfHanoi.Animation
{
    public class RingAnimator : MonoBehaviour
    {
        public Vector3 TargetPoint { get; set; }

        [SerializeField] private float _lastWaypointOffset;
        [SerializeField] private CinemachinePath _activationPath;
        [SerializeField] private float _hoverSpeed;
        [SerializeField] private float _deltaHeight;
        [SerializeField] private float _spinSpeed;

        private CinemachineDollyCart _dollyCart;
        private Transform _transform;

        private void Start()
        {
            _dollyCart = GetComponent<CinemachineDollyCart>();
            _dollyCart.enabled = false;
            _transform = transform;
        }

        public async Task PlayActivationAnimation()
        {
            AddTargetWaypoints();
            await FloatUpAnimation(1f, 0.5f);
            await HoverAnimation(3f);
            await UpdatePath();
            await HoverAnimation(5f);
        }

        public async Task UpdatePath()
        {
            _dollyCart.enabled = true;
            while (_dollyCart.m_Position < 1f)
            {
                _dollyCart.m_Position += 0.8f * Time.deltaTime;
                await Task.Yield();
            }
            _dollyCart.enabled = false;
        }

        private async Task HoverAnimation(float duration)
        {
            while (duration > 0)
            {
                Oscillate();
                duration -= Time.deltaTime;
                await Task.Yield();
            }
        }

        private async Task FloatUpAnimation(float duration, float height)
        {
            float t = duration;
            Vector3 targetPosition = _transform.position + new Vector3(0f, height, 0f);
            while (duration > 0)
            {
                _transform.position = Vector3.Lerp(_transform.position, targetPosition, t - duration / t);
                Oscillate();
                duration -= Time.deltaTime;
                await Task.Yield();
            }
        }

        private void Oscillate()
        {
            float oscillationY = Mathf.Sin(Time.time * _hoverSpeed);
            _transform.position += new Vector3(0, oscillationY, 0) * _deltaHeight;
        }

        private void AddTargetWaypoints()
        {
            int lenght = _activationPath.m_Waypoints.Length - 1;
            _activationPath.m_Waypoints[lenght - 1].position = TargetPoint + new Vector3(0f, 0.7f, 0.0f);
            _activationPath.m_Waypoints[lenght].position = TargetPoint;
            _dollyCart.m_Path = _activationPath;
        }
    }
}