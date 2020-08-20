﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class Dude : MonoBehaviour
{
    public Deck deck;
    private int suit = -1, value = -1;
    public Animator anim;
    public GameObject blockPrefab;
    public NumberScroller totalScore, comboScore;
    public SortingGroup sortingGroup;
    public EffectCamera cam;
    public Appearer comboDisplay;
    public SpeechBubble bubble;
    public Appearer holdArea;

    private float scale;
    private int combo = 1;
    private float xmod, ymod;
    private int move;

    private void Start()
    {
        scale = transform.localScale.x;

        this.StartCoroutine(() => ShowText("Hey! Could you build me (a platform) from the (cards)."), 0.3f);
    }

    public void MirrorBubble()
    {
        xmod = -xmod;
        ymod = -ymod;
        bubble.transform.position = transform.position + ymod * 1.9f * Vector3.up + xmod * 2.6f * Vector3.right;
    }

    private void ShowText(string message)
    {
        xmod = transform.position.x > 0 ? -1f : 1f;
        ymod = transform.position.y > 0 ? -1f : 1f;
        bubble.transform.position = transform.position + ymod * 1.9f * Vector3.up + xmod * 2.6f * Vector3.right;
        bubble.ShowMessage(message);
    }

    public bool HasMessage()
    {
        return move <= 5;
    }

    public void HideBubble()
    {
        bubble.Hide();
    }

    public void ShowBubble()
    {
        bubble.Show();
    }

    public void TryMove()
    {
        //Debug.Log("Trying to move @ " + Time.time);

        var neighbors = GetNeighbors().Where(n => n != null && CanJumpTo(n));

        if(neighbors.Any())
        {
            var best = neighbors.OrderBy(c => Random.value).OrderByDescending(c => c.GetValue()).FirstOrDefault();

            if(best)
            {
                Tweener.Instance.MoveTo(transform, best.transform.position, 0.25f, 0.06f, TweenEasings.QuadraticEaseInOut);
                suit = best.GetSuit();
                value = best.GetValue();
                anim.SetTrigger("jump");

                var diff = best.transform.position.x - transform.position.x;
                if (Mathf.Abs(diff) > 0.5f)
                {
                    transform.localScale = new Vector3(Mathf.Sign(diff) * scale, scale, 1f);
                }

                deck.SetPreview(best);

                Invoke("TryMove", 0.7f);

                this.StartCoroutine(() =>
                {
                    comboDisplay.Show();

                    var amt = best.GetValue();
                    comboScore.Add(amt * combo);

                    this.StartCoroutine(() => {
                        EffectManager.Instance.AddText(amt + "<size=3>x" + (combo - 1) + "</size>", transform.position + Vector3.up * 0.2f);
                    }, 0.25f);

                    this.StartCoroutine(() => {
                        cam.BaseEffect(0.1f);
                        EffectManager.Instance.AddEffect(1, transform.position + Vector3.down * 0.5f);
                    }, 0.25f);

                    Instantiate(blockPrefab, best.transform.position, Quaternion.identity);
                    Destroy(best.gameObject);

                    deck.board.Moved();

                    combo++;
                }, 0.3f);

                return;
            }
        }

        var amount = comboScore.GetValue();

        if(amount > 0)
        {
            this.StartCoroutine(() =>
            {
                totalScore.Add(amount);
                comboScore.Clear();
                this.StartCoroutine(comboDisplay.Hide, 0.5f);
            }, 0.7f);
        }

        NextText();

        deck.AddCard();
        combo = 1;

        CheckForEnd();
    }

    void CheckForEnd()
    {
        var spots = GetTakenNeighbors().Where(s => s != null);

        if(spots.Count() == 4)
            Debug.Log("Game Over!");
    }

    void NextText()
    {
        //Debug.Log("Showing message for move " + move);
        string[] suits = { "clubs", "diamonds", "hearts", "spades" };
        string[] values = { "0", "Ace", "2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King" };
        string[] pres = { "", "an", "a", "a", "a", "a", "a", "a", "an", "a", "a", "a", "a", "a" };
        var color = suit == 0 || suit == 3 ? Color.black : deck.red;
        var s = TextUtils.TextWith(suits[suit], color);
        var v = TextUtils.TextWith(values[value], color);

        if (move == 0 || move == 1)
            this.StartCoroutine(() => ShowText("I can only step on " + s + " or " + pres[value] + " " + v + " now!"), 0.3f);

        if (move == 2)
            this.StartCoroutine(() => ShowText("You can also see the matching card on (top left corner) of the screen!"), 0.3f);

        if (move == 3)
            this.StartCoroutine(() => ShowText("The (more moves) I do at one placement, the (more points) you get!"), 0.3f);

        if (move == 4)
        {
            this.StartCoroutine(() => ShowText("You can place cards (on hold) if you wish to save them for later!"), 0.3f);
            holdArea.Show();
        }

        if (move == 5)
            this.StartCoroutine(() => ShowText("Good luck!"), 0.3f);

        move++;
    }

    private bool CanJumpTo(Card card)
    {
        if(suit == -1 || value == -1)
        {
            return true;
        }

        return Matches(card);
    }

    public bool Matches(Card c)
    {
        return c.IsMatch(suit, value);
    }

    private List<Card> GetNeighbors()
    {
        var all = new List<Card>
        {
            GetNeighbor(Vector3.up),
            GetNeighbor(Vector3.down),
            GetNeighbor(Vector3.right),
            GetNeighbor(Vector3.left)
        };

        return all;
    }

    private Card GetNeighbor(Vector3 dir)
    {
        var hit = Physics2D.OverlapCircle(transform.position + dir, 0.1f);

        if(hit)
        {
            return hit.GetComponent<Card>();
        }

        return null;
    }

    private List<GameObject> GetTakenNeighbors()
    {
        var all = new List<GameObject>
        {
            GetTakenNeighbor(Vector3.up),
            GetTakenNeighbor(Vector3.down),
            GetTakenNeighbor(Vector3.right),
            GetTakenNeighbor(Vector3.left)
        };

        return all;
    }

    private GameObject GetTakenNeighbor(Vector3 dir)
    {
        var hit = Physics2D.OverlapCircle(transform.position + dir, 0.1f);

        if (hit)
        {
            return hit.gameObject;
        }

        return null;
    }
}
