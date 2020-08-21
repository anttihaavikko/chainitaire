using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
    private Bonus bonus;

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

        deck.dude.HideBubble();

        holding = true;

        Vector3 mp = Input.mousePosition;
        mp.z = 10f;
        Vector3 mouseInWorld = cam.ScreenToWorldPoint(mp);

        offset = transform.position - mouseInWorld;

        deck.dude.sortingGroup.sortingOrder = -1;
        sortingGroup.sortingOrder = 0;

        deck.board.ShowMarkers();
    }

    public void Enter()
    {
        if(bonus)
        {
            bonus.Enter();
        }
    }

    public void Exit()
    {
        if (bonus)
        {
            bonus.Exit();
        }
    }

    private void MoveTo(Vector3 pos)
    {
        Tweener.Instance.MoveTo(transform, pos, 0.2f, 0, TweenEasings.BounceEaseOut);
        this.StartCoroutine(() => EffectManager.Instance.AddEffect(2, pos), 0.05f);
    }

    public void Drop()
    {
        if (locked)
            return;

        var p = new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        var hit = Physics2D.OverlapCircle(p, 0.1f, deck.board.markerLayer);

        deck.board.HideMarkers();

        if(hit && hit.gameObject.tag == "Hold")
        {
            if (deck.held)
            {
                if(deck.held == this)
                {
                    MoveTo(hit.transform.position);
                }
                else
                {
                    MoveTo(deck.spawnPoint.position);
                }

                holding = false;

                if(deck.dude.HasMessage())
                    deck.dude.ShowBubble();

                return;
            }

            MoveTo(deck.spawnPoint.position);
            deck.held = this;
            PostDrop(hit.transform.position);
            deck.AddCard();
            return;
        }

        if (!hit)
        {
            if (deck.held == this)
            {
                MoveTo(deck.heldPoint.position);
            }
            else
            {
                MoveTo(deck.spawnPoint.position);
            }

            holding = false;

            if(deck.dude.HasMessage())
                deck.dude.ShowBubble();

            return;
        }

        if (deck.held == this)
            deck.UseHeld();

        bonus = Physics2D.OverlapCircle(p, 0.1f, deck.board.bonusLayer)?.GetComponent<Bonus>();

        if(!bonus)
        {
            var trigger = GetComponent<EventTrigger>();
            Destroy(trigger);
        }

        PostDrop(p);

        deck.HideHelp();

        deck.board.DeactivateMarker(hit.gameObject);

        this.StartCoroutine(deck.dude.TryMove, 0.1f);

        deck.dude.sortingGroup.sortingOrder = 5;
        sortingGroup.sortingOrder = -2;

        locked = true;
    }

    void PostDrop(Vector3 p)
    {
        MoveTo(p);
        holding = false;
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

    public bool HasBonus()
    {
        return bonus != null;
    }
}
