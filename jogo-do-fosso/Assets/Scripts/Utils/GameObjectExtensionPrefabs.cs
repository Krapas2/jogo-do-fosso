using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class GameObjectExtensionMethods
{
    public static bool ComparePrefabs(this GameObject thisObject, GameObject otherObject)
    {
        if (!PrefabUtility.IsPartOfAnyPrefab(thisObject)){
            return false;
        }

        GameObject thisPrefab = PrefabUtility.GetCorrespondingObjectFromSource(thisObject);
        GameObject otherPrefab = PrefabUtility.GetCorrespondingObjectFromSource(otherObject);

        return thisPrefab == otherPrefab;
    }
}
