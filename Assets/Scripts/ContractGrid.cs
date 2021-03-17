using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContractGrid : MonoBehaviour
{
    [SerializeField]
    Color pickingColor;
    public GridStats gridStats;
    // Start is called before the first frame update
    void Start()
    {

        //   gridStats.GridText = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetGrid(int staticGrid)
    {
        if (gridStats != null)
        {
            StaticGridID = staticGrid;
            gridStats.GridID = staticGrid;
            //    gridStats.GridText.text = "Contract " + StaticGridID;


        }
    }

    public void CheckCurrentGridStatus()
    {

        if (gridStats != null)
        {
            if (gridStats.isGridaken == true)
            {
                Debug.Log("grid used" + StaticGridID);
            }
        }
        else
        {

            // CHECK THIS
            Debug.Log("grid not used" + StaticGridID);
            OnCurrentChosenGrid();
        }
           

    }

    void OnCurrentChosenGrid()
    {
        ContractController.Instance.isGridsContractChosen = true;
    }


    public void SetGridStats(Contract contract)
    {
        gridStats = new GridStats();

        //  gridStats.GridID = StaticGridID;
        gridStats.isGridaken = true;
        gridStats.LinkedGridContractID = contract.contractStats.ContractID;
        string serializedJson = JsonUtility.ToJson(gridStats);
        FirebaseReferenceManager.reference.Child("USERS").Child(LogInAndRegister.Instance.UserName).Child("farmdata").Child("contract" + contract.contractStats.ContractID).Child("GridStats").SetRawJsonValueAsync(serializedJson).ContinueWith(task =>
        {
            if (task.IsFaulted)
            {

                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                Debug.Log(gridStats);

            }
        });

    }

    public void GetGridData(int contrctID)
    {
        FirebaseDatabase.DefaultInstance.GetReference("USERS").Child(LogInAndRegister.Instance.UserName).Child("farmdata").Child("contract" + contrctID).Child("GridStats")
        .GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                //  Debug.Log("GET REQUEST FAILED FOR CONTRACT " + contractPublicInfo.StaticConttractID);

            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                Debug.Log(snapshot.GetRawJsonValue());
                gridStats = JsonUtility.FromJson<GridStats>(snapshot.GetRawJsonValue());

                //  contractStats = JsonUtility.FromJson<ContractStats>(snapshot.GetRawJsonValue());
                //  if (snapshot.GetRawJsonValue().Length>=1)
                //  {
                //   Debug.Log("GET REQUEST SUCCESS FOR CONTRACT " + contractPublicInfo.StaticConttractID + " DATA :  " + snapshot.GetRawJsonValue());
                // isDataLoaded = true;
                //  UIController.Instance.isLogged = true;
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


    //PUBLIC VARIABLES
    public int StaticGridID;

    [Serializable]
    public class GridStats
    {
        //  public int StaticGridID;
        // public Text GridText;
        public bool isGridaken;
        public int GridID;
        public int LinkedGridContractID;
    }

}
