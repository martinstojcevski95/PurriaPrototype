using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.AI;
using UnityEngine.UI;


public class Contract : MonoBehaviour
{


    public ContractStats contractStats;

    ContractPublicInfo contractPublicInfo;

    public List<Plant> plants = new List<Plant>();

    public GameObject plantprefab;

    public int spawnedPlants;


    public Button CreateContractButton;
    public bool isContractDataLoadedFully;
    public ContractGrid contGrid;
 

    private void Awake()
    {
        var parent = GetComponent<ContractPublicInfo>();// transform.GetChild(2).GetComponent<ContractPublicInfo>();
        contractPublicInfo = parent;
        if (contractPublicInfo != null)
        {
            //  contractPublicInfo.SetContractPublicInfo();
            // contract not started   contractPublicInfo.ContractUIButton.enabled = false;
        }

        //     InstantiatePlants();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void InstantiatePlantsForContract()
    {
        ContractController.Instance.ContractInteractibleButtons(false);

        while (spawnedPlants < InitialPlantsCount)
        {
            spawnedPlants++;
            GameObject plant = Instantiate(plantprefab, gameObject.transform);
            plant.transform.parent = gameObject.transform;
            plants.Add(plant.GetComponent<Plant>());
            spawnedPlants = plants.Count;
        }
    }

    public void InstantiatePlantsOnContractInspection()
    {
        if (this.contractStats != null)
        {
            InstantiatePlantsForContract();
            if (!isContractDataLoadedFully)
            {
                // ContractController.Instance.CurrenctContractActivePlants(true, this);
                RetreivePlantsData();
            }
            else
            {

                Debug.Log("full contract data is already loaded");
            }
            isContractDataLoadedFully = true;
            Debug.Log(this.contractStats.ContractID);
            ContractController.Instance.CurrenctContractActivePlants(true, this);
            FieldsManager.Instance.CurrentActiveFieldButton(contractStats.ContractID);
        }


        //  ContractController.Instance.CurrenctContractActivePlants(true, contractStats.ContractID);
    }


    public void SetContractGrid()
    {
        //contGrid = grid;
        contGrid = ContractController.Instance.Grid;
        if(contGrid != null)
        contGrid.SetGridStats(this);
      //  contractPublicInfo.GridContractID = contGrid.StaticGridID;
    }

    /// <summary>
    /// spawning all fifteens plants for each contract
    /// </summary>
    void InstantiatePlants()
    {
        for (int i = 0; i < InitialPlantsCount; i++)
        {
            GameObject plant = Instantiate(plantprefab, gameObject.transform);
            plant.transform.parent = gameObject.transform;
            plants.Add(plant.GetComponent<Plant>());
            spawnedPlants = plants.Count;
        }

    }

    public void Log()
    {
        for (int i = 0; i < plants.Count; i++)
        {
            Debug.Log(plants[i].plantStats.isPlantInContract);
        }
    }


    /// <summary>
    /// Creating initial contract
    /// </summary>
    public void CreateContract()
    {
        if(ContractController.Instance.isGridsContractChosen)
        {
            InstantiatePlantsForContract();



            contractStats = new ContractStats();
            if (contractStats.isContractStarted == false)
            {
                contractStats.ContractDescription = "test";
                // contractStats.ContractID = ContractController.Instance.ChosenContractIDFroAuction;
                //  contractStats.ContractID = contGrid.StaticGridID;
                contractStats.ContractID = contractPublicInfo.StaticConttractID;
                contractStats.isContractStarted = true;
                // set public info here
                //   gameObject.GetComponent<ContractPublicInfo>().SetPlayerPrefsContractID(contract.contractStats.ContractID, true);
                string serializedJson = JsonUtility.ToJson(contractStats);
                FirebaseReferenceManager.reference.Child("USERS").Child(LogInAndRegister.Instance.UserName).Child("farmdata").Child("contract" + contractStats.ContractID).SetRawJsonValueAsync(serializedJson).ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        Debug.Log("POST REQUEST FAILED  FOR CONTRACT " + contractStats.ContractID);
                        // Handle the error...
                    }
                    else if (task.IsCompleted)
                    {

                        Debug.Log("POST REQUEST SUCCESS  FOR CONTRACT" + contractStats.ContractID);
                        LoadContractDataA();
                        CreatePlantsForContract();
                        ContractController.Instance.ContractInteractibleButtons(true);

                    }
                });
                SetContractGrid();
            }
        }
 
    }



    /// <summary>
    /// Planting plants for the created contract
    /// </summary>
    void CreatePlantsForContract()
    {
        for (int i = 0; i < plants.Count; i++)
        {
            plants[i].SetInitialPlants(contractStats.ContractID, i);
        }
    }



    /// <summary>
    /// Retreiving all plants data for each contract
    /// </summary>
    public void RetreivePlantsData()
    {
        if (contractStats == null)
            return;

        for (int i = 0; i < plants.Count; i++)
        {
            plants[i].GetPlantStatsData(contractStats.ContractID, i);
            plants[i].GetPlantsGrwothFactorsData(contractStats.ContractID, i);
        }

    }

    /// <summary>
    /// Deleting the contract with pop up confirmation yes/no
    /// </summary>
    public void DeleteContract()
    {
        if (contractStats != null)
        {
            FirebaseReferenceManager.reference.Child("USERS").Child(LogInAndRegister.Instance.UserName).Child("farmdata").Child("contract" + contractStats.ContractID).RemoveValueAsync();
            UIController.Instance.DeleteContractDialog(false);
            contractStats = null;
            UIController.Instance.DeleteDialogYesButton.onClick.RemoveAllListeners();
        }

        DeletePlants();
    }

    void DeletePlants()
    {
        for (int i = 0; i < plants.Count; i++)
        {
            plants[i].ClearPlantStats();
        }
    }

    /// <summary>
    /// Passing the deletecontract method to the Yes button in the confirmation dialog
    /// </summary>
    /// <param name="Yes"></param>
    public void OpenDeleteContractDialog()
    {
        UIController.Instance.DeleteDialogYesButton.onClick.AddListener(() => DeleteContract());
        UIController.Instance.DeleteContractDialog(true);
    }


    /// <summary>
    /// Loads data  from db for each contract after the log in 
    /// </summary>
    public void LoadContractDataA()
    {

        FirebaseDatabase.DefaultInstance
           .GetReference("USERS").Child(LogInAndRegister.Instance.UserName).Child("farmdata").Child("contract" + contractPublicInfo.StaticConttractID)
           .GetValueAsync().ContinueWith(task =>
      {
          if (task.IsFaulted)
          {
              Debug.Log("GET REQUEST FAILED FOR CONTRACT " + contractPublicInfo.StaticConttractID);

          }
          else if (task.IsCompleted)
          {
              DataSnapshot snapshot = task.Result;
              contractStats = JsonUtility.FromJson<ContractStats>(snapshot.GetRawJsonValue());
              //  if (snapshot.GetRawJsonValue().Length>=1)
              //  {
              Debug.Log("GET REQUEST SUCCESS FOR CONTRACT " + contractPublicInfo.StaticConttractID + " DATA :  " + snapshot.GetRawJsonValue());
              isDataLoaded = true;
              UIController.Instance.isLogged = true;
              //  }
              //  else
              //   {
              //   Debug.Log("no data brother");
              //   isDataLoaded = false;
              //  }
              //  contractStats = JsonUtility.FromJson<ContractStats>(snapshot.GetRawJsonValue());

          }
      });

    }

    public bool isContractDataLoaded()
    {
        return isDataLoaded;
    }

    bool isDataLoaded;
    int InitialPlantsCount = 30;
    public int StaticID;

    //CLASSES
    [Serializable]
    public class ContractStats
    {
        public int ContractID;
        public bool isContractStarted;
        public string ContractDescription;
    }



}
