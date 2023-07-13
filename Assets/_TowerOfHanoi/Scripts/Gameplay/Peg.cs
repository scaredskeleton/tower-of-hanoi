using UnityEngine;

namespace TowerOfHanoi.Gameplay
{
    public class Peg : MonoBehaviour
    {
        [SerializeField] private Transform _base;
        [SerializeField] private Transform _pole;
        
        public Transform Base { get => _base; }
        public Transform Pole { get => _pole; }
    }
}