using UnityEngine;

public class Outline : MonoBehaviour
{

    public int reductionFactor = 5;
    public GameObject testObj;
    public Transform testParent;

    //private Texture2D originalTexture;
    private SpriteRenderer outlineRenderer;

    private void Start()
    {
        outlineRenderer = GetComponent<SpriteRenderer>();
        //outlineRenderer = LevelController.Instance.
        //outlineRenderer.sprite = LevelController.

        //originalTexture = outlineRenderer.sprite.texture;

        outlineRenderer.sprite = LevelController.Instance.CurImage;

        GetSpritePixels();
    }



    private void GetSpritePixels()
    {
        // Get SR
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        // Get current sprite
        Sprite sprite = spriteRenderer.sprite;

        // Get the vertices of the sprite
        Vector2[] spriteVertices = sprite.vertices;
        //Debug.Log($"Vertices: {spriteVertices.Length}");

        // ! Instantiate an object for each vertices
        // ! reductionFactor to reduce the amount of objects
        for (int i = 0; i < spriteVertices.Length; i += reductionFactor)
        {
            //Debug.Log("Sprite vertices: " + spriteVertices.Length);
            Vector3 pixelPosition = spriteRenderer.transform.TransformPoint(spriteVertices[i]);
            Instantiate(testObj, pixelPosition, Quaternion.identity, testParent);
            OutlineManager.Instance.AddPoint(pixelPosition);
        }
    }

}
