using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{

    [SerializeField] private GameObject footprintsPrefab;

    [SerializeField] private float spd = 5f;
    [SerializeField] private float footprintSpawnInterval = 1f;

    private DefaultInputActions input;
    private Vector2 movementInput;

    private float timer = 0f;
    private bool isMoving = false;



    private void OnEnable()
    {
        if (input == null)
        {
            input = new DefaultInputActions();
            input.Player.Move.performed += OnMove;
            input.Player.Move.canceled += OnMoveCanceled;
            input.Enable();
        }
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        movementInput = Vector2.zero;
        isMoving = false;
    }

    private void OnDisable()
    {
        input.Player.Move.performed -= OnMove;
        input.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
        isMoving = true;
    }

    private void Move()
    {
        Vector3 movementVector = new Vector3(movementInput.x, movementInput.y, 0);
        transform.position += movementVector * spd * Time.deltaTime;
    }

    private void InstantiateFootprint()
    {
        if (footprintsPrefab == null) return;
        if (!isMoving) return;

        float angle = Mathf.Atan2(movementInput.y, movementInput.x) * Mathf.Rad2Deg;
        Instantiate(footprintsPrefab, transform.position, Quaternion.Euler(0f, 0f, angle));
    }

    private void Update()
    {

        timer += Time.deltaTime;

        if (timer >= footprintSpawnInterval)
        {
            InstantiateFootprint();
            timer = 0;
        }

        Move();
    }


}
