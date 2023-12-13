using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class LevelSelectItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject ImageObj;

    [SerializeField]
    private Button _startBtn;
    [SerializeField]
    private Button _resetBtn;
    [SerializeField]
    private float _animDuration;

    private IEnumerator _scaleAnimation;

    private bool HasImageComponent => ImageObj.TryGetComponent<Image>(out Image img);

    private Vector3 HoveredScale => new(1, 1, 1);
    private Vector3 UnhoveredScale => new(0.75f, 0.75f, 0.75f);

    private Image ImageComponent => HasImageComponent? ImageObj.GetComponent<Image>() : null;

    private void SetScale(Vector3 scale) => gameObject.transform.localScale = scale;
    private void SetOutlineImage(LevelData data) { ImageComponent.sprite = data.Image; }

    public void PrepareLevelDisplay(LevelData curData)
    {
        SetScale(UnhoveredScale);
        SetOutlineImage(curData);
        SetButtons(curData);
    }

    private void SetButtons(LevelData data)
    {
        _startBtn.onClick.RemoveAllListeners();
        _startBtn.onClick.AddListener(() =>
        {
            GameManager.Instance.LoadLevel(data);
        });

        _resetBtn.onClick.RemoveAllListeners();
        _resetBtn.onClick.AddListener(() =>
        {
            CallbackManager.Instance.OnLevelReset?.Invoke(data);
        });
    }

    private void SetCoroutine(Vector3 target) { _scaleAnimation = AnimateScale(target); }
    private void ClearAnimation() { StopCoroutine(_scaleAnimation); _scaleAnimation = null; }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_scaleAnimation != null) ClearAnimation();
        SetCoroutine(HoveredScale);
        StartCoroutine(_scaleAnimation);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_scaleAnimation != null) ClearAnimation();
        SetCoroutine(UnhoveredScale);
        StartCoroutine(_scaleAnimation);
    }

    IEnumerator AnimateScale(Vector3 target)
    {
        float passedTime = 0f;

        while (passedTime <= _animDuration)
        {
            passedTime += Time.deltaTime;
            
            transform.localScale = Vector3.Lerp(transform.localScale, target, passedTime);
            yield return null;
        }
        SetScale(target);
    }
}