using System.Collections.Generic;
using UnityEngine;

public class HomeOutlineGrower : MonoBehaviour
{
    public float linePointSpacing = 0.1f; // Spacing between line points
    public KeyCode finalizeShapeKey = KeyCode.Space; // Key to finalize the shape

    private bool shapeFinalized = false;
    private bool isDrawing = false;
    private List<Vector2> points = new List<Vector2>();

    private Camera mainCamera;
    private LineRenderer lineRenderer;
    private float timer = 0f;

    private void Start()
    {
        mainCamera = Camera.main;

        // Create a LineRenderer component and set its initial properties
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
        lineRenderer.loop = false;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
    }

    private void Update()
    {
        if (shapeFinalized)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            isDrawing = true;
            points.Clear();
            AddPoint(GetMouseWorldPosition());
        }
        else if (Input.GetMouseButton(0) && isDrawing)
        {
            timer += Time.deltaTime;
            if (timer >= linePointSpacing)
            {
                AddPoint(GetMouseWorldPosition());
                timer = 0f;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDrawing = false;
        }

        if (Input.GetKeyDown(finalizeShapeKey) && isDrawing)
        {
            FinalizeShape();
        }
    }

    private void AddPoint(Vector2 point)
    {
        points.Add(point);
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPosition(points.Count - 1, point);
    }

    private void FinalizeShape()
    {
        shapeFinalized = true;
        lineRenderer.loop = true;
        lineRenderer.positionCount = points.Count + 1;
        lineRenderer.SetPosition(points.Count, points[0]); // Close the shape by adding the first point as the last point
    }

    private Vector2 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -mainCamera.transform.position.z;
        return mainCamera.ScreenToWorldPoint(mousePosition);
    }
}
