using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Windows;

public class Movement : MonoBehaviour
{

    [SerializeField] private GameObject footprintsPrefab;

    [SerializeField] private float spd = 5f;
    [SerializeField] private float footprintSpawnInterval = .3f;
    [SerializeField] private Transform _stepSpawn;

    // ! total dumm das hier zu machen aber hat zeitgründe
    [Header("!!! muss noch raus aus movement !!!")]
    [SerializeField] private GameObject restartBtn;
    [SerializeField] private GameObject skipBtn;

    private InputActions input;
    private Vector2 movementInput;

    private Animator animator;

    private float timer = 0f;
    private bool isMoving = false;



    private void OnEnable()
    {
        if (input == null)
        {
            input = new InputActions();
            input.Player.Move.performed += OnMove;
            input.Player.Move.canceled += OnMoveCanceled;
            input.Player.Restart.performed += RestartLevel;
            input.Player.Skip.performed += Skiplevel;
            //input.UI.Navigate.performed
            input.Enable();
        }

        animator = GetComponent<Animator>();
    }

    private void Skiplevel(InputAction.CallbackContext context)
    {
        OutlineManager.Instance.ShowScore();
    }

    private void RestartLevel(InputAction.CallbackContext context)
    {
        CallbackManager.Instance.OnLevelReset?.Invoke(LevelSelectionData.Instance.CurrentLevelData);
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        movementInput = Vector2.zero;
        isMoving = false;
        //Debug.Log("stopped moving");
        UpdateAnimator();

    }

    private void OnDisable()
    {
        input.Player.Move.performed -= OnMove;
        input.Player.Move.canceled -= OnMoveCanceled;
        input.UI.Cancel.performed -= RestartLevel;
        input.UI.Submit.performed -= Skiplevel;
        input.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
        isMoving = true;
        //Debug.Log("is moving");
        UpdateAnimator();

    }

    private void Move()
    {
        Vector3 movementVector = new Vector3(movementInput.x, movementInput.y, 0);
        transform.position += movementVector.normalized * spd * Time.deltaTime;
        //UpdateAnimator();

    }

    private void UpdateAnimator()
    {
        //Debug.Log("idle: " + !isMoving);

        animator.SetFloat("Horizontal", movementInput.x);
        animator.SetFloat("Vertical", movementInput.y);
        animator.SetBool("Idle", !isMoving);
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
