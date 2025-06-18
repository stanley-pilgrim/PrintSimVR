using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGantryComponent
{
    void FollowPrintHead(Vector3 printHeadDelta);
}
