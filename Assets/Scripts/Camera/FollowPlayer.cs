 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FollowPlayer : MonoBehaviour
{
    [Header("Parameters")]
    public List<Transform> targets;
    public Vector3 offset;
    public float smoothTime = 0.5f;
    public float minFieldOfView = 10f;
    public float maxFieldOfView = 40f;
    public float zoomLimiter = 2f;

    private bool maxFieldOfViewReached;
    private Vector3 velocity;
    private Camera cCamera;
    private void Start()
    {
        cCamera = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if(targets.Count == 0) return;

        Bounds bounds = GetBounds();

        Follow(ref bounds);
        Zoom(ref bounds);
    }

    private void Follow(ref Bounds bounds)
    {
        if(maxFieldOfViewReached) return;

        Vector3 centerPoint = GetCenterPoint(ref bounds);
        Vector3 newPosition = centerPoint + offset;
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    private void Zoom(ref Bounds bounds)
    {
        float newFieldOfView = Mathf.Lerp(minFieldOfView, maxFieldOfView, GetGreatestDistance(ref bounds)/zoomLimiter);

        if (newFieldOfView >= maxFieldOfView)
        {
            newFieldOfView = maxFieldOfView;
            maxFieldOfViewReached = true;
        }
        else
        {
            maxFieldOfViewReached = false;
        }

        cCamera.fieldOfView = Mathf.Lerp(cCamera.fieldOfView, newFieldOfView, Time.deltaTime);
    }

    private Bounds GetBounds()
    {
        Bounds bounds = new Bounds(targets[0].position, Vector3.zero);
        for(int i=0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        return bounds;
    }

    private Vector3 GetCenterPoint(ref Bounds bounds)
    {
        return bounds.center;
    }

    private float GetGreatestDistance(ref Bounds bounds)
    {
        return bounds.size.x > bounds.size.z ? bounds.size.x : bounds.size.z;
    }

}
