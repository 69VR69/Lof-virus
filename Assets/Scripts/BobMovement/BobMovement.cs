using System;
using System.Collections;
using System.Collections.Generic;

using Assets.Scripts.BobMovement;

using UnityEngine;

namespace Assets
{
    public class BobMovement : MonoBehaviour
    {
        [SerializeField]
        private float _speed = 1.0f;
        [SerializeField]
        private float _fallAngle = -85.0f;
        [SerializeField]
        private IkRig _ikRig;
        [SerializeField]
        private List<IkLegMovementPreset> _ikLegMovementPresets;
        [SerializeField]
        private List<IkLegMovementPreset> _ikShoulderMovementPresets;


        private bool _isRunning = false;
        private Vector3 _moveDirection;

        #region Public Methods
        public void SetIdle()
        {
            _ikRig.LegPreset.StepDuration *= _speed;
            _ikRig.ShoulderPreset.StepDuration *= _speed;
        }

        /// <summary>
        /// Set the bob to running animation
        /// </summary>
        /// <param name="getDirection">Give a lambda that will calculate the direction</param>
        public void SetRun(Func<Vector3> getDirection = null)
        {
            _ikRig.LegPreset.StepDuration /= _speed;
            _ikRig.ShoulderPreset.StepDuration /= _speed;

            if (getDirection == null)
                _moveDirection = GetCurrentDirection();
            else
                _moveDirection = getDirection();
        }

        public void SetToTheGround()
        {
            Debug.Log("To the ground");
            transform.Rotate(Vector3.right, _fallAngle);
        }

        public Vector3 GetCurrentDirection() => transform.forward;
        public Vector3 GetCurrentInverseDirection() => -transform.forward;
        #endregion

        #region Private Methods
        private void Awake()
        {
            if (_ikRig == null)
                Debug.LogError("IkRig not assigned");

            if (_ikLegMovementPresets.Count != _ikShoulderMovementPresets.Count)
                Debug.LogError("You should assign the same number of leg/shoulder presets");
        }

        private void Start()
        {
            SetIdle();
        }

        private void Update()
        {
            if (_isRunning)
                MoveInDirection(_moveDirection);
        }

        private void MoveInDirection(Vector3 direction)
        {
            Vector3 force = direction * _speed * Time.deltaTime;
            transform.position += force;
        }
        #endregion

    }
}