using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(LineRenderer))]
public class TubeRenderer : MonoBehaviour
{
    [SerializeField] private int segments = 12;
    [SerializeField] private float width = 0.01f;

    private LineRenderer lineRenderer;
    private Spline spline;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        spline = GetComponent<SplineContainer>().Spline;

        lineRenderer.positionCount = segments + 1;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        lineRenderer.useWorldSpace = false;
    }

    public void RenderTube()
    {
        for (int i = 0; i < segments + 1; i++)
        {
            float t = (float)i / segments;
            Vector3 pos = spline.EvaluatePosition(t);
            lineRenderer.SetPosition(i, pos);
        }
    }

}
