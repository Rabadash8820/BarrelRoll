using System;

using UnityEngine;
using UnityEngine.Events;

namespace Rolling {

    public class GravityChangeEventArgs : EventArgs {
        public Vector2 OldVector;
        public Vector2 NewVector;
    }

    public class GravityShifter : MonoBehaviour {
        // HIDDEN FIELDS
        private EventHandler<GravityChangeEventArgs> _changedInvoker;

        // API INTERFACE
        public event EventHandler<GravityChangeEventArgs> Shifted {
            add { _changedInvoker += value; }
            remove { _changedInvoker -= value; }
        }
        public void SetGravity(Vector2 vector) {
            // Adjust the direction/magnitude of gravity
            Vector2 old = Physics2D.gravity;
            Physics2D.gravity = vector;

            // Raise the changed event, if gravity actually changed
            if (Physics2D.gravity != old) {
                GravityChangeEventArgs args = new GravityChangeEventArgs {
                    OldVector = old,
                    NewVector = Physics2D.gravity,
                };
                _changedInvoker?.Invoke(this, args);
            }
        }
        public void SetZeroGravity() {
            // Adjust the direction/magnitude of gravity
            Vector2 old = Physics2D.gravity;
            Physics2D.gravity = Vector2.zero;

            // Raise the changed event, if gravity actually changed
            if (Physics2D.gravity != old) {
                GravityChangeEventArgs args = new GravityChangeEventArgs {
                    OldVector = old,
                    NewVector = Physics2D.gravity,
                };
                _changedInvoker?.Invoke(this, args);
            }
        }
    }

}
