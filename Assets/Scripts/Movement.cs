using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{

    [SerializeField] private GameObject footprintsPrefab;

    [SerializeField] private float spd = 5f;
    [SerializeField] private float footprintSpawnInterval = .3f;
    [SerializeField] private Transform _stepSpawn;

    private DefaultInputActions input;
    private Vector2 movementInput;

    private Animator animator;

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

        animator = GetComponent<Animator>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        movementInput = Vector2.zero;
        isMoving = false;
        UpdateAnimator();

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
        UpdateAnimator();

    }

    private void Move()
    {
        Vector3 movementVector = new Vector3(movementInput.x, movementInput.y, 0);
        transform.position += movementVector.normalized * spd * Time.deltaTime;
    }

    private void UpdateAnimator()
    {
        animator.SetBool("Idle", !isMoving);
        animator.SetFloat("Horizontal", movementInput.x);
        animator.SetFloat("Vertical", movementInput.y);
    }

    private void InstantiateFootprint()
    {
        if (footprintsPrefab == null) return;
        if (!isMoving) return;

        float angle = Mathf.Atan2(movementInput.y, movementInput.x) * Mathf.Rad2Deg;
        Instantiate(footprintsPrefab, _stepSpawn.position, Quaternion.Euler(0f, 0f, angle-90f));
    }

    private void Update()
    {

        timer += Time.deltaTime;

        if (timer >= footprintSpawnInterval && isMoving)
        {
            InstantiateFootprint();
            SoundManager.Instance.PlaySFX("Footsteps");
            timer = 0;
        }

        Move();
    }
}
