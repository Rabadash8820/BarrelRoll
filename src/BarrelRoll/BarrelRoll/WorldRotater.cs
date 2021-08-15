using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace BarrelRoll {

    public class WorldRotater : MonoBehaviour
    {
        private Vector2 _targetDir = Vector2.zero;

        public Transform MainCamera;
        public GravityShifter GravityShifter;
        [Tooltip("Camera will rotate smoothly at this angular speed (degrees/second).")]
        public float Speed = 180f;

        // EVENT HANDLERS
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unity message")]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Unity message")]
        private void Awake() {
            this.AssertAssociation(GravityShifter, nameof(GravityShifter));

            // Rotate towards the opposite direction of gravity, when that direction changes
            GravityShifter.Shifted += (sender, e) => {
                Vector2 target = -Physics2D.gravity;
                if (MainCamera != null && target != Vector2.zero)
                    _targetDir = target;
            };
        }

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unity message")]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Unity message")]
        private void Update() {
            // Reset when the target rotation is reached
            Transform trans = MainCamera.transform;
            var targetRot = Quaternion.LookRotation(Vector3.forward, _targetDir);
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
