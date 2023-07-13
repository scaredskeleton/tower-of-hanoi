using UnityEngine;

namespace TowerOfHanoi.Gameplay
{
    public class GameplayManager : MonoBehaviour
    {
        public static GameplayManager Instance { get; private set; }
        public PegsManager PegsManager { get; private set; }
        public RingsManager RingsManager { get; private set; }

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
        }
    }
}