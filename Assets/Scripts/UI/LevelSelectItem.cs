using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelSelectItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject ImageObj;

    [SerializeField]
    private Button _startBtn;
    [SerializeField]
    private Button _resetBtn;
    [SerializeField]
    private float _animDuration;

    [SerializeField]
    private Image _bgHovered;
    [SerializeField]
    private Image _bgUnhovered;
    private Color NotVisibleColor(Image img) => new(ColorR(img), ColorG(img), ColorB(img), 0);
    private Color VisibleColor(Image img) => new(ColorR(img), ColorG(img), ColorB(img), 1);

    private float ColorR(Image img) => img.color.r;
    private float ColorG(Image img) => img.color.g;
    private float ColorB(Image img) => img.color.b;


    private IEnumerator _scaleAnimation;
    private IEnumerator _bgAnimation;

    private bool HasImageComponent => ImageObj.TryGetComponent<Image>(out Image img);

    private Vector3 HoveredScale => new(1, 1, 1);
    private Vector3 UnhoveredScale => new(0.75f, 0.75f, 0.75f);

    private Image ImageComponent => HasImageComponent? ImageObj.GetComponent<Image>() : null;

    private void SetScale(Vector3 scale) => gameObject.transform.localScale = scale;
    private void SetOutlineImage(LevelData data) { ImageComponent.sprite = data.Image; }

    private void SetCoroutineScale(Vector3 target) { _scaleAnimation = AnimateScale(target); }
    private void SetCoroutineBackground(bool hovered) { _bgAnimation = AnimateBackground(hovered); }

    private void ClearAnimations() { ClearScaleAnim(); ClearBgAnim(); }
    private void ClearScaleAnim() { StopCoroutine(_scaleAnimation); _scaleAnimation = null; }
    private void ClearBgAnim() { StopCoroutine (_bgAnimation); _bgAnimation = null;}

    public void PrepareLevelDisplay(LevelData curData)
    {
        SetScale(UnhoveredScale);
        _bgHovered.color = NotVisibleColor(_bgHovered);
        _bgUnhovered.color = VisibleColor(_bgUnhovered);
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_scaleAnimation != null) ClearAnimations();
        AnimateButton(HoveredScale, true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_scaleAnimation != null) ClearAnimations();
        AnimateButton(UnhoveredScale, false);
    }

    private void AnimateButton(Vector3 targetScale, bool hovered)
    {
        SetCoroutineScale(targetScale);
        SetCoroutineBackground(hovered);

        StartCoroutine(_scaleAnimation);
        StartCoroutine(_bgAnimation);
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

    IEnumerator AnimateBackground(bool hovered)
    {
        float passedTime = 0f;

        while (passedTime <= _animDuration)
        {
            passedTime += Time.deltaTime;
            if (hovered)
            {
                _bgHovered.color = Color.Lerp(_bgHovered.color, VisibleColor(_bgHovered), passedTime);
                _bgUnhovered.color = Color.Lerp(_bgUnhovered.color, NotVisibleColor(_bgUnhovered), passedTime);
            }
            else
            {
                _bgHovered.color = Color.Lerp(_bgHovered.color, NotVisibleColor(_bgHovered), passedTime);
                _bgUnhovered.color = Color.Lerp(_bgUnhovered.color, VisibleColor(_bgUnhovered), passedTime);
            }

            yield return null;
        }

        if (hovered)
        {
            _bgHovered.color = VisibleColor(_bgHovered);
            _bgUnhovered.color = NotVisibleColor(_bgUnhovered);
        }
        else
        {
            _bgHovered.color = NotVisibleColor(_bgHovered);
            _bgUnhovered.color = VisibleColor(_bgUnhovered);
        }
    }
}