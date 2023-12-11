using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outline : MonoBehaviour
{

    public int reductionFactor = 2;
    public GameObject testObj;
    public Transform testParent;

    private Texture2D originalTexture;
    private SpriteRenderer outlineRenderer;

    private void Start()
    {
        outlineRenderer = GetComponent<SpriteRenderer>();
        originalTexture = outlineRenderer.sprite.texture;

        GetSpritePixels();
    }

    private void GetSpritePixels()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Sprite sprite = spriteRenderer.sprite;

        Vector2[] spriteVertices = sprite.vertices;

        for (int i = 0; i < spriteVertices.Length; i += reductionFactor)
        {
            Debug.Log("Sprite vertices: " + spriteVertices.Length);
            Vector3 pixelPosition = spriteRenderer.transform.TransformPoint(spriteVertices[i]);
            Instantiate(testObj, pixelPosition, Quaternion.identity, testParent);
            OutlineManager.Instance.AddPoint(pixelPosition);
        }
    }

}
