using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Characters : MonoBehaviour
{

    [SerializeField] private Character[] characterArray;

    public Character[] GetCharacters()
    {
        return characterArray;
    }

    public Character GetCharacter(int index)
    {
        return characterArray[index];
    }
    
}
