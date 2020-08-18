using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Card : MonoBehaviour
{
    public Deck deck;
    public SpriteRenderer spriteRenderer, suitSprite;
    public SpriteMask spriteMask;
    public SortingGroup sortingGroup;

    private bool holding, locked;
    private Camera cam;
    private Vector3 offset;
    private int suit, value;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (holding)
        {
            Vector3 mp = Input.mousePosition;
            mp.z = 10f;
            Vector3 mouseInWorld = cam.ScreenToWorldPoint(mp);

            transform.position = mouseInWorld + offset;
        }
    }

    internal int GetSuit()
    {
        return suit;
    }

    internal bool IsMatch(int s, int v)
    {
        return s == suit || v == value + 1;
    }

    public void Grab()
    {
        if (locked)
            return;

        holding = true;

        Vector3 mp = Input.mousePosition;
        mp.z = 10f;
        Vector3 mouseInWorld = cam.ScreenToWorldPoint(mp);

        offset = transform.position - mouseInWorld;

        deck.dude.sortingGroup.sortingOrder = -1;
        sortingGroup.sortingOrder = 0;
    }

    public void Drop()
    {
        var p = new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        var hit = Physics2D.OverlapCircleAll(p, 0.1f);

        if(hit.Length > 1)
        {
            Tweener.Instance.MoveTo(transform, deck.spawnPoint.position, 0.2f, 0, TweenEasings.BounceEaseOut);
            holding = false;
            return;
        }

        this.StartCoroutine(deck.dude.TryMove, 0.1f);

        Tweener.Instance.MoveTo(transform, p, 0.1f, 0, TweenEasings.BounceEaseOut);
        holding = false;
        locked = true;

        deck.dude.sortingGroup.sortingOrder = 5;
        sortingGroup.sortingOrder = -2;
    }

    public void SetSuitAndValue(int s, int v, Sprite spr)
    {
        suit = s;
        value = v;
        spriteRenderer.sprite = spr;
        spriteMask.sprite = spr;
        suitSprite.color = s == 0 || s == 3 ? Color.black : deck.red;
    }

    public int GetValue()
    {
        return value + 1;
    }
}
