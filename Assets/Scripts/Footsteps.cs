using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    private SpriteRenderer sr;
    private Collider2D _collider;

    private OutlineManager _outlineM;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        _outlineM = OutlineManager.Instance;
        _outlineM.AddFootStep(transform.position);

        Bounds bounds = _collider.bounds;

        Collider2D[] collidingObjects = Physics2D.OverlapBoxAll(bounds.center, bounds.size, 0f, LayerMask.GetMask("OutlinePoint"));

        if (collidingObjects.Length > 0 )
        {
            foreach (Collider2D collider in collidingObjects)
            {
                if (!PointIsRegistered(collider) && IsCollidingWithPlayer(collider))
                {
                    _outlineM.AddOutsideOutlineStep(transform.position);
                }
            }
        }
    }

    public bool PointIsRegistered(Collider2D collider) => _outlineM.IsPointRegistered(collider.gameObject.transform.position);
    public bool IsCollidingWithPlayer(Collider2D collider) => _outlineM.IsCollidingWithPlayer(collider);


    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
