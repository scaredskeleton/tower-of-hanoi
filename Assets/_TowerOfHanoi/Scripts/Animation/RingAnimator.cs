using Cinemachine;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace TowerOfHanoi.Animation
{
    public class RingAnimator : MonoBehaviour
    {
        public Vector3 HoverPoint { get; set; }
        public Vector3 PlacePoint { get; set; }

        [SerializeField] private float _lastWaypointOffset;
        [SerializeField] private CinemachinePath _activationPath;
        [SerializeField] private float _minHoverSpeed;
        [SerializeField] private float _maxHoverSpeed;
        [SerializeField] private float _deltaHeight;
        [SerializeField] private float _spinSpeed;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _dropSpeed;

        private CinemachineDollyCart _dollyCart;
        private Transform _transform;

        private void Start()
        {
            _dollyCart = GetComponent<CinemachineDollyCart>();
            _dollyCart.enabled = false;
            _transform = transform;
        }

        public async Task PlayActivationSequence()
        {
            AddTargetWaypoints();
            await FloatUpAnimation(0.8f, 0.5f);
            await HoverAnimation(3f);
            await UpdatePath();
            await HoverAnimation(0.2f);
        }

        public async Task PlayHoverSequence(CancellationTokenSource source = null)
        {
            await FloatUpAnimation(1f, HoverPoint.y - _transform.position.y);
            await HoverAnimation(50f, source);
        }

        public async Task PlayTransferSequence()
        {
            await MoveToStartingHoverAnimation();
        }

        public async Task PlayDropSequence()
        {
            float t = 0;
            while (_transform.position.y > PlacePoint.y)
            {
                t += _moveSpeed * Time.deltaTime;
                _transform.position = Vector3.Lerp(_transform.position, PlacePoint, t);
                await Task.Yield();
            }
        }

        private async Task UpdatePath()
        {
            _dollyCart.enabled = true;
            while (_dollyCart.m_Position < 1f)
            {
                _dollyCart.m_Position += _moveSpeed * Time.deltaTime;
                await Task.Yield();
            }
            _dollyCart.enabled = false;
        }

        private async Task HoverAnimation(float duration = 0, CancellationTokenSource source = null)
        {
            float hoverSpeed = Random.Range(_minHoverSpeed, _maxHoverSpeed);
            while (duration > 0)
            {
                if (source != null && source.Token.IsCancellationRequested)
                    return;

                Oscillate(hoverSpeed);
                duration -= Time.deltaTime;
                await Task.Yield();
            }
        }

        private async Task FloatUpAnimation(float duration, float height)
        {
            float hoverSpeed = Random.Range(_minHoverSpeed, _maxHoverSpeed);
            float t = duration;
            Vector3 targetPosition = _transform.position + new Vector3(0f, height, 0f);
            while (duration > 0)
            {
                _transform.position = Vector3.Lerp(_transform.position, targetPosition, (t - duration) / t);
                Oscillate(hoverSpeed);
                duration -= Time.deltaTime;
                await Task.Yield();
            }
        }

        private async Task MoveToStartingHoverAnimation()
        {
            float t = 0;
            while (_transform.position.x > HoverPoint.x)
            {
                t += _moveSpeed * Time.deltaTime;
                _transform.position = Vector3.Lerp(_transform.position, HoverPoint, t);
                await Task.Yield();
            }
        }

        private void Oscillate(float hoverSpeed)
        {
            float oscillationY = Mathf.Sin(Time.time * hoverSpeed);
            _transform.position += new Vector3(0, oscillationY, 0) * _deltaHeight;
        }

        private void AddTargetWaypoints()
        {
            int lenght = _activationPath.m_Waypoints.Length - 1;
            _activationPath.m_Waypoints[lenght - 1].position = HoverPoint + new Vector3(0f, 0.6f, 0.0f);
            _activationPath.m_Waypoints[lenght].position = HoverPoint;
            _dollyCart.m_Path = _activationPath;
        }
    }
}