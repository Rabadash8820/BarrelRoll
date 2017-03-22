using UnityEngine;

using Danware.Unity.Input;

using System.Linq;

namespace Rolling {

    public class Loader : MonoBehaviour {

        // INSPECTOR INTERFACE
        public RollMover RollMover;

        // EVENT HANDLERS
        private void Awake() {
            LoadInputs();
        }

        // HELPER FUNCTIONS
        private void LoadInputs() {
            // Set rolling inputs (horizontal axis keys or device tilt on mobile)
            ValueInput roll = RollMover?.RollInput;
            if (roll != null) {
                roll.ValueCallback = () => {
                    float btnInput = Input.GetAxis("Horizontal");
                    float mobileInput = avgAcceleration().x;
                    float rot = (Mathf.Abs(btnInput) > Mathf.Abs(mobileInput) ? btnInput : mobileInput);
                    return -rot;
                };
                roll.DiscreteValueCallback = () => {
                    float btnInput = Input.GetAxisRaw("Horizontal");
                    float mobileInput = Mathf.Sign(avgAcceleration().x);
                    float rot = (Mathf.Abs(btnInput) > Mathf.Abs(mobileInput) ? btnInput : mobileInput);
                    return -rot;
                };
            }

            // Set jump input (jump key or single tap on mobile)
            StartStopInput jump = RollMover?.JumpInput;
            if (jump != null) {
                jump.StartedCallback = () => {
                    bool btnInput = Input.GetButtonDown("Jump");
                    bool mobileInput = false;
                    for (int t=0; t< Input.touchCount; ++t) {
                        if (Input.GetTouch(t).phase == TouchPhase.Began) {
                            mobileInput = true;
                            break;
                        }
                    }
                    return btnInput || mobileInput;
                };
            }
        }
        private static Vector3 avgAcceleration() {
            AccelerationEvent[] events = Input.accelerationEvents;
            Vector3 accel = events.Aggregate(Vector3.zero, (a, e) => a + e.acceleration * e.deltaTime);
            float period = events.Sum(e => e.deltaTime);
            if (period > 0)
                accel /= period;
            return accel;
        }
    }

}
