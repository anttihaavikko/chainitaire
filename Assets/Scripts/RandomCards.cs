using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCards : MonoBehaviour
{
    public Sprite[] sprites;
    public SpriteRenderer[] cards;

    // Start is called before the first frame update
    void Start()
    {
        foreach(SpriteRenderer c in cards)
        {
            c.sprite = sprites[Random.Range(0, sprites.Length)];
        }
    }
}
