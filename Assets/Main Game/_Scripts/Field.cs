using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Field : MonoBehaviour
{
    public List<GameObject> CardPositions;
    public int[,] FieldColumns;
    public GameObject[,] FieldColumnsEx;
    public TMP_Text ScoreText;
    public int Score;

    [SerializeField] private List<GameObject> currentHand;

    private void Awake()
    {
        currentHand = new List<GameObject>();
        FieldColumns = new int[4, 3];
        Score = 0;
    }

    private void Start()
    {
        ScoreText.text = Score.ToString();
    }

    public void CreateFieldColumns()
    {
        int row, col;
        for (row = 0; row < FieldColumns.GetLength(0); row++)
        {
            for (col = 0; col < FieldColumns.GetLength(1); col++)
            {
                FieldColumns[row, col] = GetCardValueFromCardPosition(row + col * FieldColumns.GetLength(0));
            }
        }
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
    
    // public void CreateFieldColumns()
    // {
    //     // Column 1   0  1  2
    //     // Column 1: [1, 5, 9 ]
    //     FieldColumns[0, 0] = GetCardValueFromCardPosition(0);
    //     FieldColumns[0, 1] = GetCardValueFromCardPosition(4);
    //     FieldColumns[0, 2] = GetCardValueFromCardPosition(8);
    //
    //     // Column 2
    //     // Column 2: [2, 6, 10]
    //     FieldColumns[1, 0] = GetCardValueFromCardPosition(1);
    //     FieldColumns[1, 1] = GetCardValueFromCardPosition(5);
    //     FieldColumns[1, 2] = GetCardValueFromCardPosition(9);
    //
    //     // Column 3
    //     // Column 3: [3, 7, 11]
    //     FieldColumns[2, 0] = GetCardValueFromCardPosition(2);
    //     FieldColumns[2, 1] = GetCardValueFromCardPosition(6);
    //     FieldColumns[2, 2] = GetCardValueFromCardPosition(10);
    //
    //     // Column 4
    //     // Column 4: [4, 8, 12]
    //     FieldColumns[3, 0] = GetCardValueFromCardPosition(3);
    //     FieldColumns[3, 1] = GetCardValueFromCardPosition(7);
    //     FieldColumns[3, 2] = GetCardValueFromCardPosition(11);
    // }

    public ArraySlice2D<int> GetColumn(int col)
    {
        return FieldColumns.Slice(col);
    }

    public void SetCurrentHand()
    {
        foreach (var t in CardPositions)
        {
            currentHand.Add(t.transform.GetChild(0).gameObject);
        }
    }

    public void UpdateScore(int newScore)
    {
        Score += newScore;
        ScoreText.text = Score.ToString();
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

    private int GetCardValueFromCardPosition(int position) => CardPositions[position].transform.GetChild(0).GetComponent<Card>().value;
    
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