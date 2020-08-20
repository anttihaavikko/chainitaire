using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    public Card cardPrefab;
    public Transform spawnPoint;
    public Sprite[] cardSprites;
    public Dude dude;
    public Image currentIndicator, indicatorSuit;
    public Color red;
    public Board board;
    public Appearer moveHelp;

    private Stack<SuitAndValue> deck;

    // Start is called before the first frame update
    void Start()
    {
        deck = new Stack<SuitAndValue>();

        var cards = new List<SuitAndValue>();
        for(int s = 0; s < 4; s++)
        {
            for (int v = 0; v < 13; v++)
            {
                cards.Add(new SuitAndValue(s, v));
            }
        }

        cards.OrderBy(c => Random.value).ToList().ForEach(c => deck.Push(c));

        AddCard();
    }

    // Update is called once per frame
    void Update()
    {
        if(Application.isEditor)
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadSceneAsync("Main");
            }
        }
    }

    public void AddCard()
    {
        if (!deck.Any()) return;

        var c = deck.Pop();
        var card = Instantiate(cardPrefab, transform);
        card.transform.localPosition = Vector3.zero;
        card.deck = this;
        var spr = cardSprites[c.suit * 13 + c.value];
        card.SetSuitAndValue(c.suit, c.value, spr);
        //Debug.Log("Now adding " + c.suit + " => " + c.value);
        Tweener.Instance.MoveTo(card.transform, spawnPoint.position, 0.2f, 0, TweenEasings.BounceEaseOut);

        if (dude.Matches(card))
        {
            this.StartCoroutine(() =>
            {
                EffectManager.Instance.AddEffect(0, spawnPoint.position);
            }, 0.2f);
        }
    }

    public void SetPreview(Card c)
    {
        var s = c.GetSuit();
        currentIndicator.sprite = cardSprites[s * 13 + c.GetValue() - 1];
        indicatorSuit.color = s == 0 || s == 3 ? Color.black : red;
    }

    public void HideHelp()
    {
        moveHelp.Hide();
    }
}

public class SuitAndValue
{
    public int suit;
    public int value;

    public SuitAndValue(int s, int v)
    {
        suit = s;
        value = v;
    }
}
