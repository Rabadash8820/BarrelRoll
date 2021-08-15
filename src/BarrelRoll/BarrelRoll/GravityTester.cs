using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

using UnityEngine.Inputs;

namespace BarrelRoll {

    public class GravityTester : MonoBehaviour {

        private readonly List<(StartStopInput Input, Vector2 Direction)> _gDirs = new(8);   // Capacity for the 8 possible directions

        private Vector2 _nextG;

        // INSPECTOR FIELDS
        public WorldRotater WorldRotater;
        public float Magnitude = 9.81f;
        public GravityShifter GravityShifter;

        [Header("Gravity Shift Inputs")]
        public StartStopInput UpLeftInput;
        public StartStopInput UpInput;
        public StartStopInput UpRightInput;
        public StartStopInput LeftInput;
        public StartStopInput RightInput;
        public StartStopInput DownLeftInput;
        public StartStopInput DownInput;
        public StartStopInput DownRightInput;
        public StartStopInput ZeroGInput;

        // EVENT HANDLERS
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unity message")]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Unity message")]
        private void Awake() {
            this.AssertAssociation(WorldRotater, nameof(WorldRotater));
            this.AssertAssociation(GravityShifter, nameof(GravityShifter));

            _nextG = Physics2D.gravity;

            _gDirs.AddRange(new[] {
                (UpLeftInput, (Vector2.up + Vector2.left).normalized),
                (UpInput, Vector2.up),
                (UpRightInput, (Vector2.up + Vector2.right).normalized),
                (LeftInput, Vector2.left),
                (RightInput, Vector2.right),
                (DownLeftInput, (Vector2.down + Vector2.left).normalized),
                (DownInput, Vector2.down),
                (DownRightInput, (Vector2.down + Vector2.right).normalized),
            });
        }

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unity message")]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Unity message")]
        private void Update() {
            Vector2 currG = Physics2D.gravity;
            
            // Don't check for changes to gravity while world is rotating, unless to make gravity zero,
            // b/c turning off gravity while things are still moving lets them smash and bounce around, which is fun :P
            if (ZeroGInput?.Started() ?? false)
                startChangingGravity(Vector2.zero);
            if (WorldRotater.IsRotating)
                return;

            Vector2? gDir = null;
            for (int g = 0; g < _gDirs.Count; ++g) {
                if (_gDirs[g].Input?.Started() ?? false) {
                    gDir = _gDirs[g].Direction;
                    break;
                }
            }

            if (gDir.HasValue && gDir.Value != currG) {
                Vector2 nextG = Magnitude * WorldRotater.MainCamera.TransformDirection(gDir.Value);
                startChangingGravity(nextG);
            }


            void startChangingGravity(Vector2 nextGravity)
            {
                _nextG = nextGravity;
                GravityShifter.StartGravityEffects(_nextG);
            }
        }

        public void CommitNewGravity() => GravityShifter.SetGravity(_nextG);

    }

}
