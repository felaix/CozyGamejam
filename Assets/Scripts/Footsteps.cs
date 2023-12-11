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

        Debug.Log("vergiss das nicht: UnityEditor.SerializedObject.FindProperty (System.String propertyPath) (at <fe7039efe678478d9c83e73bc6a6566d>:0)");
    }
}
