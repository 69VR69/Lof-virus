using System.Collections;

using Assets.Scripts.BobMovement;

using UnityEngine;

namespace Assets
{
    public class BobMovement : MonoBehaviour
    {
        [SerializeField]
        private float _speed = 1.0f;
        [SerializeField]
        private IkRig _ikRig;

        private void Awake()
        {
            if (_ikRig == null)
                Debug.LogError("IkRig not assigned");
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.K))
                MoveInDirection(transform.forward);

            if (Input.GetKeyDown(KeyCode.K))
            {
                _ikRig.LegPreset.StepDuration /= _speed;
                _ikRig.ShoulderPreset.StepDuration /= _speed;
            }
            else if (Input.GetKeyUp(KeyCode.K))
            {
                _ikRig.LegPreset.StepDuration *= _speed;
                _ikRig.ShoulderPreset.StepDuration *= _speed;
            }
        }

        private void MoveInDirection(Vector3 direction)
        {
            Vector3 force = direction * _speed * Time.deltaTime;
            transform.position += force;
        }

    }
}