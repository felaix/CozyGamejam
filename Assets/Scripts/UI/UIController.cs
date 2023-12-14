//using System;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.InputSystem;

//public class UIController : MonoBehaviour
//{
//    private DefaultInputActions input;

//    private void OnEnable()
//    {
//        if (input != null) return;

//        input = new DefaultInputActions();
//        input.UI.Navigate.performed += OnNavigate;
//        input.UI.Submit.performed += OnSubmit;
//        input.Enable();
//    }

//    private void OnDisable()
//    {
//        input.UI.Navigate.performed -= OnNavigate;
//        input.UI.Submit.performed -= OnSubmit;
//        input.Disable();
//    }

//    private void OnSubmit(InputAction.CallbackContext context)
//    {
//        GameObject selectedObject = EventSystem.current.currentSelectedGameObject;

//        if (selectedObject != null)
//        {
//            Debug.Log("Submit: " + selectedObject.name);
//        }
//    }

//    private void OnNavigate(InputAction.CallbackContext context)
//    {
//        Vector2 navigateInput = context.ReadValue<Vector2>();

//        GameObject selectedObject = SelectableNavigation(navigateInput);
//        Debug.Log("Selected Object: " + (selectedObject ? selectedObject.name : "None"));

//        EventSystem.current.SetSelectedGameObject(SelectableNavigation(navigateInput));
//    }

//    private GameObject SelectableNavigation(Vector2 navigateInput) 
//    {
//        return EventSystem.current.currentSelectedGameObject;
//    }
//}
