using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // needs infos about current situtaion
    // playerfield (open cards) + score
    // enemyfield (only open cards) + score
    // card on stack

    private Field myField, playerField;
    private Card cardOnStack;

    public void LoadAi(Field enemyField, Field playerField, Card cardOnStack)
    {
        myField = enemyField;
        this.playerField = playerField;
        this.cardOnStack = cardOnStack;
    }

    public void Decide()
    {
        int myScore = myField.Score;
        int playerScore = playerField.Score;

        List<Card> myOpenCards = GetOpenCard().Item1;
        List<Card> playerOpenCards = GetOpenCard().Item2;
        
        
        /*
         *  decide what to do:
         * 
         *  (1) reveal a card
         *  (2) draw a card and decide again
         *  (3) trade a card from the stack
         * 
         */
        
        /*
         *  some rules how to decide:
         *  player score greater than mine?
         *  which card
         * 
         */
        
        Debug.Log($"Enemy AI: I have {myOpenCards.Count} open cards!");

    }

    private (List<Card>, List<Card>) GetOpenCard()
    {
        
        List<Card> myOpenCards = new List<Card>();
        List<Card> playerOpenCards = new List<Card>();
        
        for (int i = 0; i < 12; i++)
        {
            Card e = myField.GetCardFromField(i);
            Card d = playerField.GetCardFromField(i);
            if (e.wasTurned)
            {
                myOpenCards.Add(e);
            }

            if (d.wasTurned)
            {
                playerOpenCards.Add(d);
            }
        }

        return (myOpenCards, playerOpenCards);
    }
}
