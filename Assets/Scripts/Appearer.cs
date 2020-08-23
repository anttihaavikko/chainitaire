using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Appearer : MonoBehaviour
{
	public float appearAfter = -1f;
	public float hideDelay;
    public bool silent;
    public bool hiddenOnWeb;
    public bool soundOnZero;

    public TMP_Text text;
    private Vector3 size;

    // Start is called before the first frame update
    void Start()
    {
        size = transform.localScale;
        transform.localScale = Vector3.zero;

		if (appearAfter >= 0 && (!hiddenOnWeb || Application.platform != RuntimePlatform.WebGLPlayer))
			Invoke("Show", appearAfter);
    }

    private Vector3 SoundPos()
    {
        return soundOnZero ? Vector3.zero : transform.position;
    }

    public void Show()
    {
        if(!silent)
        {
            var p = SoundPos();
            AudioManager.Instance.PlayEffectAt(19, p, 1.431f);
            AudioManager.Instance.PlayEffectAt(18, p, 1.431f);
            AudioManager.Instance.PlayEffectAt(16, p, 1.448f);
        }

        // Debug.Log("Showing " + name);
        Tweener.Instance.ScaleTo(transform, size, 0.3f, 0f, TweenEasings.BounceEaseOut);
    }

    public void Hide()
	{
        CancelInvoke("Show");

        // Debug.Log("Hiding " + name);

        if(!silent)
        {
            var p = SoundPos();
            AudioManager.Instance.PlayEffectAt(19, p, 1.431f);
            AudioManager.Instance.PlayEffectAt(18, p, 1.431f);
            AudioManager.Instance.PlayEffectAt(16, p, 1.448f);
        }

        Tweener.Instance.ScaleTo(transform, Vector3.zero, 0.2f, 0f, TweenEasings.QuadraticEaseOut);
	}

    public void HideWithDelay()
	{
		Invoke("Hide", hideDelay);
	}

    public void ShowWithText(string t, float delay)
    {
        if (text)
            text.text = t;

        Invoke("Show", delay);
    }
}
