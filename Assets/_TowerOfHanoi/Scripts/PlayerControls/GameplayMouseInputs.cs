using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace TowerOfHanoi.Gameplay.PlayerControls
{
    public class GameplayMouseInputs : MonoBehaviour
    {
        private PlayerInputs _input;

        private void Awake()
        {
            _input = GameplayManager.Instance.PlayerInputs;
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
            if (!GameplayManager.Instance.HasRingSelected)
                GameplayManager.Instance.TrySelectingRing();
            else
                GameplayManager.Instance.TryPlacingRing();
        }

        private void OnCancelPerformed(CallbackContext context) 
        {
            if (!GameplayManager.Instance.HasRingSelected)
                return;

            GameplayManager.Instance.CancelRingSelection();
        }
    }
}