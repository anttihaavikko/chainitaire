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
        onClick?.Invoke();

        if(!string.IsNullOrEmpty(changeToScene))
            SceneChanger.Instance.ChangeScene(changeToScene);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        button.color = hoverColor;
        Tweener.Instance.ScaleTo(transform, Vector3.one * 1.1f, 0.2f, 0, TweenEasings.BounceEaseOut);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        button.color = color;
        Tweener.Instance.ScaleTo(transform, Vector3.one, 0.2f, 0, TweenEasings.BounceEaseOut);

    }
}
