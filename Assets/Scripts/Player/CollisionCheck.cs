using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheck : MonoBehaviour
{
    public List<Collider2D> CollidingObjects { get; private set; } = new();

    public bool IsCurrentCollider(Collider2D col) => CollidingObjects.Contains(col);

    public OutlineManager OutlineManager => OutlineManager.Instance;

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Point"))
        {
            if (!IsCurrentCollider(col))
            {
                CollidingObjects.Add(col);
            }
        }
    }

    public void OnTriggerStay2D(Collider2D col)
    {
        if (IsCurrentCollider(col))
        {
            OutlineManager.CheckDistance(col, transform.position);
        }
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        if (IsCurrentCollider(col))
        {
            CollidingObjects.Remove(col);
        }
    }

    //  wenn step außerhalb eines triggers liegt => penalty
}
