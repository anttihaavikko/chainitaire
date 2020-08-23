using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroHops : MonoBehaviour
{
    public Transform dude;
    public Animator anim;
    public Transform[] points;

    private int spot;
    private int direction = -1;
	private float dudeSize = 0.4874408f;

    // Start is called before the first frame update
    void Start()
    {
		Invoke("Hop", Random.Range(1f, 2f));
	}

    void Hop()
    {
        if (spot == 0 || spot == points.Length - 1)
            direction = -direction;

        spot += direction;

        dude.localScale = new Vector3(dudeSize * direction, dudeSize, dudeSize);

        Tweener.Instance.MoveTo(dude, points[spot].position, 0.25f, 0.06f, TweenEasings.QuadraticEaseInOut);
        anim.SetTrigger("jump");

        JumpSound(0.25f);
        this.StartCoroutine(() => LandSound(0.25f), 0.5f);

        Invoke("Hop", Random.Range(1f, 2f));
	}

    public void JumpSound(float volume = 1f)
    {
        AudioManager.Instance.PlayEffectAt(13, dude.position, 0.94f * volume);
        AudioManager.Instance.PlayEffectAt(14, dude.position, 1.091f * volume);
        AudioManager.Instance.PlayEffectAt(20, dude.position, 1.657f * volume);
        AudioManager.Instance.PlayEffectAt(2, dude.position, 1f);
    }

    public void LandSound(float volume = 1f)
    {
        AudioManager.Instance.PlayEffectAt(15, dude.position, 1.648f * volume);
        AudioManager.Instance.PlayEffectAt(21, dude.position, 1.511f * volume);
        AudioManager.Instance.PlayEffectAt(22, dude.position, 1.474f * volume);
    }
}
