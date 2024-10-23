using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector2ExtensionMethods
{
    public static Vector3 Vector3(this Vector2 v)
    {
        return new Vector3(v.x, v.y, 0f);
    }

    public static Vector2 ClosestPosition(this Vector2 thisVector, Vector2[] compareVectors)
    {
        Vector2 closestPosition = Vector2.positiveInfinity;
        foreach(Vector2 compareVector in compareVectors){
            if(Vector2.Distance(thisVector, compareVector) < Vector2.Distance(thisVector, closestPosition)){
                closestPosition = compareVector;
            }
        }
        return closestPosition;
    }
}
