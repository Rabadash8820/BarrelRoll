using UnityEngine;

using Danware.Unity.Input;

using System.Linq;

namespace Rolling {

    public class Loader : MonoBehaviour {
        // EVENT HANDLERS
        private void Awake() {
            LoadInputs();
        }

        // HELPER FUNCTIONS
        private void LoadInputs() {
            RollMover.JumpInput = new StartStopInput("Jump");
            RollMover.RollInput = new ValueInput(
                () => {
                    float btnInput = Input.GetAxis("Horizontal");
                    float mobileInput = AvgAcceleration.x;
                    float rot = (Mathf.Abs(btnInput) > Mathf.Abs(mobileInput) ? btnInput : mobileInput);
                    return -rot;
                },
                () => {
                    float btnInput = Input.GetAxisRaw("Horizontal");
                    float mobileInput = Mathf.Sign(AvgAcceleration.x);
                    float rot = (Mathf.Abs(btnInput) > Mathf.Abs(mobileInput) ? btnInput : mobileInput);
                    return -rot;
                }
            );
        }

        // HELPER FUNCTIONS
        private static Vector3 AvgAcceleration {
            get {
                AccelerationEvent[] events = Input.accelerationEvents;
                Vector3 accel = events.Aggregate(Vector3.zero, (a, e) => a + e.acceleration * e.deltaTime);
                float period = events.Sum(e => e.deltaTime);
                if (period > 0)
                    accel /= period;
                return accel;
            }
        }
    }

}
