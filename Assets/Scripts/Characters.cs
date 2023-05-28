using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Characters : MonoBehaviour
{
    public static Characters instance;

    [SerializeField] private Character[] characterArray;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public Character[] GetCharacters()
    {
        return characterArray;
    }

    public Character GetCharacter(int index)
    {
        return characterArray[index];
    }
    
}
