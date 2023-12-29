using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Anaglyph3D {
    [Serializable]
    public sealed class Settings {
        public RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        public LayerMask layerMask = -1;
        public Shader shader = null;

        [Header("Transform")]
        public float spacing = 0.2f;
        public float focalPoint = 10f;

        [Header("Blending")]
        public OverlayMode overlayMode = OverlayMode.Opacity;
        public BlendMode blendMode = BlendMode.None;

        [Header("Rendering")]
        public RenderTextureFormat opacityOverlayRenderTextureFormat = RenderTextureFormat.ARGB32;
        public DepthBufferBitCount depthOverlayBufferBitCount = DepthBufferBitCount._24;

        internal bool SingleChannel => spacing == 0;

        internal bool NeedsDepth => overlayMode == OverlayMode.Depth;

        internal Material material = default;

        public enum OverlayMode : int {
            None = 0,
            Opacity = 1,
            Depth = 2
        }

        public enum BlendMode : int {
            None = 0,
            Additive = 1,
            Channel = 2
        }

        public enum DepthBufferBitCount : int {
            [InspectorName("0")] _0 = 0,
            [InspectorName("16")] _16 = 16,
            [InspectorName("24")] _24 = 24,
            [InspectorName("32")] _32 = 32,
        }
    }
}