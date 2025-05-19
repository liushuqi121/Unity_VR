using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryDrawer : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Vector3[] pathPoints; // 预定义的轨迹点

    void Start()
    {
        lineRenderer.positionCount = pathPoints.Length;
        lineRenderer.SetPositions(pathPoints);
    }
}