using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Bonus : MonoBehaviour
{
    public List<SpriteRenderer> sprites;
    public Transform wrapper;

    private Vector3 pos;

    public void SetSprite(Sprite sprite)
    {
        pos = transform.position;
        sprites.ForEach(s => s.sprite = sprite);
    }

    public void Enter()
    {
        pos = transform.position;
        Tweener.Instance.MoveTo(wrapper, pos + Vector3.right * 0.25f, 0.2f, 0, TweenEasings.BounceEaseOut);
    }

    public void Exit()
    {
        Tweener.Instance.MoveTo(wrapper, pos, 0.2f, 0, TweenEasings.BounceEaseOut);
    }
}
