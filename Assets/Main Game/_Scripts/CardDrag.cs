using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDrag : MonoBehaviour, IDragHandler//, IBeginDragHandler, IEndDragHandler
{
    public void OnDrag(PointerEventData eventData)
    {

       // Debug.Log("OnDrag! " + eventData);
    }

}
