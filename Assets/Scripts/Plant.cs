using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Plant : MonoBehaviour
{

    public PlantStats plantStats;
    public PlantGrowthFactors plantGrowthFactors;




    // Start is called before the first frame update
    void Start()
    {

    }

    /// <summary>
    /// Initial Plants setting when creating contract
    /// </summary>
    /// <param name="contid"></param>
    /// <param name="plantid"></param>
    public void SetInitialPlants(int contid, int plantid)
    {
      //  var generatedFieldID =  generateID();
        plantStats = new PlantStats();
        plantStats.isDroneAssigned = false;
        plantStats.isPlantPlanted = true;
        plantStats.isPlantInContract = true;
        plantStats.FieldID = contid;// generatedFieldID;
        plantStats.ContractID = contid;
        plantStats.PlantID = plantid;


        plantGrowthFactors = new PlantGrowthFactors();
        plantGrowthFactors.ColorofPlant = 10;
        plantGrowthFactors.HeatofPlant = 10;
        plantGrowthFactors.Height = 10;
        plantGrowthFactors.LeavesQuantity = 10;
        plantGrowthFactors.LeavesWidth = 10;
        plantGrowthFactors.SoilCover = 10;
        plantGrowthFactors.Weight = 10;
        plantGrowthFactors.FieldID = contid;// generatedFieldID;

        string growthFactorsJson = JsonUtility.ToJson(plantGrowthFactors);
        string statsJson = JsonUtility.ToJson(plantStats);

        FirebaseReferenceManager.reference.Child("USERS").Child(LogInAndRegister.Instance.UserName).Child("farmdata").Child("contract" + contid).Child("stats").Child("plant" + plantid).SetRawJsonValueAsync(statsJson);
        FirebaseReferenceManager.reference.Child("USERS").Child(LogInAndRegister.Instance.UserName).Child("farmdata").Child("contract" + contid).Child("growthfactors").Child("plant" + plantid).SetRawJsonValueAsync(growthFactorsJson);
     
        // do the continue with task to chek if the request was good;
    }


    //TODO post request as function so it can be overridden

    public string generateID()
    {
        
        return System.Guid.NewGuid().ToString("N");
    }


    /// <summary>
    /// RemovePlants Data
    /// </summary>
    public void ClearPlantStats()
    {
        plantStats = null;
        plantGrowthFactors = null; //check if growth factors needs to be reset
    }

    /// <summary>
    /// Pass contractID to get the plants STATS data linked with that contract
    /// </summary>
    /// <param name="StaticConttractID"></param>
    /// <param name="plantID"></param>
    /// <param name="DataTypes"></param>
    public void GetPlantStatsData(int StaticConttractID, int plantID)
    {

        FirebaseDatabase.DefaultInstance
           .GetReference("USERS").Child(LogInAndRegister.Instance.UserName).Child("farmdata").Child("contract" + StaticConttractID).Child("stats").Child("plant" + plantID)
           .GetValueAsync().ContinueWith(task =>
           {
               if (task.IsFaulted)
               {
                   // Handle the error...
               }
               else if (task.IsCompleted)
               {
                   DataSnapshot snapshot = task.Result;

                  // Debug.Log(snapshot.GetRawJsonValue());

                   plantStats = JsonUtility.FromJson<PlantStats>(snapshot.GetRawJsonValue());

               }
           });

    }

    /// <summary>
    /// Pass contractID to get the plants GROWTH FACTOR data linked with that contract
    /// </summary>
    /// <param name="StaticConttractID"></param>
    /// <param name="plantID"></param>
    /// <param name="DataTypes"></param>

    public void GetPlantsGrwothFactorsData(int StaticConttractID, int plantID)
    {
        FirebaseDatabase.DefaultInstance
        .GetReference("USERS").Child(LogInAndRegister.Instance.UserName).Child("farmdata").Child("contract" + StaticConttractID).Child("growthfactors").Child("plant" + plantID)
        .GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                Debug.Log(snapshot.GetRawJsonValue());
                plantGrowthFactors = JsonUtility.FromJson<PlantGrowthFactors>(snapshot.GetRawJsonValue());

             //   StartCoroutine(PlantsGrowthFactorsDataCoroutine(snapshot));
            }
        });
    }


    int initialPlants = 15;

    //CLASSES
    [Serializable]
    public class PlantStats
    {
        public bool isDroneAssigned;
        public bool isPlantPlanted;
        public bool isPlantInContract;
        public int PlantID;
        public int FieldID;
        public int ContractID;
        public int GrowthDays;
        public int Tultip;
        public int SoilMoisture;
        public int SoilDensity;
        public int SoilOrganicMaterial;
        public int Fertilizer;
        public int Weed;
        public int Disease;
        public int Toxicity;
        public int Acidity;
    }

    [Serializable]
    public class PlantGrowthFactors
    {
        public int Height;
        public int LeavesQuantity;
        public int LeavesWidth;
        public int Weight;
        public int HeatofPlant;
        public int ColorofPlant;
        public int SoilCover;
        public int FieldID;
    }
}
