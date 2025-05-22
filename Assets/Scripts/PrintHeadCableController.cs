using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class PrintHeadCableController : TubeController
{
    [SerializeField] public float xOffset = 0f;
    public void UpdatePrintHeadCable(BezierKnot printHeadKnot, BezierKnot middleKnot)
    {
        BezierKnot cablePrintHeadKnot = printHeadKnot;
        BezierKnot cableMiddleKnot = middleKnot;
        cablePrintHeadKnot.Position.x += xOffset;
        cableMiddleKnot.Position.x += xOffset;
        spline[0] = cablePrintHeadKnot;
        spline[1] = cableMiddleKnot;

        extrude.Rebuild();
    }
}
