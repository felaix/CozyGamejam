using UnityEngine;

public class Outline : MonoBehaviour
{
    [SerializeField]
    private GameObject _pointToSpawn;
    [SerializeField]
    private Transform _outlineParent;
    [SerializeField]
    private SpriteRenderer _displayRenderer;
    [SerializeField]
    private SpriteRenderer _codeRenderer;

    private LevelController _levelControls;
    private OutlineManager _outlineManager;

    public int reductionFactor = 5;

    private void Start()
    {
        GetInstances();

        _codeRenderer.sprite = _levelControls.ImageToCode;
        _displayRenderer.sprite = _levelControls.ImageToDisplay;

        SpawnVectorPoints();
    }

    private void GetInstances()
    {
        if (LevelController.Instance == null) Debug.LogError("ERROR: LevelController not in Scene");
        else _levelControls = LevelController.Instance;
        if (OutlineManager.Instance == null) Debug.LogError("ERROR: OutlineManager not in Scene");
        else _outlineManager = OutlineManager.Instance;
    }

    private void SpawnVectorPoints()
    {
        // Get the vertices of the sprite
        Vector2[] spriteVertices = _codeRenderer.sprite.vertices;
        //      Debug.Log($"Vertices: {spriteVertices.Length}");

        // ! Instantiate an object for each vertices
        // ! reductionFactor to reduce the amount of objects
        for (int i = 0; i < spriteVertices.Length; i += reductionFactor)
        {
            SpawnVectorPoint(spriteVertices[i]);
        }
    }

    private void SpawnVectorPoint(Vector2 point)
    {
        //      Debug.Log("Sprite vertices: " + spriteVertices.Length);

        Vector3 pixelPosition = _codeRenderer.transform.TransformPoint(point);

        Instantiate(_pointToSpawn, pixelPosition, Quaternion.identity, _outlineParent);
        _outlineManager.AddPoint(pixelPosition);
    }
}
