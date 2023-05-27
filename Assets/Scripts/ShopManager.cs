using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    private int currentCharacterIndex = 0;

    //public string blockPlayerPrefs = "CurrentBlock";

    private Transform currentIcon;

    public Button buttonPurchaseItem;
    public Text purchaseText;

    public Button buttonSelectItem;
    public Transform textSelectedItem;

    public Vector3 characterIconPosition;

    public float characterSpinSpeed = 100;
    private float characterRotation = 0;

    [SerializeField] private Characters characters;
    private Character currentCharacter;
    private Character selectedCharacter;
    private int charactersLength;

    public CoinManager coinManager;

    //[Tooltip("The/texture of the icon when it is locked ( The black color on locked 3d models )")]
    //public Texture lockedTexture;

    void Start()
    {
        //ResetToDefault();

        //characters = GameObject.Find("Blocks").GetComponent<BlockController>();
        charactersLength = characters.GetCharacters().Length;

        // Listen for a click to purchase item button to purchase current block
        //buttonPurchaseItem.onClick.AddListener(delegate { PurchaseItem(); });

        // Listen for a click to select item button to select current block if it is purchased
        //buttonSelectItem.onClick.AddListener(delegate { SelectItem(); });

        //Set first block as purchased as default 
        //PlayerPrefs.SetString(characters.GetCharacter(0).characterName + "Purchased", "true");
        //characters.GetCharacter(0).isPurchased = PlayerPrefs.GetString(characters.GetCharacter(0).characterName + "Purchased");

        //PlayerPrefs.SetString(characters.GetCharacter(0).characterName + "Selected", "true");
        //characters.GetCharacter(0).isSelected = PlayerPrefs.GetString(characters.GetCharacter(0).characterName + "Selected", "true");


        // Get all purchased status from all the blocks, using PlayerPrefs
        /*for (int index = 1; index < charactersLength; index++)
        {
            characters.GetCharacter(index).isPurchased = PlayerPrefs.GetString(characters.GetCharacter(index).characterName + "Purchased", "false");
            characters.GetCharacter(index).isSelected = PlayerPrefs.GetString(characters.GetCharacter(index).characterName + "Selected", "false");
        }*/

        
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

        /*
        if (currentCharacter.isPurchased == "false")
        {
            buttonPurchaseItem.interactable = true;
            buttonPurchaseItem.gameObject.SetActive(true);
            purchaseText.text = currentCharacter.price.ToString();
            buttonSelectItem.gameObject.SetActive(false);
            textSelectedItem.gameObject.SetActive(false);
        }
        else
        {
            buttonPurchaseItem.interactable = false;
            buttonPurchaseItem.gameObject.SetActive(false);

            if (currentCharacter.isSelected == "true")
            {
                buttonSelectItem.gameObject.SetActive(false);
                textSelectedItem.gameObject.SetActive(true);
            }
            else
            {
                textSelectedItem.gameObject.SetActive(false);
                buttonSelectItem.gameObject.SetActive(true);
            }
        }*/

    }
    /*
    public void PurchaseItem()
    {
        if (buttonPurchaseItem.GetComponent<Animation>()) buttonPurchaseItem.GetComponent<Animation>().Play();

        if (currentCharacter.price <= coinManager.GetCoins())
        {
            if (selectedBlock != null)
            {
                Debug.Log("iffff selected block before: " + selectedBlock.characterName);
                PlayerPrefs.SetString(selectedBlock.characterName + "Selected", "false");
                selectedBlock.isSelected = PlayerPrefs.GetString(selectedBlock.characterName + "Selected");
            }
            else
            {
                foreach (Block block in characters.GetCharacters())
                {
                    if (block.isSelected == "true")
                        selectedBlock = block;
                }
                Debug.Log("elseee selected block before: " + selectedBlock.characterName);
                //selectedBlock = characters.GetCharacter(0);
                PlayerPrefs.SetString(selectedBlock.characterName + "Selected", "false");
                selectedBlock.isSelected = PlayerPrefs.GetString(selectedBlock.characterName + "Selected");
            }

            coinManager.DecreaseCoin(currentCharacter.price);
            PlayerPrefs.SetString(currentCharacter.characterName + "Purchased", "true");
            currentCharacter.isPurchased = PlayerPrefs.GetString(currentCharacter.characterName + "Purchased");
            PlayerPrefs.SetString(currentCharacter.characterName + "Selected", "true");
            currentCharacter.isSelected = PlayerPrefs.GetString(currentCharacter.characterName + "Selected");
            selectedBlock = currentCharacter;

            Debug.Log("selected block after: " + selectedBlock.characterName);
            string selectedcharacterName = currentCharacter.characterName;
            PlayerPrefs.SetString("SelectedcharacterName", selectedcharacterName);

            buttonPurchaseItem.gameObject.SetActive(false);
            textSelectedItem.gameObject.SetActive(true);

            PlayerPrefs.SetInt(blockPlayerPrefs, currentCharacterIndex);
        }

    }*/

    /*
    public void SelectItem()
    {
        if (buttonSelectItem.GetComponent<Animation>()) buttonSelectItem.GetComponent<Animation>().Play();

        if (selectedBlock != null)
        {
            PlayerPrefs.SetString(selectedBlock.characterName + "Selected", "false");
            selectedBlock.isSelected = PlayerPrefs.GetString(selectedBlock.characterName + "Selected");
        }
        else
        {
            foreach (Block block in characters.GetCharacters())
            {
                if (block.isSelected == "true")
                    selectedBlock = block;
            }
            Debug.Log("elseee selected block before: " + selectedBlock.characterName);
            //selectedBlock = characters.GetCharacter(0);
            PlayerPrefs.SetString(selectedBlock.characterName + "Selected", "false");
            selectedBlock.isSelected = PlayerPrefs.GetString(selectedBlock.characterName + "Selected");
        }

        PlayerPrefs.SetString(currentCharacter.characterName + "Selected", "true");
        currentCharacter.isSelected = PlayerPrefs.GetString(currentCharacter.characterName + "Selected");
        selectedBlock = currentCharacter;

        string selectedcharacterName = currentCharacter.characterName;
        PlayerPrefs.SetString("SelectedcharacterName", selectedcharacterName);

        textSelectedItem.gameObject.SetActive(true);
        buttonSelectItem.gameObject.SetActive(false);

        PlayerPrefs.SetInt(blockPlayerPrefs, currentCharacterIndex);
    }
    */

    /*
    //If settings should be reset
    private void ResetToDefault()
    {
        PlayerPrefs.SetString(characters.GetCharacter(1).characterName + "Selected", "false");
        characters.GetCharacter(1).isSelected = PlayerPrefs.GetString(characters.GetCharacter(0).characterName + "Selected", "true");
        PlayerPrefs.SetString(characters.GetCharacter(2).characterName + "Selected", "false");
        characters.GetCharacter(2).isSelected = PlayerPrefs.GetString(characters.GetCharacter(0).characterName + "Selected", "true");
        PlayerPrefs.SetString(characters.GetCharacter(2).characterName + "Purchased", "false");
        characters.GetCharacter(2).isPurchased = PlayerPrefs.GetString(characters.GetCharacter(0).characterName + "Selected", "true");
    }
    */
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(characterIconPosition, new Vector3(1, 0.1f, 1));
    }
}
