using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class FilamentController : TubeController
{
    public void UpdateFilament(BezierKnot printHeadKnot, BezierKnot middleKnot)
    {
        spline[0] = printHeadKnot;
        spline[1] = middleKnot;

        extrude.Rebuild();
    }
}
