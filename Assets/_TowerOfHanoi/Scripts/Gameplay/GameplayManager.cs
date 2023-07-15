using UnityEngine;

namespace TowerOfHanoi.Gameplay
{
    public class GameplayManager : MonoBehaviour
    {

        [SerializeField] private Camera _gameplayCamera;

        public static GameplayManager Instance { get; private set; }
        public PegsManager PegsManager { get; private set; }
        public RingsManager RingsManager { get; private set; }
        public PlayerInputs PlayerInputs { get; private set; }
        public Camera GameplayCamera { get => _gameplayCamera; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            PegsManager = GetComponent<PegsManager>();
            RingsManager = GetComponent<RingsManager>();
            PlayerInputs = new PlayerInputs();
        }

        private void OnEnable()
        {
            PlayerInputs.Enable();
        }

        private void OnDisable()
        {
            PlayerInputs.Disable();
        }
    }
}