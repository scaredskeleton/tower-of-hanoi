using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TowerOfHanoi.PlayerControls
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float _orbitSpeed;
        [SerializeField] private float _panSpeed;
        public Transform LookTarget;

        private PlayerInputs _input;
        private Transform _transform;
        private Vector2 _orbitVector;
        private Vector2 _panVector;

        private void Awake()
        {
            _input = new PlayerInputs();
            _transform = transform;
        }

        private void OnEnable()
        {
            _input.Enable();
            _input.Camera.Orbit.performed += OnOrbitPerformed;
            _input.Camera.Orbit.canceled += OnOrbitCanceled;
            _input.Camera.Pan.performed += OnPanPerformed;
            _input.Camera.Pan.canceled += OnPanCanceled;
        }

        private void OnDisable()
        {
            _input.Disable();
            _input.Camera.Orbit.performed -= OnOrbitPerformed;
            _input.Camera.Orbit.canceled -= OnOrbitCanceled;
            _input.Camera.Pan.performed -= OnPanPerformed;
            _input.Camera.Pan.canceled -= OnPanCanceled;
        }

        private void Update()
        {
            if (LookTarget != null)
                _transform.LookAt(LookTarget.position);

            _transform.Translate(_orbitVector * _orbitSpeed * Time.deltaTime);

            LookTarget.position += (Vector3)_panVector * _panSpeed * Time.deltaTime;
            _transform.position += (Vector3)_panVector * _panSpeed * Time.deltaTime;
        }

        private void OnOrbitPerformed(InputAction.CallbackContext context) => _orbitVector = context.ReadValue<Vector2>();

        private void OnOrbitCanceled(InputAction.CallbackContext context) => _orbitVector = Vector2.zero;

        private void OnPanPerformed(InputAction.CallbackContext context) => _panVector = context.ReadValue<Vector2>();

        private void OnPanCanceled(InputAction.CallbackContext context) => _panVector = Vector2.zero;

    }
}