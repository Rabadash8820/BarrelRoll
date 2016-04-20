using UnityEngine;

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
        public float GroundedOffset = 0.3f;
        public bool JumpAutoOpposesGravity = true;

        // EVENT HANDLERS
        private void Awake() {
            _rigidbody = GetComponent<Rigidbody2D>();
            _circle = GetComponent<CircleCollider2D>();
        }
        private void Update() {
            // Get player input
            float rot = RollingInput.Roll;
            bool jump = RollingInput.JumpStart;

            // Do the rotation
            Vector2 moveDir = new Vector2(Physics2D.gravity.y, -Physics2D.gravity.x).normalized;
            float moveMag = rot * MoveForce;
            _rigidbody.AddForce(moveMag * moveDir);

            // Apply jump forces
            if (jump)
                doJump();
        }
        private void FixedUpdate() {
            // Check if this object is grounded
            int numHits = Physics2D.CircleCastNonAlloc(transform.position, _circle.radius, Physics2D.gravity, _groundHits, GroundedOffset);
            _grounded = (numHits > 1);
        }
        private void OnDrawGizmos() {
            if (_circle != null) {
                Vector2 pos = transform.position;
                Vector2 gravityOffset = Physics2D.gravity.normalized * (_circle.radius + GroundedOffset);
                Gizmos.DrawLine(pos, pos + gravityOffset);
            }
        }

        // HELPER FUNCTIONS
        private void doJump() {
            // If the player isn't grounded
            if (!_grounded)
                return;

            // Apply a force to oppose gravity, if requested
            if (JumpAutoOpposesGravity)
                _rigidbody.AddForce(-Physics2D.gravity);

            // Apply the jump force
            float g = Physics2D.gravity.magnitude;
            Vector2 jumpDir = -Physics2D.gravity / g;
            _rigidbody.AddForce(JumpForce * jumpDir, ForceMode2D.Impulse);
        }
    }

}

