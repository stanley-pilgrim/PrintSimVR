using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public abstract class TubeController : MonoBehaviour
{
    protected Spline spline;
    protected SplineExtrude extrude;

    protected void Awake()
    {
        spline = GetComponent<SplineContainer>().Spline;
        extrude = GetComponent<SplineExtrude>();

        for (int i = 0; i < spline.Count; i++)
        {
            spline.SetTangentMode(i, TangentMode.Broken);
        }
    }
}
