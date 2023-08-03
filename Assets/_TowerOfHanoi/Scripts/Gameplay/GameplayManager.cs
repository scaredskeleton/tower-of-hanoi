using System.Linq;
using TowerOfHanoi.Core;
using TowerOfHanoi.Gameplay.PlayerControls;
using UnityEngine;

namespace TowerOfHanoi.Gameplay
{
    public class GameplayManager : MonoBehaviour
    {
        public static GameplayManager Instance { get; private set; }
        public PegsManager PegsManager { get; private set; }
        public RingsManager RingsManager { get; private set; }
        public HUD HUD { get => _HUD; }
        public PlayerInputs PlayerInputs { get; private set; }
        public Camera GameplayCamera { get => _gameplayCamera; }
        public Ring SelectedRing { get; private set; }
        public bool HasRingSelected { get => SelectedRing != null; }

        [SerializeField] private Camera _gameplayCamera;
        [SerializeField] private HUD _HUD;
        [SerializeField] private GameplayMouseInputs _mouseInputs;

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
            _mouseInputs.enabled = false;
        }

        private void OnEnable()
        {
            PlayerInputs.Enable();
        }

        private void OnDisable()
        {
            PlayerInputs.Disable();
        }

        public async void Play()
        {
            MoveCounter.Reset();
            PegsManager.Initialize();
            RingsManager.Initialize();

            if (GameData.GameLevel == 1)
            {
                await RingsManager.PlayActivationAnimation();
                await RingsManager.DropRings();
            }
            else
            {
                await RingsManager.PlayHoveringAnimation();
                await PegsManager.PlayPegsAnimation();
                PegsManager.UpdatePeg();
                await RingsManager.PlayActivationAnimation();
                RingsManager.FinishHover();
                RingsManager.DisposeHoverTokens();
                await RingsManager.PlayTransferAnimation();
                await RingsManager.DropRings();
            }

            HUD.gameObject.SetActive(true);
            HUD.Initialize();

            _mouseInputs.enabled = true;
        }

        public bool IsLegalMove()
        {
            if (SelectedRing.TargetPeg.Rings.Count == 0)
                return true;
            else if (SelectedRing.TargetPeg.Rings.LastOrDefault() == null)
                return false;
            else if (SelectedRing.Index > SelectedRing.TargetPeg.Rings.LastOrDefault().Index)
                return true;
            else
                return false;
        }

        public bool IsReturningMove()
        {
            if (SelectedRing.Index == SelectedRing.TargetPeg.Rings.LastOrDefault().Index)
                return true;
            else
                return false;
        }

        public void TrySelectingRing()
        {
            Ray ray = GameplayCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Ring ring = hit.transform.GetComponent<Ring>();

                if (ring != null && ring.IsToppestRing() && ring.CanBeSelected)
                {
                    SelectedRing = ring;
                    SelectedRing.Selected();
                }
            }
        }

        public void TryPlacingRing()
        {
            if (IsLegalMove())
            {
                MoveCounter.CurrentMoveCount++;
                HUD.UpdateMoveCounter();
             
                PlaceRing();
            }
            else if (IsReturningMove())
                PlaceRing();

            void PlaceRing()
            {
                SelectedRing.PlaceToNearestPeg();
                SelectedRing = null;
            }
        }

        public void CancelRingSelection()
        {
            SelectedRing.ReturnToCurrentPeg();
            SelectedRing = null;
        }

        public void CheckIfFinished()
        {
            if (PegsManager.EvaluateGoalPeg())
                LevelFinished();
        }

        public void LevelFinished()
        {
            if (GameData.GameLevel == GameData.MaxGameLevel)
                GameManager.Instance.GameFinished();
            else
            {
                _mouseInputs.enabled = false;
                NextLevel();
            }
        }

        public void NextLevel()
        {
            GameData.GameLevel++;
            RingsManager.UpdateRingsCount();
            Play();
        }
    }
}