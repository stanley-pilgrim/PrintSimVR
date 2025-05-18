using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicsManager : MonoBehaviour
{
    [SerializeField]
    private Transform printHead;
    [SerializeField]
    private BowdenAssembly bowdenAssembly;

    private Vector3 printHeadPos;

    void Start()
    {
        printHeadPos = printHead.position;
    }

    void Update()
    {
        Vector3 printHeadDelta = printHead.position - printHeadPos;
        if (printHeadDelta.magnitude < 0.001f) return;

        // updating cached position
        printHeadPos = printHead.position;

        // updating the bowden tube
        bowdenAssembly.UpdateBowdenAssembly(printHeadDelta);
    }
}
