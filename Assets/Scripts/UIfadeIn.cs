using UnityEngine;
using DG.Tweening;

public class UIfadeIn : MonoBehaviour
{
    [SerializeField] CanvasGroup _uiFadeGroup;

    private Tween _fadeTween;

    public void FadeIn()
    {
        if (_fadeTween != null)
        {
            _fadeTween.Kill();
        }
        _fadeTween = _uiFadeGroup.DOFade(1, 1);
    }

    public void FadeOut()
    {
        if (_fadeTween != null)
        {
            _fadeTween.Kill();
        }
        _fadeTween = _uiFadeGroup.DOFade(0, 1);
    }
}
