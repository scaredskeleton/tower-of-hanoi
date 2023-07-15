using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace TowerOfHanoi.Gameplay.PlayerControls
{
    public class GameplayMouseInputs : MonoBehaviour
    {
        private PlayerInputs _input;
        private Camera _camera;
        private bool _hasSelection;
        private Ring _selectedRing;

        private void Awake()
        {
            _input = GameplayManager.Instance.PlayerInputs;
            _camera = GameplayManager.Instance.GameplayCamera;
        }

        private void OnEnable()
        {
            _input.Gameplay.Select.performed += OnSelectPerformed;
            _input.Gameplay.Cancel.performed += OnCancelPerformed;
        }


        private void OnDisable()
        {
            _input.Gameplay.Select.performed -= OnSelectPerformed;
            _input.Gameplay.Cancel.performed -= OnCancelPerformed;
        }

        private void OnSelectPerformed(CallbackContext context) 
        {
            if (!_hasSelection)
                TrySelectingRing();
            else
                TryPlacingRing();
        }

        private void OnCancelPerformed(CallbackContext context) 
        {
            if (!_hasSelection)
                return;

            CancelRingSelection();
        }

        private void TrySelectingRing()
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Ring ring = hit.transform.GetComponent<Ring>();

                if (ring != null && ring.IsToppestRing())
                {
                    _selectedRing = ring;
                    _selectedRing.Selected();
                    _hasSelection = true;
                }
            }
        }

        private void TryPlacingRing()
        {
            if (GameplayManager.Instance.RingsManager.IsLegalMove(_selectedRing))
            {
                _selectedRing.PlaceToNearestPeg();
                _hasSelection = false;
                _selectedRing = null;
            }
        }

        private void CancelRingSelection()
        {
            _selectedRing.ReturnToCurrentPeg();
            _hasSelection = false;
            _selectedRing = null;
        }
    }
}