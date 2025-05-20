using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class TubeController : MonoBehaviour
{
    protected Spline spline;
    protected SplineExtrude extrude;

    void Awake()
    {
        spline = GetComponent<SplineContainer>().Spline;
        extrude = GetComponent<SplineExtrude>();
    }
}
