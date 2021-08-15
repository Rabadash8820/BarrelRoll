using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Inputs;

namespace BarrelRoll {

    [RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
    public class RollMover : MonoBehaviour {
        // HIDDEN FIELDS
        private Rigidbody2D _rigidbody;
        private CircleCollider2D _circle;
        private bool _grounded = false;
        private readonly RaycastHit2D[] _groundHits = new RaycastHit2D[2];

        // INSPECTOR FIELDS
        public StartStopInput JumpInput;
        public ValueInput RollInput;
        public float MoveSpeed = 2f;    // Units/sec
        public float MoveForce = 5f;    // Units/sec
        public bool CanJump = true;
        public float JumpForce = 5f;
        public float GroundedOffset = 0.3f;
        public bool JumpAutoOpposesGravity = true;

        public UnityEvent Jumping = new();

        // EVENT HANDLERS
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unity message")]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Unity message")]
        private void Awake() {
            _rigidbody = GetComponent<Rigidbody2D>();
            _circle = GetComponent<CircleCollider2D>();
        }

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unity message")]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Unity message")]
        private void Update() {
            // Get player input
            float roll = RollInput?.Value() ?? 0f;
            bool jump = JumpInput?.Started() ?? false;

            // Do the rotation
            Vector2 moveDir = new Vector2(Physics2D.gravity.y, -Physics2D.gravity.x).normalized;
            float moveMag = roll * MoveForce;
            _rigidbody.AddForce(moveMag * moveDir);

            // Apply jump forces
            if (jump)
                doJump();
        }

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unity message")]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Unity message")]
        private void FixedUpdate() {
            // Check if this object is grounded
            int numHits = Physics2D.CircleCastNonAlloc(transform.position, _circle.radius, Physics2D.gravity, _groundHits, GroundedOffset);
            _grounded = (numHits > 1);
        }

        // HELPER FUNCTIONS
        private void doJump() {
            // If the player isn't grounded or there is no gravity, then just return
            bool zeroG = Physics2D.gravity == Vector2.zero;
            if (!_grounded || zeroG)
                return;

            // Apply a force to oppose gravity, if requested
            if (JumpAutoOpposesGravity)
                _rigidbody.AddForce(-Physics2D.gravity);

            // Apply the jump force
            float g = Physics2D.gravity.magnitude;
            Vector2 jumpDir = -Physics2D.gravity / g;
            _rigidbody.AddForce(JumpForce * jumpDir, ForceMode2D.Impulse);

            Jumping.Invoke();
        }
    }

}

