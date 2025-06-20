using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YAxisRotate : MonoBehaviour, IGantryComponent
{
    private float axisDiameter = 0.008f;

    public void FollowPrintHead(Vector3 printHeadDelta)
    {
        float rotationDegrees = (printHeadDelta.z * 360f) / (Mathf.PI * axisDiameter);
        transform.Rotate(rotationDegrees, 0, 0);
    }
}
