using TMPro;
using UnityEngine;

namespace TowerOfHanoi.Gameplay
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _moveCounter;
        [SerializeField] private TextMeshProUGUI _optimalMoveCounter;

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            
        }

        public void UpdateMoveCounter()
        {
            _moveCounter.text = MoveCounter.CurrentMoveCount.ToString();
        }

        public void UpdateOptimalMoveCounter()
        {
            _optimalMoveCounter.text = $"Optimal Move #: {MoveCounter.OptimalMoveCount}";
        }
    }
}