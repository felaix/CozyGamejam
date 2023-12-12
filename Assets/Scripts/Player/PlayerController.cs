using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Movement _movement;

    private void Awake()
    {
        if (TryGetComponent(out Movement move)) _movement = move;
    }
}