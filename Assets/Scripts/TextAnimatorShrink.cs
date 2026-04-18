using UnityEngine;
using TMPro;
using PrimeTween;

public class TextAnimatorShrink : MonoBehaviour
{
    [SerializeField]
    private float _delayBeforeShrink = 2f;

    [SerializeField]
    private float _shrinkDuration = 2f;

    [SerializeField]
    private float _shrinkToSize = 100f;

    [SerializeField]
    private RectTransform _shrinkedPosition;

    [SerializeField]
    private TextMeshProUGUI _text;

    private void Start()
    {
        Tween.Delay(_delayBeforeShrink, () =>
        {
            Tween.TextFontSize(_text, _text.fontSize, _shrinkToSize, _shrinkDuration);
            Tween.UIAnchoredPosition(_text.rectTransform, new Vector2(_shrinkedPosition.localPosition.x, _shrinkedPosition.localPosition.y), _shrinkDuration);
        });
    }
}
