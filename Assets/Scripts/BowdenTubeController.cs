using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Splines;

public class BowdenTubeController : TubeController
{
    [SerializeField] private FilamentController filament;
    [SerializeField] private PrintHeadCableController cable;
    [SerializeField] private float tangentLength = 0.2f;

    private float bowdenLength;

    private void Start()
    {
        bowdenLength = spline.GetLength();
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
        BezierKnot middleKnot = spline[1];

        // updating the tangent
        Vector3 middleKnotTangent = CalculateMiddleKnotTangent();
        middleKnot.TangentIn = middleKnotTangent;
        middleKnot.TangentOut = - middleKnotTangent;
        spline[1] = middleKnot;

        // finding the right height
        middleKnot = spline[1];
        Vector3 middleKnotPos = CalculateMiddleKnot();
        middleKnot.Position = middleKnotPos;
        spline[1] = middleKnot;

        // updating the filament and the cable
        filament.UpdateFilament(printHeadKnot, middleKnot);
        cable.UpdatePrintHeadCable(printHeadKnot, middleKnot);

        extrude.Rebuild();
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
        const float epsilon = 0.01f;
        const int maxIterations = 20;

        Vector3 knot0 = spline[0].Position;
        Vector3 knot2 = spline[2].Position;

        // calculating position by x and z
        float midX = (knot0.x + knot2.x) / 2;
        float midZ = (knot0.z + knot2.z) / 2;

        // can't get lower than the printhead and higher than the length
        float minY = knot0.y;
        float maxY = bowdenLength / 2;

        Vector3 testPos = new Vector3(midX, 0f, midZ);

        for (int i = 0; i < maxIterations; i++)
        {
            float midY = (minY + maxY) / 2;
            testPos.y = midY;

            // applying and testing the length
            BezierKnot knot = spline[1];
            knot.Position = testPos;
            spline[1] = knot;

            float currentLength = spline.GetLength();
            float delta = currentLength - bowdenLength;

            // if close enough, ending the cycle
            if (Mathf.Abs(delta) < epsilon) break;
            // the tube is too long
            else if (delta > 0) maxY = midY;
            // the tube is too short
            else if (delta < 0) minY = midY;
        }

        return testPos;
    }
}
