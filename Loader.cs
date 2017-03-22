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
            RollMover.RollInput.ValueCallback = () => {
                float btnInput = Input.GetAxis("Horizontal");
                float mobileInput = avgAcceleration.x;
                float rot = (Mathf.Abs(btnInput) > Mathf.Abs(mobileInput) ? btnInput : mobileInput);
                return -rot;
            };
            RollMover.RollInput.DiscreteValueCallback = () => {
                float btnInput = Input.GetAxisRaw("Horizontal");
                float mobileInput = Mathf.Sign(avgAcceleration.x);
                float rot = (Mathf.Abs(btnInput) > Mathf.Abs(mobileInput) ? btnInput : mobileInput);
                return -rot;
            };
        }
        private static Vector3 avgAcceleration {
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
