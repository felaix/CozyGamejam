using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheck : MonoBehaviour
{
    private Collider2D _curCollider;
    private Collider2D _lastCollider;

    public bool IsCurrentCollider(Collider2D col) => col == _curCollider;
    public bool IsLastCollider(Collider2D col) => col == _lastCollider;
    public bool IsFirstCollider() => _lastCollider == null;

    public OutlineManager OutlineManager => OutlineManager.Instance;

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Point"))
        {
            if (IsFirstCollider() || !IsLastCollider(col))
            {
                _curCollider = col;
            }
        }
    }

    public void OnTriggerStay2D(Collider2D col)
    {
        if (col == _curCollider)
        {
            OutlineManager.CheckDistance(col, transform.position);
        }
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        if (IsCurrentCollider(col))
        {
            _lastCollider = _curCollider;
        }
    }
}
