using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class TubeController : MonoBehaviour
{
    private Spline spline;
    private SplineExtrude extrude;

    void Awake()
    {
        spline = GetComponent<SplineContainer>().Spline;
        extrude = GetComponent<SplineExtrude>();
    }

    public void UpdateTube(Vector3 delta)
    {
        Vector3 localDelta = transform.InverseTransformDirection(delta);
        
        BezierKnot printHeadKnot = spline[0];
        Vector3 printHeadKnotPos = printHeadKnot.Position;
        printHeadKnotPos += localDelta;
        printHeadKnot.Position = printHeadKnotPos;
        spline[0] = printHeadKnot;
        extrude.Rebuild();
    }
}
