using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YAxisMove : MonoBehaviour, IGantryComponent
{
    public void FollowPrintHead(Vector3 printHeadDelta)
    {
        Vector3 newPosition = transform.localPosition;
        newPosition.z += printHeadDelta.z;
        transform.localPosition = newPosition;
    }
}
