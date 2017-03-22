using UnityEngine;

using Danware.Unity.Input;

namespace Rolling {

    public class GravityTester : MonoBehaviour {

        private static float DIAGONAL = Mathf.Sqrt(2f) / 2f;

        // INSPECTOR FIELDS
        public WorldRotater MainCameraRotater;
        public float Magnitude = 9.81f;
        public GravityShifter GravityShifter;

        [Header("Gravity Shift Inputs")]
        public StartStopInput UpLeftInput;
        public StartStopInput UpInput;
        public StartStopInput UpRightInput;
        public StartStopInput LeftInput;
        public StartStopInput RightInput;
        public StartStopInput DownLeftInput;
        public StartStopInput DownInput;
        public StartStopInput DownRightInput;
        public StartStopInput ZeroGInput;

        // EVENT HANDLERS
        private void Awake() {
            Debug.Assert(MainCameraRotater != null, $"A {nameof(GravityTester)} must be associated with a {nameof(WorldRotater)}");
            if (GravityShifter == null)
                Debug.LogWarning($"A {nameof(GravityTester)} must be associated with a {nameof(GravityShifter)}");
        }
        private void Update() {
            // Get player input
            bool inputGiven = (UpLeftInput?.Started ?? false) ||
                              (UpInput?.Started ?? false) ||
                              (UpRightInput?.Started ?? false) ||
                              (LeftInput?.Started ?? false) ||
                              (RightInput?.Started ?? false) ||
                              (DownLeftInput?.Started ?? false) ||
                              (DownInput?.Started ?? false) ||
                              (DownRightInput?.Started ?? false) ||
                              (ZeroGInput?.Started ?? false);

            // If a direction button was pressed then adjust gravity
            if (inputGiven) {
                Vector2 newG = newGravity();
                if (newG != Physics2D.gravity)
                    GravityShifter?.SetGravity(newG);
            }
        }

        // HELPERS
        private Vector2 newGravity() {
            // Return a zero-vector if gravity has been turned off
            bool gravityOff = (ZeroGInput?.Started ?? false);
            if (gravityOff)
                return Vector2.zero;

            // Otherwise, if the MainCamera is still rotating, then just return the current gravity vector;
            if (MainCameraRotater.IsRotating)
                return Physics2D.gravity;

            // Get the x-component
            float gx = ((UpLeftInput?.Started ?? false) ? -DIAGONAL : 0) +
                       ((UpRightInput?.Started ?? false) ? DIAGONAL : 0) +
                       ((LeftInput?.Started ?? false) ? -1 : 0) +
                       ((RightInput?.Started ?? false) ? 1 : 0) +
                       ((DownLeftInput?.Started ?? false) ? -DIAGONAL : 0) +
                       ((DownRightInput?.Started ?? false) ? DIAGONAL : 0);

            // Get the y-component
            float gy = ((UpLeftInput?.Started ?? false) ? DIAGONAL : 0) +
                       ((UpInput?.Started ?? false) ? 1 : 0) +
                       ((UpRightInput?.Started ?? false) ? DIAGONAL : 0) +
                       ((DownLeftInput?.Started ?? false) ? -DIAGONAL : 0) +
                       ((DownInput?.Started ?? false) ? -1 : 0) +
                       ((DownRightInput?.Started ?? false) ? -DIAGONAL : 0);

            // Return the unit vector with these components, in camera space
            Vector2 localDir = new Vector2(gx, gy);
            Vector2 worldDir = MainCameraRotater.MainCamera.transform.TransformDirection(localDir);
            return Magnitude * worldDir;
        }
    }

}
