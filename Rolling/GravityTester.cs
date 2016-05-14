using UnityEngine;
using UnityEngine.UI;

using Danware.Unity.Input;

namespace Rolling {

    public class GravityTester : MonoBehaviour {
        // HIDDEN FIELDS
        private StartStopInputArray Input = new StartStopInputArray(
            KeyCode.Keypad4,    // 0 - left
            KeyCode.Keypad7,    // 1 - leftup
            KeyCode.Keypad8,    // 2 - up
            KeyCode.Keypad9,    // 3 - upright
            KeyCode.Keypad6,    // 4 - right
            KeyCode.Keypad3,    // 5 - rightdown
            KeyCode.Keypad2,    // 6 - down
            KeyCode.Keypad1,    // 7 - downleft
            KeyCode.Keypad5     // 8 - zero-G
        );   

        // INSPECTOR FIELDS
        public WorldRotater MainCameraRotater;
        public float Magnitude = 9.81f;
        public GravityShifter Mediator;

        // EVENT HANDLERS
        private void Update() {
            // Get player input
            bool gravityChanged = Input.AnyStarted;

            // If a direction button was pressed then adjust gravity
            if (gravityChanged) {
                if (Mediator != null)
                    Mediator.SetGravity(newGravity());
            }
        }

        // HELPERS
        private Vector2 newGravity() {
            // Return a zero-vector if gravity has been turned off
            bool gravityOff = (Input.Started[8]);
            if (gravityOff)
                return Vector2.zero;

            // Otherwise, if the MainCamera is still rotating, then just return the current gravity vector;
            if (MainCameraRotater.IsRotating)
                return Physics2D.gravity;
            bool[] key = Input.Started;
            float diag = Mathf.Sqrt(2f) / 2f;

            // Get the x-component
            float gx = (key[0] ? -1    : 0) +
                       (key[1] ? -diag : 0) +
                       (key[3] ?  diag : 0) +
                       (key[4] ?  1    : 0) +
                       (key[5] ?  diag : 0) +
                       (key[7] ? -diag : 0);

            // Get the y-component
            float gy = (key[1] ?  diag : 0) +
                       (key[2] ?  1    : 0) +
                       (key[3] ?  diag : 0) +
                       (key[5] ? -diag : 0) +
                       (key[6] ? -1    : 0) +
                       (key[7] ? -diag : 0);

            // Return the unit vector with these components, in camera space
            Vector2 newGravity = new Vector2(gx, gy);
            Transform trans = MainCameraRotater.MainCamera.transform;
            return Magnitude * trans.TransformDirection(newGravity);
        }
    }

}
