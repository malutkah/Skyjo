using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(fileName ="New Card", menuName ="Card")]
public class CardData : ScriptableObject
{
    public GameObject cardPrefab;
    public int value;
    public string valueText;
}
