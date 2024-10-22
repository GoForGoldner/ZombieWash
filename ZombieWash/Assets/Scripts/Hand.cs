using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hand : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> cardObjects = new List<GameObject>();

    [SerializeField]
    private Canvas handCanvas;

    [SerializeField]
    [Range(1, 7)]
    private int startingHandSize;

    [SerializeField]
    private float xOffset;

    [SerializeField]
    private float yOffset;

    [SerializeField]
    [Range(0, 1000)]
    private float deckWidth;

    [SerializeField]
    private CardDeck cardDeck;


    // Start is called before the first frame update
    void Start()
    {
        cardDeck = GameObject.Find("Deck").GetComponent<CardDeck>();
        for (int i = 0; i < startingHandSize; i++)
        {
            drawCard();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void drawCard()
    {
        if (cardDeck.cardObjects.Count <= 0) return;
        addCardToHand(cardDeck.cardObjects[cardDeck.cardObjects.Count - 1]);
        cardDeck.cardObjects.RemoveAt(cardDeck.cardObjects.Count - 1);

        displayHand();
    }

    public void addCardToHand(GameObject cardObject)
    {
        cardObjects.Add(cardObject);
        cardObject.GetComponent<DisplayCard>().flipCard();
    }

    public void displayHand()
    {
        float xOffsetTemp = xOffset;
        float spacing = deckWidth / cardObjects.Count;

        foreach (GameObject cardObject in cardObjects)
        {
            // Set the parent to the Canvas
            cardObject.transform.SetParent(handCanvas.transform, false);

            // Adjust position based on offsets
            RectTransform rectTransform = cardObject.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = new Vector2(xOffsetTemp, yOffset);
                cardObject.GetComponent<DisplayCard>().originalPosition = rectTransform.anchoredPosition;
                xOffsetTemp += spacing; // Increment xOffsetTemp for the next card
            }
        }
    }
}
