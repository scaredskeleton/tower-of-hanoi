using UnityEngine;

namespace TowerOfHanoi.Gameplay
{
    public static class MoveCounter
    {
        public static int CurrentMoveCount;

        public static int OptimalMoveCount { get => (int)Mathf.Pow(2, GameplayManager.Instance.RingsManager.Count) - 1; }
    } 
}