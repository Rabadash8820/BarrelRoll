using UnityEngine;
using System.Collections;

namespace Rolling {

    [RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
    public class RollMover : MonoBehaviour {
        // HIDDEN FIELDS
        private Rigidbody2D _rigidbody;
        private CircleCollider2D _circle;
        private bool _grounded = false;
        RaycastHit2D[] _groundHits = new RaycastHit2D[2];

        // INSPECTOR FIELDS
        public float MoveForce = 1f;
        public bool CanJump = true;
        public float JumpForce = 1f;
        public float MaxOffsetToJump = 0.3f;
        public bool JumpAutoOpposesGravity = true;

        // EVENT HANDLERS
        private void Awake() {
            _rigidbody = GetComponent<Rigidbody2D>();
            _circle = GetComponent<CircleCollider2D>();
        }
        private void Update() {
            // Get player input
            float rot = RollingInput.RotateCW;
            bool jump = RollingInput.JumpStart;

            // Do the rotation
            Vector2 moveDir = new Vector2(Physics2D.gravity.y, -Physics2D.gravity.x).normalized;
            float moveMag = rot * MoveForce;
            _rigidbody.AddForce(moveMag * moveDir);

            // Do jump
            if (jump && _grounded) {
                jump = false;
                float g = Physics.gravity.magnitude;
                Vector2 jumpDir = -Physics2D.gravity / g;
                if (JumpAutoOpposesGravity)
                    _rigidbody.AddForce(-Physics2D.gravity);
                _rigidbody.AddForce(JumpForce * jumpDir, ForceMode2D.Impulse);
            }
        }
        private void FixedUpdate() {
            // Check if this object is grounded
            int numHits = Physics2D.RaycastNonAlloc(transform.position, Physics2D.gravity, _groundHits, _circle.radius + MaxOffsetToJump);
            _grounded = (numHits > 1);
        }
        private void OnDrawGizmos() {
            Gizmos.DrawLine(transform.position, transform.position + Physics.gravity.normalized * (_circle.radius + MaxOffsetToJump));
        }
    }

}

