using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

using UnityEngine.Triggers;

namespace BarrelRoll {

    public class GravityChangeEventArgs : EventArgs {
        public Vector2 OldVector;
        public Vector2 NewVector;
    }

    public class GravityShifter : MonoBehaviour {
        // HIDDEN FIELDS
        private EventHandler<GravityChangeEventArgs> _changedInvoker;
        private Vector2 _oldG;
        private Vector2 _newG;

        // INSPECTOR FIELDS
        public AnimationEventTrigger AnimationEventTrigger;
        [Tooltip("This GameObject will be activated every time the gravity shifts, allowing for custom particle effects/animations.  This GameObject should have a child Animator/AnimationEventDetector component pair that raises AnimationEvents named 'GravityShifted' and 'EffectsCompleted.")]
        public GameObject GravityShiftEffects;

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unity message")]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Unity message")]
        private void Awake() {
            // When the appropriate animation event is raised by the Effects GameObject, then perform the actual gravity shift
            if (AnimationEventTrigger != null) {
                AnimationEventTrigger.AnimationEventOccurred.AddListener((animator, eventName) => {
                    if (eventName == "GravityShifted")
                        setGravity(_newG);
                    if (eventName == "EffectsCompleted")
                        GravityShiftEffects.SetActive(false);
                });
            }
        }

        // API INTERFACE
        public event EventHandler<GravityChangeEventArgs> Shifted {
            add { _changedInvoker += value; }
            remove { _changedInvoker -= value; }
        }
        public void StartGravityEffects(Vector2 newGravity) {
            _oldG = Physics2D.gravity;
            _newG = newGravity;

            // Start the gravity shift effects, with a rotation to match the new gravity direction
            if (GravityShiftEffects != null) {
                var rot = Quaternion.LookRotation(forward: Vector3.forward, upwards: newGravity);
                GravityShiftEffects.transform.rotation = rot;
                GravityShiftEffects.SetActive(true);
            }
        }
        public void SetZeroGravity() {
            // Adjust the direction/magnitude of gravity
            Vector2 old = Physics2D.gravity;
            Physics2D.gravity = Vector2.zero;

            // Raise the changed event, if gravity actually changed
            if (Physics2D.gravity != old) {
                var args = new GravityChangeEventArgs {
                    OldVector = old,
                    NewVector = Physics2D.gravity,
                };
                _changedInvoker?.Invoke(this, args);
            }
        }

        // HELPERS
        private void setGravity(Vector2 newGravity) {
            // Adjust gravity, if necessary
            if (newGravity == _oldG)
                return;
            Physics2D.gravity = newGravity;

            // Raise the gravity changed event
            var args = new GravityChangeEventArgs {
                OldVector = _oldG,
                NewVector = Physics2D.gravity,
            };
            _changedInvoker?.Invoke(this, args);
        }

    }

}
