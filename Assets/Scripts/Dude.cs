using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Dude : MonoBehaviour
{
    public Deck deck;
    private int suit = -1, value = -1;
    public Animator anim;
    public GameObject blockPrefab;
    public NumberScroller totalScore, comboScore;

    private float scale;
    private int combo = 1;

    private void Start()
    {
        scale = transform.localScale.x;
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
                    var amt = best.GetValue();
                    comboScore.Add(amt * combo);

                    this.StartCoroutine(() => {
                        EffectManager.Instance.AddText(amt + "<size=3>x" + (combo - 1) + "</size>", transform.position + Vector3.up * 0.2f);
                    }, 0.25f);

                    Instantiate(blockPrefab, best.transform.position, Quaternion.identity);
                    Destroy(best.gameObject);

                    combo++;
                }, 0.3f);

                return;
            }
        }

        var amount = comboScore.GetValue();

        if(amount > 0)
        {
            totalScore.Add(amount);
            comboScore.Clear();
            //this.StartCoroutine(() => comboScore.Clear(), 0.2f);
        }

        deck.AddCard();
        combo = 1;
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
}
