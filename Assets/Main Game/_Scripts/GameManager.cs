using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum State { PLAYER_TURN, ENEMY_TURN, CHOOSING, WON, LOST, OVER }
 
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject gameBoard, enemyBoard, cardStack;
    public GameObject cardStackPrefab;
    public CardDeck cardDeck;
    public TMP_Text playerScoreText, enemyScoreText;

    [HideInInspector] public CardData enemyData;
    private GameObject newCardGO;
    private GameObject minusTwo, minusOne;
    private GameObject zero;
    private GameObject one, two, three, four, five, six, seven, eight, nine, ten, eleven, twelve;
    private Field playerField, enemyField;

    /*
     * RULES:
     * 
     * - each player has one move per round
     * - player with the least points wins
     * - if player who turned the last card has more points than his opponent, his points get doubled
     * - at the beginning, every player reveals two cards
     *  - the player with the highest score begins
     * - one card the the deck is revealed (stack)
     */

    /*
     *   GAME FLOW:
     *   
     *    (1) choose a card from the stack to trade with own card
     * 
     *    (2) draw card and decide to put back or trade
     *      - if decided to put back:
     *          - player has to choose a not yet turned card to reveal
     *      - if player decided to trade
     *          - he has to choose a card to trade
     *              - that card is going to the stack
     * 
        when a card is turned:
            - from which player?
            - get the number
            - was it the last card? yes => game over, no => continue
            - sum up points
     
     */
    
    private void Awake()
    {
        instance = this;

        playerField = gameBoard.GetComponent<Field>();
        enemyField = enemyBoard.GetComponent<Field>();
    }

    // Start is called before the first frame update
    void Start()
    {
        string pathPrefix = "scriptable objects/";

        minusTwo = LoadCard(pathPrefix + "minusTwo");
        minusOne = LoadCard(pathPrefix + "minusOne");
        zero = LoadCard(pathPrefix + "zero");
        one = LoadCard(pathPrefix + "one");
        two = LoadCard(pathPrefix + "two");
        three = LoadCard(pathPrefix + "three");
        four = LoadCard(pathPrefix + "four");
        five = LoadCard(pathPrefix + "five");
        six = LoadCard(pathPrefix + "six");
        seven = LoadCard(pathPrefix + "seven");
        eight = LoadCard(pathPrefix + "eight");
        nine = LoadCard(pathPrefix + "nine");
        ten = LoadCard(pathPrefix + "ten");
        eleven = LoadCard(pathPrefix + "eleven");
        twelve = LoadCard(pathPrefix + "twelve");

        CreateDeck();
        cardDeck.ShuffleDeck();

        PlaceCardInField();
        playerField.CreateFieldColumns();
        enemyField.CreateFieldColumns();
        
        playerField.CreateFieldColumnsEx();
        enemyField.CreateFieldColumnsEx();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            PlaceCardOnStack();
        }
    }

    // update score after turning card
    public void UpdateScore(GameObject cardGO, Card c)
    {
        switch (cardGO.tag)
        {
            case "PlayerField":
                //update player score
                playerField.UpdateScore(c.value);
                Debug.Log(c.value);
                break;
            case "EnemyField":
                // update enemy score
                enemyField.UpdateScore(c.value);
                break;
        }
    }   

    public void SwitchCards(Card cardInField, Card cardFromStack)
    {
        (cardInField.value, cardFromStack.value) = (cardFromStack.value, cardInField.value);

        cardFromStack.valueText.text = cardFromStack.value.ToString();

        cardInField.valueText.enabled = true;
        cardInField.valueText.text = cardInField.value.ToString();
        cardInField.wasTurned = true;

        // update column
        UpdateScore(cardInField.location);
    }

    void PlaceCardOnStack()
    {
        Card c = CreateCard();

        cardStackPrefab.GetComponent<Card>().value = c.value;
        cardStackPrefab.GetComponent<Card>().valueText.text = c.valueText.text;
        cardStackPrefab.GetComponent<Card>().TurnCard();
        cardStackPrefab.GetComponent<Card>().location = c.location;
    }

    private bool AllValuesAreTheSame(ArraySlice2D<int> slice)
    {
        // Compare each element to the first element.
        for (int i = 1; i < slice.Length; i++)
        {
            if (slice[i] != slice[0])
            {
                // If any element is not equal to the first element, return false.
                return false;
            }
        }

        // If the loop completes without finding any elements that are not equal to the first element, return true.
        return true;
    }

    private void RemoveColumn(ArraySlice2D<int> currentCol)
    {
        Debug.Log($"Column {currentCol} can be removed!");
        
        // set card gameobjects from column to invisible
        // maybe set value to -99 => if -99 don't count them 
        
        // get GameObjects fromm currentCol
        
    }

    private void CheckColumnForSameCard(Location loc)
    {
        Field f = loc == Location.PLAYER ? playerField : enemyField;
        
        for (int i = 0; i < 4; i++)
        {
            var currentCol = f.GetColumn(i);

            if (AllValuesAreTheSame(currentCol))
            {
                RemoveColumn(currentCol);
                break;
            }

        }
    }

    public void UpdateScore(Location loc)
    {
        Field f = loc == Location.PLAYER ? playerField : enemyField;
        f.CreateFieldColumns();
        f.CreateFieldColumnsEx();


        int count = 0;
        for (int i = 0; i < f.CardPositions.Count; i++)
        {
            Card c = f.GetCardFromField(i);

            if (c.wasTurned && c.value != -99)
            {
                count += c.value;
            }
        }

        f.ScoreText.text = count.ToString();
        CheckColumnForSameCard(loc);
    }

    private void PlaceCardInField()
    {
        for (int i = 0; i < playerField.CardPositions.Count; i++)
        {
            InstantiateCard(cardDeck.DrawCard(Location.PLAYER), playerField.CardPositions[i].transform);
        }

        for (int i = 0; i < enemyField.CardPositions.Count; i++)
        {
            InstantiateCard(cardDeck.DrawCard(Location.OPPONENT), enemyField.CardPositions[i].transform);
        }

        playerField.SetCurrentHand();
        playerField.SetTag("PlayerField");
        enemyField.SetCurrentHand();
        enemyField.SetTag("EnemyField");
    }

    private void CreateDeck()
    {
        List<GameObject> cards = new List<GameObject>();

        // create deck
        // 5x : -2
        // 15x: 0
        // 10x: -1, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12

        for (int i = 0; i < 15; i++)
        {
            if (i < 5)
                cards.Add(minusTwo);

            if (i < 10)
            {
                cards.Add(minusOne);
                cards.Add(one);
                cards.Add(two);
                cards.Add(three);
                cards.Add(four);
                cards.Add(five);
                cards.Add(six);
                cards.Add(seven);
                cards.Add(eight);
                cards.Add(nine);
                cards.Add(ten);
                cards.Add(eleven);
                cards.Add(twelve);
            }

            cards.Add(zero);

        }

        cardDeck.PopulateDeck(cards);
    }

    private Card CreateCard()
    {
        return cardDeck.DrawCard(Location.STACK).GetComponent<Card>();
    }


    private Card InstantiateCard(GameObject c, Transform t)
    {
        newCardGO = Instantiate(c);
        newCardGO.transform.parent = t;
        newCardGO.transform.localScale = new Vector3(1f, 1f, 1);
        newCardGO.transform.position = t.position;
        return newCardGO.GetComponent<Card>();
    }

    private GameObject InstantiateCard(GameObject c, GameObject t)
    {
        newCardGO = Instantiate(c);
        newCardGO.transform.parent = t.transform;
        newCardGO.transform.localScale = new Vector3(1f, 1f, 1);
        newCardGO.transform.position = t.transform.position;
        return newCardGO;
    }

    private GameObject LoadCard(string path)
    {
        var _cardData = Resources.Load<CardData>(path);
        GameObject c = _cardData.cardPrefab;
        c.GetComponent<Card>().value = _cardData.value;
        c.GetComponent<Card>().valueText.text = _cardData.valueText;

        return c;
    }
}
