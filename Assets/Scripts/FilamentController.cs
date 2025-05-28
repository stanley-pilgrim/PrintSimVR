using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(SplineContainer))]
public class FilamentController : TubeController
{
    public void UpdateFilament(BezierKnot printHeadKnot, BezierKnot middleKnot)
    {
        spline[2] = printHeadKnot;
        spline[1] = middleKnot;

        //extrude.Rebuild();
        tubeRenderer.RenderTube();
    }
}
