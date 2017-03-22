using UnityEngine;

namespace Rolling {

    public class WorldRotater : MonoBehaviour {
        // HIDDEN FIELDS
        private Vector2 _targetDir = Vector2.zero;

        // INSPECTOR FIELDS
        public Transform MainCamera;
        public GravityShifter GravityShifter;
        [Tooltip("Camera will rotate smoothly at this angular speed (degrees/second).")]
        public float Speed = 90f;

        // EVENT HANDLERS
        private void Awake() {
            Debug.Assert(GravityShifter != null, $"{nameof(WorldRotater)} {name} must be associated with a {nameof(GravityShifter)}!");

            // Rotate towards the opposite direction of gravity, when that direction changes
            GravityShifter.Shifted += (sender, e) => {
                Vector2 target = -e.NewVector;
                if (MainCamera != null && target != Vector2.zero)
                    _targetDir = target;
            };
        }
        private void Update() {
            // Reset when the target rotation is reached
            Transform trans = MainCamera.transform;
            Quaternion targetRot = Quaternion.LookRotation(Vector3.forward, _targetDir);
            if (trans.rotation == targetRot)
                _targetDir = Vector2.zero;

            // Slerp the Transform's rotation towards the target
            if (_targetDir != Vector2.zero)
                trans.rotation = Quaternion.RotateTowards(trans.rotation, targetRot, Speed * Time.deltaTime);
        }

        // API INTERFACE
        public bool IsRotating => _targetDir != Vector2.zero;

    }

}