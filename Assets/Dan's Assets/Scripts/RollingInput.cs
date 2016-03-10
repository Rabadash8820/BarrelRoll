using UnityEngine;

namespace Rolling {

    class RollingInput {
        public static float RotateCW { get { return -Input.GetAxis("Horizontal"); } }
        public static float RotateCWRaw { get { return -Input.GetAxisRaw("Horizontal"); } }
    }

}
