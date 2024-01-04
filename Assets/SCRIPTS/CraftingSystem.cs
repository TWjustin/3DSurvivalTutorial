using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    public GameObject craftingScreenUI;
    public GameObject toolsScreenUI, survivalScreenUI, refineScreenUI, constructionScreenUI;

    public List<string> inventoryItemList = new List<string>();
    
    // Category Buttons
    private Button toolsBtn, survivalBtn, refineBtn, constructionBtn;
    
    // Craft Buttons
    private Button craftAxeBtn, craftPlankBtn, craftFoundationBtn, craftWallBtn;
    
    // Requirement Text
    private Text AxeReq1, AxeReq2, PlankReq1, FoundationReq1, WallReq1;

    public bool isOpen;
    
    // All Blueprint
    public Blueprint AxeBLP = new Blueprint("Axe", 1, 2, "Stone", 3, "Stick", 3);
    public Blueprint PlankBLP = new Blueprint("Plank", 2, 1, "Log", 1, "", 0);
    public Blueprint FoundationBLP = new Blueprint("Foundation", 1, 1, "Plank", 4, "", 0);
    public Blueprint WallBLP = new Blueprint("Wall", 1, 1, "Plank", 2, "", 0);

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
        
        survivalBtn = craftingScreenUI.transform.Find("SurvivalButton").GetComponent<Button>();
        survivalBtn.onClick.AddListener(delegate { OpenSurvivalCategory(); });
        
        refineBtn = craftingScreenUI.transform.Find("RefineButton").GetComponent<Button>();
        refineBtn.onClick.AddListener(delegate { OpenRefineCategory(); });
        
        constructionBtn = craftingScreenUI.transform.Find("ConstructionButton").GetComponent<Button>();
        constructionBtn.onClick.AddListener(delegate { OpenConstructionCategory(); });
        
        // Axe
        AxeReq1 = toolsScreenUI.transform.Find("Axe").transform.Find("req1").GetComponent<Text>();
        AxeReq2 = toolsScreenUI.transform.Find("Axe").transform.Find("req2").GetComponent<Text>();

        craftAxeBtn = toolsScreenUI.transform.Find("Axe").transform.Find("Button").GetComponent<Button>();
        craftAxeBtn.onClick.AddListener(delegate { CraftAnyItem(AxeBLP); });
        
        // Plank
        PlankReq1 = refineScreenUI.transform.Find("Plank").transform.Find("req1").GetComponent<Text>();
        
        craftPlankBtn = refineScreenUI.transform.Find("Plank").transform.Find("Button").GetComponent<Button>();
        craftPlankBtn.onClick.AddListener(delegate { CraftAnyItem(PlankBLP); });
        
        // Foundation
        FoundationReq1 = constructionScreenUI.transform.Find("Foundation").transform.Find("req1").GetComponent<Text>();
        
        craftFoundationBtn = constructionScreenUI.transform.Find("Foundation").transform.Find("Button").GetComponent<Button>();
        craftFoundationBtn.onClick.AddListener(delegate { CraftAnyItem(FoundationBLP); });
        
        // Wall
        WallReq1 = constructionScreenUI.transform.Find("Wall").transform.Find("req1").GetComponent<Text>();
        
        craftWallBtn = constructionScreenUI.transform.Find("Wall").transform.Find("Button").GetComponent<Button>();
        craftWallBtn.onClick.AddListener(delegate { CraftAnyItem(WallBLP); });
    }

    void OpenToolsCategory()
    {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(true);
    }
    
    void OpenSurvivalCategory()
    {
        craftingScreenUI.SetActive(false);
        survivalScreenUI.SetActive(true);
    }
    
    void OpenRefineCategory()
    {
        craftingScreenUI.SetActive(false);
        refineScreenUI.SetActive(true);
    }
    
    void OpenConstructionCategory()
    {
        craftingScreenUI.SetActive(false);
        constructionScreenUI.SetActive(true);
    }

    void CraftAnyItem(Blueprint blueprintToCraft)
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.craftingSound);
        
        // produce the amount of items according to the blueprint
        for (var i = 0; i < blueprintToCraft.numberOfItemToProduce; i++)
        {
            // add item into inventory
            InventorySystem.Instance.AddToInventory(blueprintToCraft.itemName);
        }
        
        
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

        
    }


    public IEnumerator calculate()
    {
        yield return 0;
        InventorySystem.Instance.ReCaculateList();
        RefreshNeededItems();
    }

    // Update is called once per frame
    void Update()
    {
        
        
        if (Input.GetKeyDown(KeyCode.C) && !isOpen && !ConstructionManager.Instance.inConstructionMode)
        {
            craftingScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;
            
            isOpen = true;
 
        }
        else if (Input.GetKeyDown(KeyCode.C) && isOpen)
        {
            craftingScreenUI.SetActive(false);
            toolsScreenUI.SetActive(false);
            survivalScreenUI.SetActive(false);
            refineScreenUI.SetActive(false);
            constructionScreenUI.SetActive(false);


            if (!InventorySystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                
                SelectionManager.Instance.EnableSelection();
                SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
            }
            
            isOpen = false;
        }
    }

    public void RefreshNeededItems()
    {
        int stone_count = 0;
        int stick_count = 0;
        int log_count = 0;
        int plank_count = 0;
        
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
                case "Log":
                    log_count += 1;
                    break;
                case "Plank":
                    plank_count += 1;
                    break;
            }
        }
        
        // axe
        AxeReq1.text = "3 Stone [" + stone_count + "]";
        AxeReq2.text = "3 Stick [" + stick_count + "]";

        if (stone_count >= 3 && stick_count >= 3 && InventorySystem.Instance.CheckSlotAvailable(1))
        {
            craftAxeBtn.gameObject.SetActive(true);
        }
        else
        {
            craftAxeBtn.gameObject.SetActive(false);
        }
        
        // plank
        PlankReq1.text = "1 Log [" + log_count + "]";
        
        if (log_count >= 1 && InventorySystem.Instance.CheckSlotAvailable(2))
        {
            craftPlankBtn.gameObject.SetActive(true);
        }
        else
        {
            craftPlankBtn.gameObject.SetActive(false);
        }
        
        // foundation
        FoundationReq1.text = "4 Plank [" + plank_count + "]";
        
        if (plank_count >= 4 && InventorySystem.Instance.CheckSlotAvailable(1))
        {
            craftFoundationBtn.gameObject.SetActive(true);
        }
        else
        {
            craftFoundationBtn.gameObject.SetActive(false);
        }
        
        // wall
        WallReq1.text = "2 Plank [" + plank_count + "]";
        
        if (plank_count >= 2 && InventorySystem.Instance.CheckSlotAvailable(1))
        {
            craftWallBtn.gameObject.SetActive(true);
        }
        else
        {
            craftWallBtn.gameObject.SetActive(false);
        }
    }
}
