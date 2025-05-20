using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class FilamentController : TubeController
{
    public void UpdateFilament(Vector3 printHeadKnotPos)
    {
        BezierKnot printHeadKnot = spline[0];
        printHeadKnot.Position = printHeadKnotPos;
        spline[0] = printHeadKnot;
        extrude.Rebuild();
    }
}
