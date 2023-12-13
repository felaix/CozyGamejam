using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float smoothSpd = .125f;
    [SerializeField] private Vector3 offset = new(0f, -3.5f, 0f);

    private void Update()
    {
        if (player == null) return;

        //Vector3 desiredPos = new(player.position.x, player.position.y, transform.position.z);

        //transform.position = desiredPos + offset;

        // player.position.y + offset.y
        Vector3 desiredPos = new(player.position.x, player.position.y, transform.position.z);
        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos + offset, smoothSpd);
        transform.position = smoothedPos;
    }
}
