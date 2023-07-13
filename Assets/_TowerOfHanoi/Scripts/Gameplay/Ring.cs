using UnityEngine;

namespace TowerOfHanoi.Gameplay
{
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshFilter))]
    public class Ring : MonoBehaviour
    {
        public MeshFilter MeshFilter { get; private set; }
        private void Start()
        {
            MeshFilter = GetComponent<MeshFilter>();
        }
    }
}