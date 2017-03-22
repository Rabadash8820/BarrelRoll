using UnityEngine;

namespace Rolling {

    public class WorldRotater : MonoBehaviour {
        // HIDDEN FIELDS
        private Vector2 _targetDir;

        // INSPECTOR FIELDS
        public Transform MainCamera;
        public GravityShifter Mediator;
        [Tooltip("Camera will rotate smoothly at this angular speed (degrees/second).")]
        public float Speed = 45f;

        // EVENT HANDLERS
        private void Awake() {
            Debug.Assert(Mediator != null, $"{nameof(WorldRotater)} {name} must be associated with a GravityMediator!");

            // Rotate towards the opposite direction of gravity, when that direction changes
            Mediator.Shifted += (sender, e) => {
                StartRotating(-e.NewVector);
            };
        }
        private void Update() {
            // Reset when the target rotation is reached
            Quaternion currRot = MainCamera.transform.rotation;
            if (currRot == TargetRotation)
                _targetDir = Vector2.zero;

            // Slerp the Transform's rotation towards the target
            if (_targetDir != Vector2.zero) {
                Quaternion newRot = Quaternion.RotateTowards(currRot, TargetRotation, Speed * Time.deltaTime);
                MainCamera.transform.rotation = newRot;
            }
        }

        // API INTERFACE
        public bool IsRotating => _targetDir != Vector2.zero;
        public void StartRotating(Vector2 target) {
            if (MainCamera != null && target != Vector2.zero)
                _targetDir = target;
        }

        // HELPERS
        private Quaternion TargetRotation =>
            (_targetDir != Vector2.zero) ? Quaternion.LookRotation(Vector3.forward,_targetDir) : Quaternion.identity;
    }

}