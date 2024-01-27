using System;
using System.Collections;

using UnityEngine;

namespace Assets.Scripts.BobMovement
{
    public class IkUpperBody : MonoBehaviour
    {

        #region Variable Field
        [SerializeField] private Transform _body = null;
        [SerializeField] private IkShoulder _leftShoulder = null;
        [SerializeField] private IkShoulder _rightShoulder = null;
        [SerializeField] private DominentLeg _dominentLeg = DominentLeg.Left;

        private IkShoulder _primaryShoulder = null;
        private IkShoulder _secondaryShoulder = null;
        private bool _legMoving = false;
        private float _leftRightDelay = 0.5f;

        private IEnumerator _legMovement = null;

        #endregion

        #region Public Functions
        public void Init()
        {
            _leftShoulder.Init();
            _rightShoulder.Init();

            if (_dominentLeg == DominentLeg.Left)
            {
                _primaryShoulder = _leftShoulder;
                _secondaryShoulder = _rightShoulder;
            }
            else
            {
                _primaryShoulder = _rightShoulder;
                _secondaryShoulder = _leftShoulder;
            }
        }

        public void MoveLegs(IkLegMovementPreset preset)
        {
            if (_legMoving)
                return;
            _legMoving = true;

            _legMovement = LegMovement(preset, () =>
            {
                _legMoving = false;
            });
            StartCoroutine(_legMovement);
        }

        #endregion

        #region Private Functions
        private IEnumerator LegMovement(IkLegMovementPreset preset, Action done)
        {
            _primaryShoulder.MoveLeg(_body, preset);

            float delay = preset.StepDuration * _leftRightDelay;
            yield return new WaitForSeconds(delay);

            _secondaryShoulder.MoveLeg(_body, preset);

            done?.Invoke();
        }

        #endregion
    }
}