using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryController : MonoBehaviour
{
    public LineRenderer lineRenderer;
    private float trajectoryKoef;
    private Vector3[] points;

    void Start()
    {
        trajectoryKoef = 50f;
        points = new Vector3[25];
        lineRenderer.positionCount = points.Length;
    }


    //показываем траекторию
    public void ShowTrajectory(Vector3 originPos, Vector3 forceVector)
    {
        lineRenderer.enabled = true;
        forceVector /= trajectoryKoef;

        for (int i = 0; i < points.Length; i++)
        {
            float time = i * 0.1f;
            points[i] = (originPos + forceVector * time + time * time * Physics.gravity / 2f);

        }

        lineRenderer.SetPositions(points);
    }

    //прячем траекторию
    public void HideTrajectory()
    {
        lineRenderer.enabled = false;
    }
}
