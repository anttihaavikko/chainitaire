using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TextButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public TMPro.TMP_Text button;
    public Color hoverColor;
    public string changeToScene;
    public UnityEvent onClick;

    private Color color;

    void Start()
    {
        color = button.color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Instance.PlayEffectAt(0, Vector3.zero, 1.391f);
        AudioManager.Instance.PlayEffectAt(4, Vector3.zero, 1.373f);
        AudioManager.Instance.PlayEffectAt(7, Vector3.zero, 1.562f);

        onClick?.Invoke();

        if(!string.IsNullOrEmpty(changeToScene))
            SceneChanger.Instance.ChangeScene(changeToScene);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        button.color = hoverColor;
        Tweener.Instance.ScaleTo(transform, Vector3.one * 1.1f, 0.2f, 0, TweenEasings.BounceEaseOut);

        AudioManager.Instance.PlayEffectAt(21, Vector3.zero, 1.356f);
        AudioManager.Instance.PlayEffectAt(20, Vector3.zero, 1.362f);
        AudioManager.Instance.PlayEffectAt(19, Vector3.zero, 1.362f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        button.color = color;
        Tweener.Instance.ScaleTo(transform, Vector3.one, 0.2f, 0, TweenEasings.BounceEaseOut);

        AudioManager.Instance.PlayEffectAt(21, Vector3.zero, 1.356f);
        AudioManager.Instance.PlayEffectAt(20, Vector3.zero, 1.362f);
        AudioManager.Instance.PlayEffectAt(19, Vector3.zero, 1.362f);
    }
}
