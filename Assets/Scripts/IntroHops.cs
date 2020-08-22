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

		Invoke("Hop", Random.Range(1f, 2f));
	}
}
