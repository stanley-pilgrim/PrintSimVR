using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public abstract class TubeController : MonoBehaviour
{
    protected Spline spline;
    protected TubeRenderer tubeRenderer;

    protected void Awake()
    {
        spline = GetComponent<SplineContainer>().Spline;
        tubeRenderer = GetComponent<TubeRenderer>();

        for (int i = 0; i < spline.Count; i++)
        {
            spline.SetTangentMode(i, TangentMode.Broken);
        }
    }
}
