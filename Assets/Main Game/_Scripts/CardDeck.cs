using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardDeck : MonoBehaviour
{
    [SerializeField] private List<GameObject> deck;
    public TMP_Text DeckCountText;
    public List<GameObject> deckShuffled;

    public void PopulateDeck(List<GameObject> cards)
    {
        deck = cards;
    }

    public void CountCards(GameObject card, int i = 0)
    {
        List<GameObject> count = i == 0 ? deck.FindAll(x => x == card) : deckShuffled.FindAll(x => x == card);


        //Debug.Log($"{count.Count}");
    }

    public void ShuffleDeck()
    {
        while (deck.Count > 0)
        {
            int randomCardInDeck = Random.Range(0, deck.Count);
            GameObject randomCard = deck[randomCardInDeck];
            deck.Remove(randomCard);

            deckShuffled.Add(randomCard);
        }

        //deck = deckShuffled;
    }


    // draws a card from shuffeld deck and removes it from the deck
    public GameObject DrawCard(Location loc)
    {
        int r = Random.Range(0, deckShuffled.Count);
        GameObject card = deckShuffled[r];
        deckShuffled.Remove(card);
        card.GetComponent<Card>().location = loc;
        DeckCountText.text = deckShuffled.Count.ToString();
        return card;
    }
}
