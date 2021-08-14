using UnityEngine;

using UnityEngine.Inputs;

namespace BarrelRoll
{
    /// <summary>
    /// Uses either horizontal axis keys or device tilt on mobile for rolling
    /// </summary>
    [CreateAssetMenu(fileName = "barrel-rolling-input", menuName = nameof(BarrelRoll) + "/" + nameof(BarrelRollingInput))]
    public class BarrelRollingInput : ValueInput {
        public override float Value()
        {
            float btnInput = Input.GetAxis("Horizontal");
            float mobileInput = avgAcceleration().x;
            float rot = Mathf.Abs(btnInput) > Mathf.Abs(mobileInput) ? btnInput : mobileInput;
            return -rot;
        }

        public override float DiscreteValue()
        {
            float btnInput = Input.GetAxisRaw("Horizontal");
            float mobileInput = Mathf.Sign(avgAcceleration().x);
            float rot = Mathf.Abs(btnInput) > Mathf.Abs(mobileInput) ? btnInput : mobileInput;
            return -rot;
        }

        private static Vector3 avgAcceleration()
        {
            float period = 0f;
            Vector3 accel = Vector3.zero;
            for (int e = 0; e < Input.accelerationEvents.Length; ++e) {
                AccelerationEvent accelEvent = Input.accelerationEvents[e];
                accel += accelEvent.acceleration * accelEvent.deltaTime;
                period += accelEvent.deltaTime;
            }

            return period > 0 ? accel / period : accel;
        }
    }

}
