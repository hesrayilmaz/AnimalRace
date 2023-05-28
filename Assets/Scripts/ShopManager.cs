using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    private int currentCharacterIndex = 0;
    private string characterPlayerPrefs = "CurrentCharacter";

    private Transform currentIcon;

    [SerializeField] private GameObject buttonPurchaseItem;
    [SerializeField] private TextMeshProUGUI purchaseText;

    [SerializeField] private GameObject buttonSelectItem;
    [SerializeField] private GameObject textSelectedItem;

    [SerializeField] private Vector3 characterIconPosition;

    [SerializeField] private float characterSpinSpeed = 100;
    private float characterRotation = 0;

    private Characters characters;
    private Character currentCharacter;
    private Character selectedCharacter;
    private int charactersLength;

    [SerializeField] private CoinManager coinManager;

    void Start()
    {
        characters = GameObject.Find("Characters").GetComponent<Characters>();
        charactersLength = characters.GetCharacters().Length;

        //ResetToDefault();

        //Set first character as purchased as default 
        PlayerPrefs.SetString(characters.GetCharacter(0).characterName + "Purchased", "true");
        characters.GetCharacter(0).isPurchased = PlayerPrefs.GetString(characters.GetCharacter(0).characterName + "Purchased");

        //PlayerPrefs.SetString(characters.GetCharacter(0).characterName + "Selected", "true");
        characters.GetCharacter(0).isSelected = PlayerPrefs.GetString(characters.GetCharacter(0).characterName + "Selected", "true");


        // Get all purchased status from all the blocks, using PlayerPrefs
        for (int index = 1; index < charactersLength; index++)
        {
            characters.GetCharacter(index).isPurchased = PlayerPrefs.GetString(characters.GetCharacter(index).characterName + "Purchased", "false");
            characters.GetCharacter(index).isSelected = PlayerPrefs.GetString(characters.GetCharacter(index).characterName + "Selected", "false");
        }

        foreach (Character chr in characters.GetCharacters())
        {
            if (chr.isSelected == "true")
                selectedCharacter = chr;
        }

        //currentCharacterIndex = PlayerPrefs.GetInt(blockPlayerPrefs, currentCharacterIndex);
        currentCharacter = characters.GetCharacter(currentCharacterIndex);

        ChangeBlock(0);
    }

    void Update()
    {
        if (currentIcon)
        {
            currentIcon.Rotate(Vector3.up * characterSpinSpeed * Time.deltaTime, Space.World);
        }
    }

    public void ChangeBlock(int changeValue)
    {
        // Change the index of the block
        currentCharacterIndex += changeValue;

        // Make sure we don't go out of the list of available stages
        if (currentCharacterIndex > charactersLength - 1) currentCharacterIndex = 0;
        else if (currentCharacterIndex < 0) currentCharacterIndex = charactersLength - 1;

        currentCharacter = characters.GetCharacter(currentCharacterIndex);

        // Remove the icon of the previous stage
        if (currentIcon)
        {
            characterRotation = currentIcon.eulerAngles.y;

            Destroy(currentIcon.gameObject);
        }

        // Display the icon of the current block
        if (currentCharacter.characterPrefab)
        {
            currentIcon = Instantiate(currentCharacter.characterPrefab, characterIconPosition, Quaternion.identity) as Transform;

            currentIcon.eulerAngles = Vector3.up * characterRotation;

            if (currentIcon.GetComponent<Animation>()) currentIcon.GetComponent<Animation>().Play();
        }

        
        if (currentCharacter.isPurchased == "false")
        {
            buttonPurchaseItem.SetActive(true);
            purchaseText.text = currentCharacter.price.ToString();
            buttonSelectItem.SetActive(false);
            textSelectedItem.SetActive(false);
        }
        else
        {
            buttonPurchaseItem.SetActive(false);

            if (currentCharacter.isSelected == "true")
            {
                buttonSelectItem.SetActive(false);
                textSelectedItem.SetActive(true);
            }
            else
            {
                textSelectedItem.SetActive(false);
                buttonSelectItem.SetActive(true);
            }
        }

    }
    
    public void PurchaseItem()
    {
        //if (buttonPurchaseItem.GetComponent<Animation>()) buttonPurchaseItem.GetComponent<Animation>().Play();

        if (currentCharacter.price <= coinManager.GetCoins())
        {
            if (selectedCharacter != null)
            {
                Debug.Log("iffff selected block before: " + selectedCharacter.characterName);
                PlayerPrefs.SetString(selectedCharacter.characterName + "Selected", "false");
                selectedCharacter.isSelected = PlayerPrefs.GetString(selectedCharacter.characterName + "Selected");
            }
            else
            {
                foreach (Character chr in characters.GetCharacters())
                {
                    if (chr.isSelected == "true")
                        selectedCharacter = chr;
                }
                Debug.Log("elseee selected block before: " + selectedCharacter.characterName);
                //selectedBlock = characters.GetCharacter(0);
                PlayerPrefs.SetString(selectedCharacter.characterName + "Selected", "false");
                selectedCharacter.isSelected = PlayerPrefs.GetString(selectedCharacter.characterName + "Selected");
            }

            coinManager.DecreaseCoin(currentCharacter.price);
            PlayerPrefs.SetString(currentCharacter.characterName + "Purchased", "true");
            currentCharacter.isPurchased = PlayerPrefs.GetString(currentCharacter.characterName + "Purchased");
            PlayerPrefs.SetString(currentCharacter.characterName + "Selected", "true");
            currentCharacter.isSelected = PlayerPrefs.GetString(currentCharacter.characterName + "Selected");
            selectedCharacter = currentCharacter;

            Debug.Log("selected block after: " + selectedCharacter.characterName);
            string selectedcharacterName = currentCharacter.characterName;
            PlayerPrefs.SetString("SelectedCharacter", selectedcharacterName);

            buttonPurchaseItem.SetActive(false);
            textSelectedItem.SetActive(true);

            PlayerPrefs.SetInt(characterPlayerPrefs, currentCharacterIndex);
        }

    }

    
    public void SelectItem()
    {
        //if (buttonSelectItem.GetComponent<Animation>()) buttonSelectItem.GetComponent<Animation>().Play();

        if (selectedCharacter != null)
        {
            PlayerPrefs.SetString(selectedCharacter.characterName + "Selected", "false");
            selectedCharacter.isSelected = PlayerPrefs.GetString(selectedCharacter.characterName + "Selected");
        }
        else
        {
            foreach (Character chr in characters.GetCharacters())
            {
                if (chr.isSelected == "true")
                    selectedCharacter = chr;
            }
            Debug.Log("elseee selected block before: " + selectedCharacter.characterName);
            //selectedBlock = characters.GetCharacter(0);
            PlayerPrefs.SetString(selectedCharacter.characterName + "Selected", "false");
            selectedCharacter.isSelected = PlayerPrefs.GetString(selectedCharacter.characterName + "Selected");
        }

        PlayerPrefs.SetString(currentCharacter.characterName + "Selected", "true");
        currentCharacter.isSelected = PlayerPrefs.GetString(currentCharacter.characterName + "Selected");
        selectedCharacter = currentCharacter;
        Debug.Log("selected block after: " + selectedCharacter.characterName);
        string selectedcharacterName = currentCharacter.characterName;
        PlayerPrefs.SetString("SelectedCharacter", selectedcharacterName);

        textSelectedItem.SetActive(true);
        buttonSelectItem.SetActive(false);

        PlayerPrefs.SetInt(characterPlayerPrefs, currentCharacterIndex);
    }


    
    //If settings should be reset
    private void ResetToDefault()
    {
        for (int index = 1; index < charactersLength; index++)
        {
            PlayerPrefs.SetString(characters.GetCharacter(index).characterName + "Purchased", "false");
            characters.GetCharacter(index).isPurchased = PlayerPrefs.GetString(characters.GetCharacter(index).characterName + "Purchased");
            PlayerPrefs.SetString(characters.GetCharacter(index).characterName + "Selected", "false");
            characters.GetCharacter(index).isSelected = PlayerPrefs.GetString(characters.GetCharacter(index).characterName + "Selected");
        }
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(characterIconPosition, new Vector3(1, 0.1f, 1));
    }
}
