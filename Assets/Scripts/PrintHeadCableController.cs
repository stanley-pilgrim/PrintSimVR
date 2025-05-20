using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class PrintHeadCableController : TubeController
{
    public void UpdatePrintHeadCable(Vector3 printHeadKnotPos)
    {
        BezierKnot printHeadKnot = spline[0];
        printHeadKnot.Position = printHeadKnotPos;
        spline[0] = printHeadKnot;
        extrude.Rebuild();
    }
}
