using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class KinematicsManager : MonoBehaviour
{
    [SerializeField]
    private Transform printHead;

    private IGantryComponent[] gantryComponents;

    private float movementThreshold = 0.01f;
    private Vector3 printHeadPos;

    void Start()
    {
        gantryComponents = GetComponentsInChildren<IGantryComponent>();
        printHeadPos = transform.InverseTransformPoint(printHead.position);
    }

    void Update()
    {
        Vector3 newPrintHeadPos = transform.InverseTransformPoint(printHead.position);
        Vector3 printHeadDelta = newPrintHeadPos - printHeadPos;
        if (printHeadDelta.magnitude == 0) return;

        // updating cached position
        printHeadPos = newPrintHeadPos;

        foreach (var component in gantryComponents)
        {
            component.FollowPrintHead(printHeadDelta);
        }
    }
}
