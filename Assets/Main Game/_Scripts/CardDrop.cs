using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDrop : MonoBehaviour, IDropHandler
{
    // draggin card from stack to card field

    public void OnDrop(PointerEventData eventData)
    {
        var tC = gameObject.GetComponent<Card>();


        GameManager.instance.SwitchCards(tC, eventData.pointerDrag.gameObject.GetComponent<Card>());
        GameManager.instance.CheckColumnForSameCard(tC.location);
        GameManager.instance.playerTradedCard = true;
        GameManager.instance.PlayerTurn();
        //GameManager.instance.PlaceCardOnStack();
    }

}
