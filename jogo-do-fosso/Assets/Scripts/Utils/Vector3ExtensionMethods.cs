using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3ExtensionMethods
{
    public static Vector2 Vector2(this Vector3 v)
    {
        return new Vector2(v.x, v.y);
    }

    public static Vector3 ClosestPosition(this Vector3 thisVector, Vector3[] compareVectors)
    {
        Vector3 closestPosition = Vector3.positiveInfinity;
        foreach(Vector3 compareVector in compareVectors){
            if(Vector3.Distance(thisVector, compareVector) < Vector3.Distance(thisVector, closestPosition)){
                closestPosition = compareVector;
            }
        }
        return closestPosition;
    }
}
