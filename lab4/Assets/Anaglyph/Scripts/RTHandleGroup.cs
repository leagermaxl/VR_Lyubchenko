using UnityEngine.Rendering;

namespace Anaglyph3D {
    internal struct RTHandleGroup {
        public RTHandle color;
        public RTHandle depth;

        public RTHandleGroup(RTHandle color, RTHandle depth) {
            this.color = color;
            this.depth = depth;
        }

        public void Release() {
            color?.Release();
            depth?.Release();
        }
    }
}