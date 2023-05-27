using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character { 

    public string characterName;
    public Transform characterPrefab;
    public string isPurchased = "false";
    public string isSelected = "false";
    public int price;
}
