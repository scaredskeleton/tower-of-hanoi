using TowerOfHanoi.Gameplay;
using UnityEngine;

namespace TowerOfHanoi.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [SerializeField] GameObject _mainMenuCanvas;
        [SerializeField] GameObject _endScreenCanvas;
         
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(Instance);
        }

        public void StartGame()
        {
            GameplayManager.Instance.Play();
            _mainMenuCanvas.SetActive(false);
        }

        public void GameFinished() => _endScreenCanvas.SetActive(true);

        public void ExitGame() => Application.Quit();
    }
}