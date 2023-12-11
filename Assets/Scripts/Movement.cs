using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    }

    private void OnDisable()
    {
        input.Player.Move.performed -= OnMove;
        input.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    private void Move()
    {
        Vector3 movementVector = new Vector3(movementInput.x, movementInput.y, 0);
        transform.position += movementVector * spd * Time.deltaTime;

        //Invoke(nameof(InstantiateFootprint), 2f);
    }

    private void InstantiateFootprint()
    {
        if (footprintsPrefab == null) return;

        Instantiate(footprintsPrefab, transform.position, Quaternion.identity);
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
