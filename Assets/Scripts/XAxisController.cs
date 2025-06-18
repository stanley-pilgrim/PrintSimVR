using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XAxisController : MonoBehaviour, IGantryComponent
{
    public void FollowPrintHead(Vector3 printHeadDelta)
    {
        Vector3 newPosition = transform.localPosition;
        newPosition.x += printHeadDelta.x;
        transform.localPosition = newPosition;
    }
}
