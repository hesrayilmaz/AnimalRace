using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character { 

    public string characterName;
    public Transform characterPrefab;
    private string isPurchased = "false";
    private string isSelected = "false";
    public int price;
}
