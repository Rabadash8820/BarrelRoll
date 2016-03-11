using UnityEngine;
using System.Linq;

namespace Rolling {

    class RollingInput {
        // API INTERFACE
        public static float RotateCW {
            get {
                float btnInput = Input.GetAxis("Horizontal");
                float mobileInput = AvgAcceleration.x;
                float rot = (Mathf.Abs(btnInput) > Mathf.Abs(mobileInput) ? btnInput : mobileInput);
                return -rot;
            }
        }
        public static bool JumpStart {
            get {
                bool btnInput = Input.GetButtonDown("Jump");
                return btnInput;
            }
        }
        public static bool JumpStay {
            get {
                bool btnInput = Input.GetButton("Jump");
                return btnInput;
            }
        }
        public static bool JumpStop {
            get {
                bool btnInput = Input.GetButtonUp("Jump");
                return btnInput;
            }
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
