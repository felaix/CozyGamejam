using UnityEngine;

public class Footsteps : MonoBehaviour
{
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        OutlineManager.Instance.AddFootStep(transform.position);
    }
}
