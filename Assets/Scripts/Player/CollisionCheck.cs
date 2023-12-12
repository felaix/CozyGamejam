using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheck : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Point")) {
            //Debug.Log("point found");
            OutlineManager.Instance.DoSomething(col, transform.position);
        }
    }
}
