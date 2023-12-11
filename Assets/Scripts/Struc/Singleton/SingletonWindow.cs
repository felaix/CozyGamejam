using System.Collections;
using UnityEngine;

public class SingletonWindow<T> : Singleton<T> where T: MonoBehaviour
{
    public bool IsVisible => _panel.gameObject.activeInHierarchy;
    
    protected Transform _panel;

    public GameObject FirstSelected => _firstSelected;
    protected GameObject _firstSelected;


    protected override void Awake()
    {
        base.Awake();
        if (transform == null/* || transform.childCount == 0*/)
        {
            Debug.LogError("Window has no Panel - required: adding empty gameobject as child");
        }

        _panel = transform.GetChild(0);
    }

    public virtual void ToggleWindowVisible()
    {
        if (IsVisible) HideWindow();
        else ShowWindow();
    }

    public virtual void ShowWindow()
    {
        _panel.gameObject.SetActive(true);
    }

    public virtual void HideWindow()
    {
        _panel.gameObject.SetActive(false);
    }

    private IEnumerator SetFirstButton()
    {
        yield return new WaitForSeconds(0.1f);

        EventSystemHandler.Instance.SetFirstButton(_firstSelected);
    }
}
