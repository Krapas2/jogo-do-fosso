using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LayerMaskExtensionMethods
{
    public static bool Includes(this LayerMask layerMask, GameObject gameObject)
    {
        return (layerMask & (1 << gameObject.layer)) != 0;
    }

    public static LayerMask Inverse(this LayerMask layerMask)
    {
        return ~layerMask | LayerMask.GetMask("All");
    }
}
