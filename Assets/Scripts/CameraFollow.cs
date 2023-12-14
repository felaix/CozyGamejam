using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float smoothSpd = .125f;
    [SerializeField] private Vector3 offset = new(0f, -3.5f, 0f);
    //private float minY = -6.5f;


    [SerializeField] private float leftBoundary = -3f;
    [SerializeField] private float rightBoundary = 3f;
    [SerializeField] private float topBoundary = 3f;
    [SerializeField] private float bottomBoundary = -3f;

    private void Update()
    {
        if (player == null) return;


        float clampedX = Mathf.Clamp(player.position.x, leftBoundary, rightBoundary);
        float clampedY = Mathf.Clamp(player.position.y, bottomBoundary, topBoundary);


        Vector3 desiredPos = new(clampedX, clampedY, transform.position.z);
        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos + offset, smoothSpd);

        transform.position = smoothedPos;
    }
}
