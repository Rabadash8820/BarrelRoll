using UnityEngine;

namespace Rolling {

    public class Rotator : MonoBehaviour {
        // INSPECTOR FIELDS
        public Transform Target;
        public float RotationSpeed = 1f;

        // EVENT HANDLERS
        private void Awake() {

        }
        private void Update() {
            // Get player input
            float rot = RollingInput.RotateCW;

            // Do the rotation
            float rotMag = rot * RotationSpeed;
            Vector3 rotAxis = Vector3.forward;
            doRotate(rotMag, rotAxis);
        }

        // HELPER FUNCTIONS
        private void doRotate(float magnitude, Vector3 axis) {
            // If the Target has a non-kinematic Rigidbody, then apply a Torque
            Rigidbody2D rb = Target.GetComponent<Rigidbody2D>();
            if (rb != null && !rb.isKinematic)
                rb.AddTorque(magnitude);

            // Otherwise, rotate its Transform directly
            else
                Target.Rotate(magnitude * axis);

        }
    }

}
