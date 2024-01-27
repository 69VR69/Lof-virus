using System.Collections;

using UnityEngine;

namespace Assets.Scripts.BobMovement
{
    [CreateAssetMenu(fileName = "IkLegMovement", menuName = "BobRagdoll/IkLegMovement")]
    public class IkLegMovementPreset : ScriptableObject
    {
        public float StepLength = 0.3f;
        public float StepThreshold = 0.6f;
        public float StepHeight = 0.3f;
        public float StepDuration = 1f;
        public AnimationCurve StepSpeedCurve = null;
    }
}