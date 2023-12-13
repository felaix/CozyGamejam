using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{

    [SerializeField] private GameObject footprintsPrefab;

    [SerializeField] private float spd = 5f;
    [SerializeField] private float footprintSpawnInterval = .3f;
    [SerializeField] private Transform footStepSpawnPoint;

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

        //if (GetIsWalkingHorizontal()) { }
        if (!isMoving) { animator.SetBool("Idle", true); Debug.Log("Idle"); return; }
        else
        {
            animator.SetBool("Idle", false);
            animator.SetFloat("Horizontal", movementInput.x);
            animator.SetFloat("Vertical", movementInput.y);
        }

        //animator.SetBool("Horizontal", GetIsWalkingHorizontal());
        //animator.SetInteger("Direction", GetDirection(GetIsWalkingHorizontal()));
        //animator.SetBool("Idle", false);
        //GetDirection(GetIsWalkingHorizontal());

    }

    //private bool GetIsWalkingHorizontal() { return (Mathf.Abs(movementInput.x) > Mathf.Abs(movementInput.y)); }

    //private int GetDirection(bool horizontal) { Debug.Log(horizontal + "movementInputY: " + (int)movementInput.y + "," + "movementInputX: " + (int)movementInput.x); return horizontal ? (int)movementInput.y : (int)movementInput.x; }



    ////{
    ////    Debug.Log("Movement Input Y: " + movementInput.y + "Movement Input X: " + movementInput.x);
    ////    if (horizontal) { Debug.Log("walk left/right"); return movementInput.y > 0 ? 1 : -1; } // 1 for right, -1 for left

    ////    else { Debug.Log("walk up/down"); return movementInput.x > 0 ? 1 : -1; }// 1 for up, -1 for down

    ////}

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

        if (timer >= footprintSpawnInterval && isMoving)
        {
            InstantiateFootprint();
            SoundManager.Instance.PlaySFX("Footsteps");
            timer = 0;
        }

        Move();
    }

    //private void FixedUpdate()
    //{
    //    bool horizontal = GetIsWalkingHorizontal();
    //    GetDirection(horizontal);

    //    if (horizontal)
    //    {
    //        if (movementInput.x == 1) { animator.Play("Player_walkleft"); Debug.Log("walk left"); } else Debug.Log("walk right"); animator.Play("Player_walkright");
    //    }
    //    else
    //    {
    //        if (movementInput.y == 1) { animator.Play("Player_walkup"); Debug.Log("walk up"); } else Debug.Log("walk down"); animator.Play("Player_walkdown");
    //    }
    //}


}
