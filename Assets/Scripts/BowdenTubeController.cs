using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Splines;

public class BowdenTubeController : TubeController
{
    [SerializeField] private FilamentController filament;
    [SerializeField] private PrintHeadCableController cable;

    private float tangentLength = 0.14f;

    private float xBias = 0.25f;
    private float zBias = 0.8f;

    private void Start()
    {
        UpdateMiddleKnot();
        extrude.Rebuild();

        filament.UpdateFilament(spline[0], spline[1]);
        cable.UpdatePrintHeadCable(spline[0], spline[1]);
    }

    public void UpdateBowdenTube(Vector3 printHeadDelta)
    {
        // updating the print head knot
        BezierKnot printHeadKnot = spline[0];

        // print head knot follows the print head
        Vector3 printHeadKnotPos = CalculatePrintHeadKnot(printHeadDelta);
        printHeadKnot.Position = printHeadKnotPos;
        spline[0] = printHeadKnot;

        // updating the middle knot
        UpdateMiddleKnot();

        extrude.Rebuild();

        // updating the filament and the cable
        filament.UpdateFilament(spline[0], spline[1]);
        cable.UpdatePrintHeadCable(spline[0], spline[1]);
    }

    private Vector3 CalculatePrintHeadKnot(Vector3 delta)
    {
        // figuring out where the printhead moved in relation to the spline
        Vector3 localDelta = transform.InverseTransformVector(delta);

        // calculating new position
        BezierKnot printHeadKnot = spline[0];
        Vector3 newPosition = printHeadKnot.Position;
        newPosition += localDelta;

        return newPosition;
    }

    private void UpdateMiddleKnot()
    {
        BezierKnot middleKnot = spline[1];

        // updating the tangent
        Vector3 middleKnotTangent = CalculateMiddleKnotTangent();
        middleKnot.TangentIn = middleKnotTangent;
        middleKnot.TangentOut = -middleKnotTangent;

        // finding the position
        Vector3 middleKnotPos = CalculateMiddleKnot();
        middleKnot.Position = middleKnotPos;

        spline[1] = middleKnot;
    }

    private Vector3 CalculateMiddleKnotTangent()
    {
        // finding the difference between the first and the last knot
        Vector3 delta = spline[0].Position - spline[2].Position;
        delta.y = 0f;
        Vector3 dir = delta.normalized;
        Vector3 middleTangent = dir * tangentLength;

        return middleTangent;
    }

    private Vector3 CalculateMiddleKnot()
    {
        Vector3 knot0 = spline[0].Position;
        Vector3 knot2 = spline[2].Position;

        // calculating horizontal distance between knots
        float xDelta = knot0.x - knot2.x;
        float zDelta = knot0.z - knot2.z;
        float knotsDistance = Mathf.Sqrt(xDelta * xDelta + zDelta * zDelta);

        // middle knot is closer to the printhead by x and to the feeder by z
        float midX = (1 - xBias) * knot0.x + xBias * knot2.x;
        float midZ = (1 - zBias) * knot0.z + zBias * knot2.z;

        // linear formula
        float midY = -0.3081f * knotsDistance + 0.2562f;

        return new Vector3(midX, midY, midZ);
    }
}
