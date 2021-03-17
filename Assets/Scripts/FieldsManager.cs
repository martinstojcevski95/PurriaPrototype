using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FieldsManager : MonoBehaviour
{
    public static FieldsManager Instance;

    public List<Plant> FieldPlants = new List<Plant>();
    public List<Contract> ContractFields = new List<Contract>();
    public List<FieldManager> FieldsManagerButtons = new List<FieldManager>();

    [SerializeField]
    ContractController contractController;

    [SerializeField]
    List<Button> DirtBlocks = new List<Button>();

    [SerializeField]
    RectTransform FieldHolder;
    public Contract currentContract;



    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        SetAllContractsForFields();
    }

    void SetAllContractsForFields()
    {
        //var findFieldManagerButtons = FindObjectsOfType<FieldManager>();
        //if (findFieldManagerButtons != null)
        //    FieldsManagerButtons = findFieldManagerButtons.ToList();
        //var sortedList = FieldsManagerButtons.OrderBy(go => go.name).ToList();
        //FieldsManagerButtons = sortedList;
        for (int i = 0; i < contractController.Contracts.Count; i++)
        {
            ContractFields.Add(contractController.Contracts[i]);

        }

        var findFieldDirtBlocks = GameObject.FindGameObjectsWithTag("dirtblock");
        foreach (var item in findFieldDirtBlocks)
        {
            DirtBlocks.Add(item.GetComponent<Button>());
        }
        DirtBlocks.ForEach(b => b.gameObject.AddComponent<DirtBlock>());


        FieldsManagerButtons.ForEach(f => f.GetComponent<Button>().interactable = false);
    }

    public void SetField(int contractid)
    {
        currentContract = ContractFields.Find(c => c.contractStats.ContractID.Equals(contractid));
        // var currentContract = ContractFields.Find(c => c.contractStats.ContractID.Equals(contractid));
        Debug.Log(currentContract.contractStats.ContractID);

        //var contract = ContractFields.Find(c => c.contractStats.ContractID == contractid);
        if (currentContract != null)
        {
            if (FieldPlants.Count > -1)
            {
                FieldPlants = currentContract.plants;
                Testing();
            }
        }

    }


    void Testing()
    {
        for (int i = 0; i < DirtBlocks.Count; i++)
        {
            if (DirtBlocks[i].gameObject.GetComponent<DirtBlock>() != null)
            {
                //  DirtBlocks[i].GetComponent<DirtBlock>().blockPlant = FieldPlants[i];
                DirtBlocks[i].GetComponent<DirtBlock>().SetDirtBlock(FieldPlants[i]);
            }
        }
        FieldHolder.DOAnchorPos(new Vector2(46f, -95.2f), 0.5f);
    }

    public void SetFieldManagerButtons()
    {
 

        for (int i = 0; i < ContractFields.Count; i++)
        {
            if (ContractFields[i].contractStats != null)
            {
                FieldsManagerButtons[i].SetUI(ContractFields[i].contractStats.ContractID);
                FieldsManagerButtons[i].GetComponent<Button>().onClick.AddListener(SetFieldOnButtonClick);

            }
        }
    }

    public void CurrentActiveFieldButton(int fieldButtonID)
    {
        FieldsManagerButtons.ForEach(f => f.GetComponent<Button>().interactable = false);
        FieldsManagerButtons[fieldButtonID].GetComponent<Button>().interactable = true;
        SetFieldManagerButtons();
        UIController.Instance.OpenFieldUI();

    }

    private void SetFieldOnButtonClick()
    {

        var selectedUIObject = EventSystem.current.currentSelectedGameObject;
        if (selectedUIObject == null) return;
        var fieldManager = selectedUIObject.GetComponent<FieldManager>();
        if (fieldManager != null)
        {

            SetField(fieldManager.FieldContractID);
        }
    }

  
    void SetDirBlocks()
    {
        var findFieldDirtBlocks = GameObject.FindGameObjectsWithTag("dirtblock");
        if (findFieldDirtBlocks == null) return;

        foreach (var item in findFieldDirtBlocks)
        {
            DirtBlocks.Add(item.GetComponent<Button>());
        }

        DirtBlocks.ForEach(d => d.gameObject.AddComponent<DirtBlock>());

        for (int i = 0; i < DirtBlocks.Count; i++)
        {
            if (DirtBlocks[i].GetComponent<DirtBlock>() != null)
            {
                DirtBlocks[i].GetComponent<DirtBlock>().blockPlant = FieldPlants[i];

            }
        }
        //for (int i = 0; i < DirtBlocks.Count; i++)
        //{
        //}

        //show the field, check the code again, add loading sign etc

        FieldHolder.DOAnchorPos(new Vector2(46f, -95.2f), 0.5f);
        findFieldDirtBlocks = null;
    }

    public void ResetField()
    {
        UIController.Instance.CloseFieldUI();
     //   UIController.Instance.CloseFieldOnly();
        //  DirtBlocks.Clear();
        // FieldPlants.Clear();
        currentContract = null;
        //  contractController.Test();
    }


    // Update is called once per frame
    void Update()
    {

    }
}
