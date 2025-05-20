using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class BowdenTubeController : TubeController
{
    [SerializeField] private FilamentController filament;
    [SerializeField] private PrintHeadCableController cable;

    public void UpdateBowdenTube(Vector3 printHeadDelta)
    {
        Vector3 localDelta = transform.InverseTransformDirection(printHeadDelta);

        BezierKnot printHeadKnot = spline[0];
        Vector3 printHeadKnotPos = printHeadKnot.Position;
        printHeadKnotPos += localDelta;
        printHeadKnot.Position = printHeadKnotPos;
        spline[0] = printHeadKnot;
        extrude.Rebuild();

        filament.UpdateFilament(printHeadKnotPos);
        cable.UpdatePrintHeadCable(printHeadKnotPos);
    }
}
