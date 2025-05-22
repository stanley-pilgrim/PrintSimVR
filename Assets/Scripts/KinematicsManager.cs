using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicsManager : MonoBehaviour
{
    [SerializeField]
    private Transform printHead;
    [SerializeField]
    private BowdenTubeController bowdenTube;

    [SerializeField] 
    private float movementThreshold = 0.01f;

    private Vector3 printHeadPos;

    void Start()
    {
        printHeadPos = printHead.position;
    }

    void Update()
    {
        Vector3 printHeadDelta = printHead.position - printHeadPos;
        if (printHeadDelta.magnitude < movementThreshold) return;

        // updating cached position
        printHeadPos = printHead.position;

        // updating the bowden tube
        bowdenTube.UpdateBowdenTube(printHeadDelta);
    }
}
