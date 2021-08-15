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
        private Vector2 _oldG;

        // INSPECTOR FIELDS
        [Tooltip("This GameObject will be activated every time the gravity shifts, allowing for custom particle effects/animations.")]
        public GameObject GravityShiftEffects;

        [Tooltip("This GameObject will be activated every time gravity is set to zero, allowing for custom particle effects/animations.")]
        public GameObject ZeroGravityEffects;

        public event EventHandler Shifted;

        public void StartGravityEffects(Vector2 newGravity) {
            _oldG = Physics2D.gravity;

            // Start the gravity shift effects, with a rotation to match the new gravity direction
            if (newGravity != Vector2.zero && GravityShiftEffects != null) {
                var rot = Quaternion.LookRotation(forward: Vector3.forward, upwards: newGravity);
                GravityShiftEffects.transform.rotation = rot;
                GravityShiftEffects.SetActive(true);
            }
            else if (newGravity == Vector2.zero && ZeroGravityEffects) {
                ZeroGravityEffects.SetActive(true);
            }
        }

        public void EndGravityEffects()
        {
            GravityShiftEffects.SetActive(false);
            ZeroGravityEffects.SetActive(false);
        }

        public void SetGravity(Vector2 newGravity) {
            // Adjust gravity, if necessary
            if (newGravity == _oldG)
                return;
            Physics2D.gravity = newGravity;

            Shifted?.Invoke(this, EventArgs.Empty);
        }

    }

}
