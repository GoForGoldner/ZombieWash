using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDeck : MonoBehaviour
{
    [SerializeField]
    private float xOffset;

    [SerializeField]
    private float yOffset;

    [SerializeField]
    [Range(0, 1000)]
    private float deckWidth;

    [SerializeField]
    [Range(0f, 1f)]
    public float cardSizeScale;

    [SerializeField]
    private GameObject cardPrefab; // Renamed for clarity

    [SerializeField]
    private int deckSize;

    [SerializeField]
    private DeckListScriptableObject deckList;

    [SerializeField]
    private Canvas deckCanvas;

    [SerializeField]
    private List<CardScriptableObject> cards = new List<CardScriptableObject>();

    [SerializeField]
    public List<GameObject> cardObjects = new List<GameObject>();

    void Start()
    {
        spawnDeck();
        displayDeck(); // Display the deck once at start
    }


    private void spawnDeck()
    {
        cards.Clear(); // Clear previous cards if any

        for (int i = 0; i < deckSize; i++)
        {
            cards.Add(deckList.cardScripts[Random.Range(0, deckList.cardScripts.Count)]);
        }
    }

    private void displayDeck()
    {
        float xOffsetTemp = xOffset;
        float spacing = deckWidth / cards.Count;

        foreach (CardScriptableObject cardScript in cards)
        {
            // Instantiate the UI prefab
            GameObject newCard = Instantiate(cardPrefab);

            DisplayCard d = newCard.GetComponent<DisplayCard>();
            d.cardScript = cardScript;
            //d.flipCard();

            newCard.SetActive(true);

            // Set the parent to the Canvas
            newCard.transform.SetParent(deckCanvas.transform, false);
            newCard.transform.localScale = new Vector3(1, 1, 1) * cardSizeScale;

            // Adjust position based on offsets
            RectTransform rectTransform = newCard.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = new Vector2(xOffsetTemp, yOffset);
                xOffsetTemp += spacing; // Increment xOffsetTemp for the next card
            }

            d.originalPosition = rectTransform.anchoredPosition;

            cardObjects.Add(newCard);
        }
    }
}
