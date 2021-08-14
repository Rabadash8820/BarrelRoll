using UnityEngine;

using UnityEngine.Inputs;

namespace BarrelRoll
{
    /// <summary>
    /// Uses either jump key or single tap on mobile for jumping
    /// </summary>
    [CreateAssetMenu(fileName = "barrel-jump-input", menuName = nameof(BarrelRoll) + "/" + nameof(BarrelJumpInput))]
    public class BarrelJumpInput : StartStopInput
    {
        public override bool Happening() => throw new System.NotImplementedException();
        public override bool Started()
        {
            bool btnInput = Input.GetButtonDown("Jump");
            bool mobileInput = false;
            for (int t = 0; t < Input.touchCount; ++t) {
                if (Input.GetTouch(t).phase == TouchPhase.Began) {
                    mobileInput = true;
                    break;
                }
            }
            return btnInput || mobileInput;
        }
        public override bool Stopped() => throw new System.NotImplementedException();
    }

}
