using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XAxisRotate : MonoBehaviour, IGantryComponent
{
    private float axisDiameter = 0.008f;

    public void FollowPrintHead(Vector3 printHeadDelta)
    {
        float rotationDegrees = - (printHeadDelta.x * 360f) / (Mathf.PI * axisDiameter);
        transform.Rotate(0, 0, rotationDegrees);
        Debug.Log($"Received delta: {printHeadDelta.x:F6}");
        Debug.Log($"Rotation degrees: {rotationDegrees:F2}");
    }
}
