using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class Field : MonoBehaviour
{
    public List<GameObject> CardPositions;
    public TMP_Text ScoreText;
    public int Score;
    
    public CardColumn[] columns;

    [SerializeField] private List<GameObject> currentHand;

    private void Awake()
    {
        currentHand = new List<GameObject>();
        columns = new CardColumn[4];
        Score = 0;
    }

    private void Start()
    {
        ScoreText.text = Score.ToString();
    }

    public bool AreAllCardsTurned()
    {
        for (int i = 0; i < CardPositions.Count; i++)
        {
            Card c = GetCardFromField(i);
            if (!c.wasTurned)
            {
                return false;
            }
        }

        return true;
    }

    public void CreateColumns()
    {
        int row, col;
        for (col = 0; col < 4; col++)
        {
            columns[col] = gameObject.AddComponent<CardColumn>();
            columns[col].columnNumber = col;
            
            for (row = 0; row < 3; row++)
            {
                Card c = GetCardFromField(col + row * columns.GetLength(0));
                columns[col].SetColumn(c, row);
            }
        }
    }

    public CardColumn GetColumn(int col)
    {
        return columns[col];
    }

    public void SetCurrentHand()
    {
        foreach (var t in CardPositions)
        {
            currentHand.Add(t.transform.GetChild(0).gameObject);
        }
    }

    public void SetTag(string t)
    {
        gameObject.tag = t;
    }

    
    public Card GetCardFromField(int position) => CardPositions[position].transform.GetChild(0).GetComponent<Card>();

}