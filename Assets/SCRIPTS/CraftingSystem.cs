using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    public GameObject craftingScreenUI;
    public GameObject toolsScreenUI;

    public List<string> inventoryItemList = new List<string>();
    
    // Category Buttons
    private Button toolsBtn;
    
    // Craft Buttons
    private Button craftAxeBtn;
    
    // Requirement Text
    private Text AxeReq1, AxeReq2;

    public bool isOpen;
    
    // All Blueprint
    public Blueprint AxeBLP = new Blueprint("Axe", 2, "Stone", 3, "Stick", 3);

    public static CraftingSystem Instance { get; set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;

        toolsBtn = craftingScreenUI.transform.Find("ToolsButton").GetComponent<Button>();
        toolsBtn.onClick.AddListener(delegate { OpenToolsCategory(); });
        
        // Axe
        AxeReq1 = toolsScreenUI.transform.Find("Axe").transform.Find("req1").GetComponent<Text>();
        AxeReq2 = toolsScreenUI.transform.Find("Axe").transform.Find("req2").GetComponent<Text>();

        craftAxeBtn = toolsScreenUI.transform.Find("Axe").transform.Find("Button").GetComponent<Button>();
        craftAxeBtn.onClick.AddListener(delegate { CraftAnyItem(AxeBLP); });
    }

    void OpenToolsCategory()
    {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(true);
    }

    void CraftAnyItem(Blueprint blueprintToCraft)
    {
        // add item into inventory
        InventorySystem.Instance.AddToInventory(blueprintToCraft.itemName);
        
        // remove resources from inventory
        if (blueprintToCraft.numOfRequirements == 1)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1Amount);
        }
        else if (blueprintToCraft.numOfRequirements == 2)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1Amount);
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req2, blueprintToCraft.Req2Amount);
        }

        // refresh it
        StartCoroutine(calculate());

        RefreshNeededItems();
    }


    public IEnumerator calculate()
    {
        yield return new WaitForSeconds(1f);
        
        InventorySystem.Instance.ReCaculateList();
    }

    // Update is called once per frame
    void Update()
    {
        RefreshNeededItems();
        
        if (Input.GetKeyDown(KeyCode.C) && !isOpen)
        {
            craftingScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            isOpen = true;
 
        }
        else if (Input.GetKeyDown(KeyCode.C) && isOpen)
        {
            craftingScreenUI.SetActive(false);
            toolsScreenUI.SetActive(false);
            
            if (!InventorySystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            
            isOpen = false;
        }
    }

    private void RefreshNeededItems()
    {
        int stone_count = 0;
        int stick_count = 0;
        
        inventoryItemList = InventorySystem.Instance.itemList;

        foreach (string itemName in inventoryItemList)
        {
            switch (itemName)
            {
                case "Stone":
                    stone_count += 1;
                    break;
                case "Stick":
                    stick_count += 1;
                    break;
            }
        }
        
        // axe
        AxeReq1.text = "3 Stone [" + stone_count + "]";
        AxeReq2.text = "3 Stick [" + stick_count + "]";

        if (stone_count >= 3 && stick_count >= 3)
        {
            craftAxeBtn.gameObject.SetActive(true);
        }
        else
        {
            craftAxeBtn.gameObject.SetActive(false);
        }
    }
}
