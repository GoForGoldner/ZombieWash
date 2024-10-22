using System.Collections.Generic;
using System.IO.Pipes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR;

public class DisplayCard : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField]
    public CardScriptableObject cardScript;

    public List<TaskScriptableObject> taskList;

    private bool cardFlipped = false;

    [SerializeField]
    private TMP_Text taskText;

    [SerializeField]
    private Image frontSide;

    [SerializeField] 
    private Image backSide;

    [SerializeField]
    private Image thumbnail;

    [SerializeField]
    private Image healthBar;

    private GameObject player;

    public int health;

    [SerializeField]
    [Range(0f, 1f)]
    private float sizeOfCardOnStation;

    private TurnManager turnManager;

    private Hand hand;

    private Vector2 dragOffset;

    public bool onStation = false;

    private bool isDragging = false;
    private RectTransform rectTransform;
    public Vector2 originalPosition; // Store original position

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        turnManager = GameObject.FindGameObjectWithTag("TurnManager").GetComponent<TurnManager>();

        hand = GameObject.FindGameObjectWithTag("Hand").GetComponent<Hand>();

        taskList = new List<TaskScriptableObject>(cardScript.tasks);

        thumbnail.sprite = cardScript.zombieThumbnail;
        backSide.sprite = cardScript.backCard;
        health = cardScript.health;
    }

    // Update is called once per frame
    void Update()
    {
        updateTaskText();
        updateHealthBar();
    }

    public void updateTaskText()
    {
        // Creates the information for what tasks need to be done on a card
        string taskInfo = "Tasks:\n";
        foreach (TaskScriptableObject task in taskList)
        {
            taskInfo += "- " + task.name + " (" + task.turnsToComplete + " turns)" + "\n";
        }

        taskText.text = taskInfo;
    }

    private void updateHealthBar()
    {

        switch (health)
        {
            case 3:
                healthBar.color = new Color(32f / 255f, 221f / 255f, 26f / 255f); // Green
                break;
            case 2:
                healthBar.color = new Color(229f / 255f, 105f / 255f, 14f / 255f); // Orange
                break;
            case 1:
                healthBar.color = new Color(229f / 255f, 0f / 255f, 0f / 255f); // Red
                break;
            default:
                healthBar.color = Color.white; // Default to white or any other color
                break;
        }
    }

    public void flipCard()
    {
        frontSide.gameObject.SetActive(!cardFlipped);
        backSide.gameObject.SetActive(cardFlipped);

        cardFlipped = !cardFlipped;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            isDragging = true;
            // Calculate the offset between the mouse position and the card position
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform.parent as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out dragOffset);
            dragOffset = rectTransform.anchoredPosition - dragOffset;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            isDragging = false;

            CheckDrop();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging && cardFlipped && !onStation)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint);

            // Update the card's anchored position based on the mouse position and the stored offset
            rectTransform.anchoredPosition = localPoint + dragOffset;
        }
    }

    private void CheckDrop()
    {
        bool wasDropped = false;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(rectTransform.position, 50f); // Adjust radius as needed
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Station")) // Ensure the target has this tag
            {
                Station station = hitCollider.GetComponent<Station>();
                if (station != null && station.isCardDroppedOn && !station.hasCardInStation() && taskIsNecessary(station.stationTaskSript.taskName))
                {
                    wasDropped = true;

                    // Add any additional logic for when the card is successfully placed
                    rectTransform.position = station.transform.position;
                    transform.localScale = new Vector3(sizeOfCardOnStation, sizeOfCardOnStation, sizeOfCardOnStation);

                    onStation = true;

                    originalPosition = transform.localPosition;

                    station.cardEntersStation();

                    hand.cardObjects.Remove(gameObject);

                    player.GetComponent<Movement>().setDestination(station.personTravelPosition);

                    turnManager.AddCardToStation(gameObject, station, station.stationTaskSript.turnsToComplete);
                    turnManager.nextTurn();
                }
            }
        }

        if (!wasDropped)
        {
            // Return to original position if not placed on target
            rectTransform.anchoredPosition = originalPosition;
        }
    }

    private bool taskIsNecessary(string taskName)
    {
        foreach(TaskScriptableObject task in taskList)
        {
            if (task.taskName == taskName)
            {
                taskList.Remove(task);
                return true;
            }
        }

        return false;
    }
}
