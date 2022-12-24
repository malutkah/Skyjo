using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class Field : MonoBehaviour
{
    public List<GameObject> CardPositions;
    public GameObject[,] FieldColumnsEx;
    public TMP_Text ScoreText;
    public int Score;
    
    public CardColumn[] columns;

    [SerializeField] private List<GameObject> currentHand;

    private void Awake()
    {
        currentHand = new List<GameObject>();
        FieldColumnsEx = new GameObject[4, 3];
        columns = new CardColumn[4];
        Score = 0;
    }

    private void Start()
    {
        ScoreText.text = Score.ToString();
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

    public void CreateFieldColumnsEx()
    {
        int row, col;
        for (row = 0; row < FieldColumnsEx.GetLength(0); row++)
        {
            for (col = 0; col < FieldColumnsEx.GetLength(1); col++)
            {
                FieldColumnsEx[row, col] = GetCardGameObjectFromCardPosition(row + col * FieldColumnsEx.GetLength(0));
            }
        }
    }
    
    public ArraySlice2D<GameObject> GetColumnEx(int col)
    {
        return FieldColumnsEx.Slice(col);
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

    public void DeactivateColumn(ArraySlice2D<int> col)
    {
        for (int i = 0; i < col.Length; i++)
        {
            col[i] = -99;
        }
    }
    
    public Card GetCardFromField(int position) => CardPositions[position].transform.GetChild(0).GetComponent<Card>();

    private GameObject GetCardGameObjectFromCardPosition(int position) => CardPositions[position].transform.GetChild(0).gameObject;
}

#region ArraySlice2D

static class ArraySliceExt
{
    public static ArraySlice2D<T> Slice<T>(this T[,] arr, int firstDimension)
    {
        return new ArraySlice2D<T>(arr, firstDimension);
    }
}

public class ArraySlice2D<T>
{
    private readonly T[,] arr;
    private readonly int firstDimension;
    private readonly int length;
    public int Length { get { return length; } }

    public ArraySlice2D(T[,] arr, int firstDimension)
    {
        this.arr = arr;
        this.firstDimension = firstDimension;
        length = arr.GetUpperBound(1) + 1;
    }

    public T this[int index]
    {
        get => arr[firstDimension, index];
        set => arr[firstDimension, index] = value;
    }
}

#endregion