using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum Location { DECK, STACK, PLAYER, OPPONENT }

public class Card : MonoBehaviour
{
    public TMP_Text valueText;
    public int value;
    public bool wasTurned;
    public Location location;

    public Card ThisCard;

    public Card EmptyCard()
    {
        return gameObject.AddComponent<Card>();
    }

    public void GetValuesFromCard(Card from)
    {
        value = from.value;
        wasTurned = from.wasTurned;
        location = from.location;
        valueText.text = from.valueText.text;
        location = from.location;
    }

    private void Start()
    {
        valueText.enabled = wasTurned;
    }

    public void TurnCard()
    {
        wasTurned = true;
        valueText.enabled = wasTurned;
    }

    // Card on click
    public void ShowCard()
    {
        if (!wasTurned)
        {
            if (GameManager.instance.gameState == State.START)
            {
                GameManager.instance.leftToTurn--;
                
                TurnCard();
                GameManager.instance.UpdateScore(location);
                
                GameManager.instance.PlayerTurnTwoCards();
            }

            if (GameManager.instance.gameState == State.PLAYER_TURN)
            {
                TurnCard();
                GameManager.instance.UpdateScore(location);
                GameManager.instance.playerRevealedCard = true;
            }
        }
    }
}
