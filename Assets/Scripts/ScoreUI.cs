using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{

    [SerializeField] private GameObject selectionBtn;
    [SerializeField] private List<Button> buttonsToDeactivate;


    private void OnEnable()
    {
        MakeButtonsNotInteractable();
        EventSystemHandler.Instance.SetSelected(selectionBtn);
        Invoke(nameof(SetSelectedInvoke), .1f);
        GameManager.Instance.PauseGame(true);
    }

    private void OnDisable()
    {
        MakeButtonsInteractable();
    }

    private void MakeButtonsNotInteractable()
    {
        foreach (var button in buttonsToDeactivate)
        {
            button.interactable = false;
        }
    }

    private void MakeButtonsInteractable()
    {
        foreach (var button in buttonsToDeactivate)
        {
            button.interactable = true;
        }
    }

    private void Start()
    {
        EventSystemHandler.Instance.SetSelected(selectionBtn);
    }

    public void Unpause()
    {
        Time.timeScale = 1f;
    }

    private void SetSelectedInvoke()
    {
        EventSystemHandler.Instance.SetSelected(selectionBtn);
    }
}
