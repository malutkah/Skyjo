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
    public TMP_Text dialogText;

    [HideInInspector] public CardData enemyData;
    private GameObject newCardGO;
    private GameObject minusTwo, minusOne;
    private GameObject zero;
    private GameObject one, two, three, four, five, six, seven, eight, nine, ten, eleven, twelve;
    private Field playerField, enemyField;
   
    
    #region Unity
    
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
        
        playerField.CreateColumns();
        enemyField.CreateColumns();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            PlaceCardOnStack();
        }
    }
    
    #endregion

    #region Game Rules

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

    #endregion

    #region Game Flow

    /* ***********************
     *       GAME FLOW:      *
     *************************/
    
    // GameFlowManager to combine the GameFlow Methods (Script? Method?)
    

    /*    (1) player needs to turn 2 cards (automated for cpu)
    */
    private void PlayerTurnTwoCards()
    {
        dialogText.text = "Choose 2 Cards to reveal!";
        // wait for player
        
        // check if 2 cards where opened
    }

    private void EnemyTurnTowCards()
    {
        for (int i = 0; i < 2; i++)
        {
            var r = Random.Range(0, enemyField.CardPositions.Count - 1);
            enemyField.GetCardFromField(r).TurnCard();
        }
    }
     
    /*    (2) player with the highest score begins
     */ 
    
     /*    (3) draw card and decide to put back or trade
     *      3.1 - if decided to put back:
     *              - player has to choose a not yet turned card to reveal
     *
     *      3.2 - if player decided to trade
     *              - he has to choose a card to trade
     *                  - that card is going to the stack
     * 
     *    () choose a card from the stack to trade with own card
     *
     */ 
    
    
    /* when a card is turned:
        - from which player?
        - get the number
        - was it the last card? yes => one last turn for the other player, then game over
                                no  => continue
        - sum up points     
     */

    public void CheckForGameOver(Location loc)
    {
        Field f = loc == Location.PLAYER ? playerField : enemyField;

        if (f.AreAllCardsTurned())
        {
            Debug.Log("Last Turn...");
            Debug.Log("Game Over..!");
        }
        
    }

    #endregion
    
    #region Place cards

    public void PlaceCardOnStack()
    {
        Card c = CreateCard();

        cardStackPrefab.GetComponent<Card>().value = c.value;
        cardStackPrefab.GetComponent<Card>().valueText.text = c.valueText.text;
        cardStackPrefab.GetComponent<Card>().TurnCard();
        cardStackPrefab.GetComponent<Card>().location = c.location;
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
    #endregion

    #region Column Check
    private bool AllValuesAreTheSame(CardColumn col)
    {
        for (int i = 1; i < col.cards.Length; i++)
        {
            if (col.cards[i].value != col.cards[0].value)
            {
                return false;
            }
        }
        return true;
    }

    private void RemoveColumn(CardColumn currentCol, Location loc)
    {
        Debug.Log($"Column {currentCol.columnNumber+1} can be removed!");

        for (int i = 0; i < currentCol.cards.Length; i++)
        {
            currentCol.cards[i].wasTurned = false;
            currentCol.deleted = true;
            currentCol.cards[i].gameObject.SetActive(false);
        }
        
        UpdateScore(loc);
    }

    public void CheckColumnForSameCard(Location loc)
    {
        Field f = loc == Location.PLAYER ? playerField : enemyField;
        
        for (int i = 0; i < 4; i++)
        {
            var currentCol = f.GetColumn(i);

            if (!currentCol.deleted)
            {
                if (AllValuesAreTheSame(currentCol))
                {
                    RemoveColumn(currentCol, loc);
                    break;
                }
            }
        }
    }
    #endregion
    
    #region Update Fields
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
    
    public void UpdateScore(Location loc)
    {
        Field f = loc == Location.PLAYER ? playerField : enemyField;

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
        CheckForGameOver(loc);
    }
    
    #endregion

    #region create and load deck
    
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


    private void InstantiateCard(GameObject c, Transform t)
    {
        newCardGO = Instantiate(c, t, true);
        newCardGO.transform.localScale = new Vector3(1f, 1f, 1);
        newCardGO.transform.position = t.position;
    }

    private GameObject LoadCard(string path)
    {
        var _cardData = Resources.Load<CardData>(path);
        GameObject c = _cardData.cardPrefab;
        c.GetComponent<Card>().value = _cardData.value;
        c.GetComponent<Card>().valueText.text = _cardData.valueText;

        return c;
    }
    
    #endregion
}
