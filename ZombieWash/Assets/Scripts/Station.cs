using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Station : MonoBehaviour
{
    private bool cardInStation = false;

    [SerializeField] 
    public TMP_Text turnsLeftText;

    [SerializeField]
    public Vector3 personTravelPosition;

    [SerializeField]
    public TaskScriptableObject stationTaskSript;

    public bool isCardDroppedOn = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Card"))
        {
            isCardDroppedOn = true;
        }
    }

    public bool hasCardInStation()
    {
        return cardInStation;
    }

    public void cardEntersStation()
    {
        cardInStation = true;
    }

    public void cardLeavesStation()
    {
        cardInStation = false;
    }

    public void updateTurnsLeft(int turnsLeft)
    {
        turnsLeftText.text = turnsLeft.ToString();
    }
}
