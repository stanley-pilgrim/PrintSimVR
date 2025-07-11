using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(SplineContainer))]

public class BowdenTubeController : TubeController, IGantryComponent
{
    [SerializeField] private FilamentController filament;
    [SerializeField] private PrintHeadCableController cable;

    private float bowdenLength = 0.65f;

    private float tangentLength = 0.12f;

    private float xBias = 0.1f;
    private float zBias = 0.5f;

    private Vector3 middleHomePos = new Vector3(-0.06f, 0f, -0.08f);

    private void Start()
    {
        UpdateMiddleKnot();
        tubeRenderer.RenderTube();

        filament.UpdateFilament(spline[2], spline[1]);
        cable.UpdatePrintHeadCable(spline[2], spline[1]);
    }

    public void FollowPrintHead(Vector3 printHeadDelta)
    {
        // updating the print head knot
        BezierKnot printHeadKnot = spline[2];

        // print head knot follows the print head
        Vector3 printHeadKnotPos = CalculatePrintHeadKnot(printHeadDelta);
        printHeadKnot.Position = printHeadKnotPos;
        spline[2] = printHeadKnot;

        // updating the middle knot
        UpdateMiddleKnot();

        tubeRenderer.RenderTube();

        // updating the filament and the cable
        filament.UpdateFilament(spline[2], spline[1]);
        cable.UpdatePrintHeadCable(spline[2], spline[1]);
    }

    private Vector3 CalculatePrintHeadKnot(Vector3 delta)
    {
        // calculating new position
        BezierKnot printHeadKnot = spline[2];
        Vector3 newPosition = printHeadKnot.Position;
        newPosition += delta;

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
        Vector3 middleKnotPos = CalculateMiddleKnot(middleKnot.Position.y);
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

    private Vector3 CalculateMiddleKnot(float lastY)
    {
        const float epsilon = 0.001f;
        const int maxIterations = 10;

        Vector3 knot0 = spline[0].Position;
        Vector3 knot2 = spline[2].Position;

        // calculating position by x and z
        Vector3 printHeadOffset = knot2 - knot0;
        float midX = middleHomePos.x + printHeadOffset.x * xBias;
        float midZ = middleHomePos.z + printHeadOffset.z * zBias;

        // can't get lower than the printhead and higher than the half the length
        float minY = Mathf.Min(lastY, 0);
        float maxY = Mathf.Max(lastY, bowdenLength / 2);

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

    /* private Vector3 CalculateMiddleKnotLinear()
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
    } */
}
