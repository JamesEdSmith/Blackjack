using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public const int BLACK_JOKER = 52;
    public const int RED_JOKER = 53;
    public const int CARD_BACK = 54;

    private const int BUTTON_PADDING = 20;

    public int cardSpace = 20;
    public int dealerHandY = 100;
    public int playerHandY = -100;

    public GameObject cardObjectPrefab;
    public Canvas canvas;
    public Button dealButton;
    private Text dealButtonText;
    public Button standButton;

    private Vector2 dealButtonNewPosition;
    private Vector2 standButtonNewPosition;

    public float buttonTotalTime;
    private float buttonTimer;

    private Sprite[] cardSprites;
    private List<Card> deck;
    private List<Card> cards;
    private List<CardObject> dealerHand;
    private List<CardObject> playerHand;
    private List<GameObject> cardGameObjects;

    public enum Suit
    {
        clubs, diamonds, hearts, spades
    }

    private const int STATE_START = 0;
    private const int STATE_CONTINUE = 1;
    private const int STATE_END = 2;

    int gameState = 0;

    // Start is called before the first frame update
    void Start()
    {
        cardSprites = Resources.LoadAll<Sprite>("cards");
        //8 decks of cards
        cards = new List<Card>(52);
        deck = new List<Card>(416);
        dealerHand = new List<CardObject>(10);
        playerHand = new List<CardObject>(10);
        cardGameObjects = new List<GameObject>(42);

        for (int suit = 0; suit < 4; suit++)
        {
            for (int value = 1; value < 13; value++)
            {
                cards.Add(new Card((Suit)suit, value));
            }
        }

        fillDeck();

        //create cardobject pool
        for (int i = 0; i < 42; i++)
        {
            GameObject gameObject = Instantiate(cardObjectPrefab, canvas.transform);
            gameObject.SetActive(false);
            gameObject.GetComponent<CardObject>().gameManager = this;
            cardGameObjects.Add(gameObject);
        }

        dealButtonText = dealButton.GetComponentInChildren<Text>();
        Vector2 position = dealButton.gameObject.GetComponent<RectTransform>().anchoredPosition;
        dealButtonNewPosition = new Vector2(position.x, position.y);
        position = standButton.gameObject.GetComponent<RectTransform>().anchoredPosition;
        standButtonNewPosition = new Vector2(position.x, position.y);

    }

    private void fillDeck()
    {
        deck.Clear();
        for (int i = 0; i < 8; i++)
        {
            foreach (Card card in cards)
            {
                deck.Add(card);
            }
        }
    }

    public void standPressed()
    {

    }

    public void dealPressed()
    {
        if (gameState == STATE_START)
        {
            dealButtonText.text = "Hit";
            standButton.gameObject.SetActive(true);
            buttonTimer = 0;

            float x = dealButton.gameObject.GetComponent<RectTransform>().anchoredPosition.x + dealButton.gameObject.GetComponent<RectTransform>().sizeDelta.x * 0.5f + BUTTON_PADDING * 0.5f;
            dealButtonNewPosition = new Vector2(x, dealButton.gameObject.GetComponent<RectTransform>().anchoredPosition.y);
      
            x = standButton.gameObject.GetComponent<RectTransform>().anchoredPosition.x - standButton.gameObject.GetComponent<RectTransform>().sizeDelta.x * 0.5f - BUTTON_PADDING * 0.5f;
            standButtonNewPosition = new Vector2(x, standButton.gameObject.GetComponent<RectTransform>().anchoredPosition.y);

            gameState = STATE_CONTINUE;

            while (dealerHand.Count > 0)
            {
                dealerHand[0].gameObject.SetActive(false);
                dealerHand.RemoveAt(0);
            }

            while (playerHand.Count > 0)
            {
                playerHand[0].gameObject.SetActive(false);
                playerHand.RemoveAt(0);
            }

            dealerHand.Add(dealCard());
            dealerHand.Add(dealCard());
            dealerHand[1].setFlipped(true);

            playerHand.Add(dealCard());
            playerHand.Add(dealCard());

            adjustPlayerHand(dealerHand, dealerHandY);
            adjustPlayerHand(playerHand, playerHandY);

        }
        else if (gameState == STATE_CONTINUE)
        {
            playerHand.Add(dealCard());
            adjustPlayerHand(playerHand, playerHandY);
        }

    }

    private void adjustPlayerHand(List<CardObject> playerHand, int handY)
    {
        float cardStartX = playerHand.Count * cardSpace * -0.5f + cardSpace * 0.5f;
        foreach (CardObject cardObject in playerHand)
        {
            setAnchoredPosition(cardObject.gameObject, cardStartX + cardSpace * playerHand.IndexOf(cardObject), handY);
        }
    }

    private void setAnchoredPosition(GameObject gObject, float x, int y)
    {
        RectTransform rectTransform = gObject.GetComponent<RectTransform>();
        Vector3 pos = rectTransform.anchoredPosition;
        pos.x = x;
        pos.y = y;
        rectTransform.anchoredPosition = pos;
    }

    private CardObject dealCard()
    {
        if (deck.Count < 1)
        {
            fillDeck();
        }

        int cardIndex = UnityEngine.Random.Range(0, deck.Count - 1);
        GameObject cardGameObject = null;
        foreach (GameObject obj in cardGameObjects)
        {
            if (!obj.activeSelf)
            {
                obj.SetActive(true);
                cardGameObject = obj;
                CardObject cardObject = cardGameObject.GetComponent<CardObject>();
                cardObject.setValue(deck[cardIndex]);
                deck.RemoveAt(cardIndex);
                return cardObject;
            }
        }
        return null;
    }

    public Sprite getCardSprite(Suit suit, int value)
    {
        return cardSprites[13 * (int)suit + value - 1];
    }

    public Sprite getCardBackSprite()
    {
        return cardSprites[CARD_BACK];
    }

    // Update is called once per frame
    void Update()
    {
        if(buttonTimer < buttonTotalTime)
        {
            buttonTimer += Time.deltaTime;
        }

        float easedValue = EasingFunction.EaseOutElastic(0, 1f, buttonTimer / buttonTotalTime);

        Vector2 position = dealButton.gameObject.GetComponent<RectTransform>().anchoredPosition;
        if (position.x != dealButtonNewPosition.x)
        {
            dealButton.gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.LerpUnclamped(position, dealButtonNewPosition, easedValue);
        }

        position = standButton.gameObject.GetComponent<RectTransform>().anchoredPosition;
        if (position.x != standButtonNewPosition.x)
        {
            standButton.gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(position, standButtonNewPosition, easedValue);
        }
    }
}
