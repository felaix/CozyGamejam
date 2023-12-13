using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float smoothSpd = .125f;
    [SerializeField] private Vector3 offset = new(0f, -3.5f, 0f);
    private float minY = -6.5f;

    private void Update()
    {
        if (player == null) return;

        Vector3 desiredPos = new(player.position.x, player.position.y, transform.position.z);
        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos + offset, smoothSpd);
        smoothedPos.y = Mathf.Clamp(smoothedPos.y, minY, Mathf.Infinity);
        transform.position = smoothedPos;
    }
}
