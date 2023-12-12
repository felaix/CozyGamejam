using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectItem : MonoBehaviour
{
    public GameObject ImageObj;

    [SerializeField]
    private Button _startBtn;
    [SerializeField]
    private Button _resetBtn;

    public void SetButtons(LevelData curData)
    {
        _startBtn.onClick.RemoveAllListeners();
        _startBtn.onClick.AddListener(() =>
        {
            CallbackManager.Instance.OnLoadLevel?.Invoke(curData);
            LevelSelectionWindow.Instance.HideWindow();
        });
    }
}