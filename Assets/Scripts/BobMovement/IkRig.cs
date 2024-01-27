using System.Collections;

using UnityEngine;

namespace Assets.Scripts.BobMovement
{
    [DisallowMultipleComponent]
    public class IkRig : MonoBehaviour
    {
        #region Variable Field
        [SerializeField] private IkLowerBody[] _lowerBodies = null;
        [SerializeField] private IkUpperBody[] _upperBodies = null;
        [SerializeField] private IkLegMovementPreset _legPreset = null;
        [SerializeField] private IkLegMovementPreset _shoulderPreset = null;

        public IkLegMovementPreset LegPreset => _legPreset;
        public IkLegMovementPreset ShoulderPreset => _shoulderPreset;

        private float _rearDelay = 0.75f;

        private IEnumerator _lowerBodyMovement = null;
        private IEnumerator _upperBodyMovement = null;

        #endregion

        private void Awake()
        {
            for (int i = 0; i < _lowerBodies.Length; i++)
                _lowerBodies[i].Init();
            for (int i = 0; i < _upperBodies.Length; i++)
                _upperBodies[i].Init();
        }

        private void Start()
        {
            _lowerBodyMovement = LowerBodyMovement();
            StartCoroutine(_lowerBodyMovement);
            _upperBodyMovement = UpperBodyMovement();
            StartCoroutine(_upperBodyMovement);
        }

        #region Private Field
        private IEnumerator LowerBodyMovement()
        {
            float elapsedTime = 0f;
            int index = 0;

            while (true)
            {
                float delay = _legPreset.StepDuration * _rearDelay;
                if (elapsedTime > delay)
                {
                    _lowerBodies[index++ % _lowerBodies.Length].MoveLegs(_legPreset);
                    elapsedTime = 0;
                }
                elapsedTime += Time.fixedDeltaTime;

                yield return new WaitForFixedUpdate();
            }
        }
        private IEnumerator UpperBodyMovement()
        {
            float elapsedTime = 0f;
            int index = 0;

            while (true)
            {
                float delay = _shoulderPreset.StepDuration * _rearDelay;
                if (elapsedTime > delay)
                {
                    _upperBodies[index++ % _upperBodies.Length].MoveLegs(_shoulderPreset);
                    elapsedTime = 0;
                }
                elapsedTime += Time.fixedDeltaTime;

                yield return new WaitForFixedUpdate();
            }
        }

        #endregion
    }
}