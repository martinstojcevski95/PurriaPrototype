using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ContractController : MonoBehaviour
{

    [SerializeField]
    FieldsManager fieldsManager;


    [SerializeField]
    List<ContractGrid> ContractsGrid = new List<ContractGrid>();
    [SerializeField]
    Color pickingColor;
    // Sprite pickingImg,defaultImage;

    public int ChosenContractIDFroAuction;
    public bool isGridSelected;


    bool isCountDownStarted;
    public bool isBidding;
    public float countDownCounter;
    float gridRefreshTime;
    [SerializeField]
    Text countDownText;
    int gridSelector;



    public Contract CurrentContract;
    public ContractGrid Grid;
    public bool isGridsContractChosen;
    private void Awake()
    {
        Instance = this;

        SetContractsGrid();
    }

    // Start is called before the first frame update
    void Start()
    {

        //var test = ContractsGrid.(c => c.contractStats.isContractStarted);

    }

    public void StartCountDown(bool isOn)
    {
        isCountDownStarted = isOn;

    }

    public void ToAuction()
    {

    }

    public void ResetBidding()
    {
        ContractsGrid.ForEach(c => c.GetComponent<Image>().color = Color.white);
        ContractsGrid.ForEach(c => c.GetComponent<Button>().interactable = false);
        isBidding = false;
        gridSelector = 0;
    }

    // Update is called once per frame
    void Update()
    {
        countDownText.text = "Auction bidding starts in - " + countDownCounter.ToString("F0");
        if (isCountDownStarted)
        {
            if (countDownCounter > 0)
            {
                countDownCounter -= Time.deltaTime;
            }
            else
            {

                countDownCounter = 5;
                isCountDownStarted = false;
                isBidding = true;
            }
        }
        if (isBidding)
        {

            countDownText.text = "Bid now!";
            if (gridRefreshTime < 2)
            {
                gridRefreshTime += Time.deltaTime;
            }
            else
            {
                if (gridSelector <= ContractsGrid.Count)
                {

                    ContractsGridLoop();
                }
                else
                    isBidding = false;
            }

        }

    }

    void SetContractsGrid()
    {
        for (int i = 0; i < ContractsGrid.Count; i++)
        {
            ContractsGrid[i].SetGrid(i);
            //  ContractsGrid[i].GetComponentInChildren<Text>().text = "Contract " + i;
            //  ContractsGrid[i].StaticID = i;
            //   ContractsGrid[i].GetComponent<ContractPublicInfo>().StaticConttractID = i;
        }
        isCountDownStarted = false;
        countDownText.text = "";
        gridSelector = 0;
        countDownCounter = 5;
        gridRefreshTime = 0;
        ContractsGrid.ForEach(c => c.GetComponent<Button>().interactable = false);
    }


    void ContractsGridLoop()
    {

        //if (ContractsGrid[gridSelector].gridStats.isGridaken)
        //{
        //    ContractsGrid[gridSelector].GetComponent<Button>().interactable = false;
        //    ContractsGrid[gridSelector].GetComponent<Image>().color = Color.white;
        //    gridSelector = gridSelector += 1;

        //    //set notification that auction field grid is started
        //}
        //else
        //{
        //    gridSelector += 1;
        //}

        //ContractsGrid.ForEach(c => c.GetComponent<Image>().color = Color.white);
        //ContractsGrid.ForEach(c => c.GetComponent<Button>().interactable = false);
        //ContractsGrid[gridSelector].GetComponent<Button>().interactable = true;
        //ContractsGrid[gridSelector].GetComponent<Image>().color = pickingColor;
        ////    gridSelector += 1;
        //gridRefreshTime = 0;


        ContractsGrid.ForEach(c => c.GetComponent<Image>().color = Color.white);
        ContractsGrid.ForEach(c => c.GetComponent<Button>().interactable = false);
        ContractsGrid[gridSelector].GetComponent<Button>().interactable = true;
        ContractsGrid[gridSelector].GetComponent<Image>().color = pickingColor;
        gridSelector += 1;
        gridRefreshTime = 0;



    }


    //public void SetGrid(Contract currentContract)
    //{
    //    if(currentContract != null)
    //    {
    //        currentContract.SetContractGrid(Grid);
    //    }
    //}

    public void LinkCurrentGridWithContract()
    {
        var selectedGrid = EventSystem.current.currentSelectedGameObject.GetComponent<ContractGrid>();
        // ChosenContractIDFroAuction = selectedGrid.StaticGridID;
        Grid = selectedGrid;

        //  isGridSelected = true;
    }

    public void RemoveStartedGridContract(int GridNumber)
    {
        ContractsGrid.RemoveAt(GridNumber);
    }


    /// <summary>
    /// Getting the db data for each started contract
    /// </summary>
    public void GetDataForAllContracts()
    {
        foreach (var item in Contracts)
        {
            item.LoadContractDataA();
        }
    }

    /// <summary>
    /// Retreiving all contracts plants data only
    /// </summary>
    public void GetDataForAllPlantsLinkedWithContracts()
    {
        for (int i = 0; i < Contracts.Count; i++)
        {
            Contracts[i].RetreivePlantsData();

        }
    }

    public void GridsData()
    {
        for (int i = 0; i < ContractsGrid.Count; i++)
        {
            ContractsGrid[i].GetGridData(i);
        }
    }

    public void CurrenctContractActivePlants(bool setActive, Contract currentContract)
    {
        //int plantsCount = Contracts[contractID].plants.Count;
        //int contractCounts = Contracts.Count;
        //Debug.Log(plantsCount + " " + contractCounts);
        //for (int i = 0; i < Contracts.Count; i++)
        //{


        //}
        //Contracts[contractCounts].plants[plantsCount].gameObject.SetActive(false);
        //Contracts[contractID].plants[plantsCount].gameObject.SetActive(true);



        Contracts.ForEach(x => x.plants.ForEach(p => p.gameObject.SetActive(false)));
        currentContract.plants.ForEach(p => p.gameObject.SetActive(setActive));
        currentContract = null;

    }

    public void SeachForNotStartedContracts()
    {


    }


    public void Test()
    {
        Contracts.ForEach(x => x.plants.ForEach(p => p.gameObject.SetActive(false)));

    }

    public void ContractInteractibleButtons(bool isActive)
    {
        if (Contracts != null)
            Contracts.ForEach(c => c.CreateContractButton.interactable = isActive);
    }

    //PUBLIC VARIABLES 
    public static ContractController Instance;
    public List<Contract> Contracts = new List<Contract>();

}
