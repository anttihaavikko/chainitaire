using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinders : MonoBehaviour
{
    public Transform left, right;
    public bool startsOpen, openAtStart = true;

    private float duration = 0.3f;
    private bool isOpen;

    // Start is called before the first frame update
    void Start()
    {
        isOpen = startsOpen;

        if (startsOpen) return;

        left.transform.localScale = new Vector3(1f, 1f, 1f);
        right.transform.localScale = new Vector3(1f, 1f, 1f);

        if(openAtStart)
            Invoke("Open", 0.5f);
    }

    public void Close()
    {
        if (!isOpen) return;

        Tweener.Instance.ScaleTo(left, Vector3.one, duration, 0f, TweenEasings.BounceEaseOut);
        Tweener.Instance.ScaleTo(right, Vector3.one, duration, 0f, TweenEasings.BounceEaseOut);

        if(AudioManager.Instance)
        {
            AudioManager.Instance.PlayEffectAt(18, Vector3.zero, 0.302f);
            AudioManager.Instance.PlayEffectAt(19, Vector3.zero, 0.514f);
        }

        Invoke("Clang", duration * 0.9f);

        isOpen = false;
    }

    public void Open()
    {
        Tweener.Instance.ScaleTo(left, new Vector3(0f, 1f, 1f), duration, 0f, TweenEasings.BounceEaseOut);
        Tweener.Instance.ScaleTo(right, new Vector3(0f, 1f, 1f), duration, 0f, TweenEasings.BounceEaseOut);

        if(AudioManager.Instance)
        {
            AudioManager.Instance.PlayEffectAt(18, Vector3.zero, 0.302f);
            AudioManager.Instance.PlayEffectAt(19, Vector3.zero, 0.514f);
        }

        isOpen = true;
    }

    public float GetDuration()
    {
        return duration;
    }

    void Clang()
    {
        if (AudioManager.Instance)
        {
            AudioManager.Instance.PlayEffectAt(34, Vector3.zero, 1.322f);
            AudioManager.Instance.PlayEffectAt(33, Vector3.zero, 1.273f);
            AudioManager.Instance.PlayEffectAt(31, Vector3.zero, 1.192f);
        }

    }
}
