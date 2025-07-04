using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace TowerOfHanoi.Gameplay
{
    public class PegsManager : MonoBehaviour
    {
        public float BaseThickness { get => _baseThickness; }
        public float PegHeight { get; private set; }
        public float HoverPointOffset { get => _hoverPointOffset; }
        public List<Peg> Pegs { get => _pegs; }
        public Peg StartingPeg { get => Pegs[0]; }
        public Peg GoalPeg { get => Pegs.Last(); }

        [SerializeField] private float _pegRadius;
        [SerializeField] private float _baseThickness;
        [SerializeField] private float _baseRadiusOffset;
        [SerializeField] private float _hoverPointOffset;
        [SerializeField] private float _pegsDistanceOffset;
        [SerializeField] private List<Peg> _pegs;

        public void Initialize()
        {
            UpdatePegsProportions();
            ClearPegs();
        }

        public void UpdatePegsProportions()
        {
            int ringCount = GameplayManager.Instance.RingsManager.Count;
            float minRingRadius = GameplayManager.Instance.RingsManager.MinRadius;
            float radiusIncrement = GameplayManager.Instance.RingsManager.RadiusIncrement;

            float baseRadius = minRingRadius + (radiusIncrement * ringCount) + _baseRadiusOffset;
            Vector3 baseScale = new Vector3(baseRadius, _baseThickness, baseRadius);
            Vector3 basePosition = new Vector3(0, _baseThickness, 0);
            
            PegHeight = ringCount * GameplayManager.Instance.RingsManager.Thickness + 0.2f;
            Vector3 poleScale = new Vector3(_pegRadius, PegHeight, _pegRadius);
            float baseOffset = (_baseThickness * 2) + PegHeight;
            Vector3 polePosition = new Vector3(0, baseOffset, 0);

            float distanceBetweenPegs = (baseRadius + _pegsDistanceOffset);
            float leftmostPositionX = transform.position.x - distanceBetweenPegs;
            Vector3 leftmostPosition = transform.position + new Vector3(leftmostPositionX, 0, 0);

            for (int i = 0; i < _pegs.Count; i++)
            {
                Peg peg = _pegs[i];
                //peg.Base.localScale = baseScale;
                //peg.Base.localPosition = basePosition;

                //peg.Pole.localScale = poleScale;
                //peg.Pole.localPosition = polePosition;

                //Vector3 pegPosition = leftmostPosition + new Vector3(distanceBetweenPegs * i, 0, 0);
                //peg.transform.localPosition = pegPosition;

                peg.Animator.BaseSize = baseScale;
                peg.Animator.BaseLocalPosition = basePosition;

                peg.Animator.PoleSize = poleScale;
                peg.Animator.PoleLocalPosition = polePosition;

                Vector3 pegPosition = leftmostPosition + new Vector3(distanceBetweenPegs * i, 0, 0);
                peg.Animator.TargetPostion = pegPosition;

                peg.UpdateRingsStartPoint();
            }
        }

        public async Task PlayPegsAnimation()
        {
            Task[] tasks = new Task[_pegs.Count];
            for (int i = 0; i < _pegs.Count;i++)
            {
                tasks[i] = _pegs[i].Animator.PlayTransformationSequence();
            }

            await Task.WhenAll(tasks);
        }

        public void UpdatePeg()
        {
            foreach (Peg peg in _pegs)
                peg.UpdateRingsStartPoint();
        }

        public bool EvaluateGoalPeg()
        {
            if (GoalPeg.Rings.Count == GameplayManager.Instance.RingsManager.Count)
                return true;
            else
                return false;
        }

        private void ClearPegs()
        {
            foreach (Peg peg in _pegs)
            {
                peg.Clear();
            }
        }
    }
}