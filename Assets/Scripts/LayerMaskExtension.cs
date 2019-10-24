using UnityEngine;

namespace JoelQ.Helper
{
    public static class LayerMaskExtension
    {
        public static bool CompareLayer(this int objLayer, LayerMask layerMask)
        {
            return (layerMask.value & 1 << objLayer) > 0;
        }
    }
}