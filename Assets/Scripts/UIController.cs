using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] ContractController contractController;

    public static UIController Instance;

    [Header("REGISTER AND LOGIN UI SCREENS")]
    public Canvas FullDashboardUI;


    public RectTransform FullDashboard;
    public RectTransform ContractsUI;
    public RectTransform ContractsAuctionUI;
    public RectTransform DronesUI;
    public RectTransform FieldUI;
    public RectTransform WeatherUI;
    public Canvas LogInAndRegisterScreen;
    public RectTransform LoginUI, RegisterUI, LogAndRegInitialButtonsScreen;
    public GameObject FieldOnly;
    public RectTransform DashBoardUI;

    [SerializeField]
    Text currentTime;

    [Header("LOG INFO")]

    public Text LogText;
    [SerializeField]
    public Canvas LogPanel, StaticLogPanel;
    public Canvas DeleteContractDialogConfirmationPanel;
    public Button DeleteDialogYesButton;

    public bool canHideLogText;
    float waitTime;
    public bool isLogged;

    public bool noDataLogIn;
    private void Awake()
    {

        Instance = this;
    }


    #region RegisterAndLogin

    public void OpenRegister()
    {
        LogAndRegInitialButtonsScreen.DOAnchorPos(new Vector2(-800, 0), 0.4f);
        RegisterUI.DOAnchorPos(new Vector2(0, 7.8f), 0.4f);
    }

    public void OpenLogin()
    {
        LogAndRegInitialButtonsScreen.DOAnchorPos(new Vector2(800, 0), 0.4f);
        LoginUI.DOAnchorPos(new Vector2(0, 7.8f), 0.4f);
    }

    public void CloseRegister()
    {
        LogAndRegInitialButtonsScreen.DOAnchorPos(new Vector2(0, 0), 0.4f);
        RegisterUI.DOAnchorPos(new Vector2(-800, 7.8f), 0.4f);
    }

    public void CloseLogin()
    {
        LogAndRegInitialButtonsScreen.DOAnchorPos(new Vector2(0, 0), 0.4f);
        LoginUI.DOAnchorPos(new Vector2(800, 7.8f), 0.4f);
    }

    #endregion

    #region FullDashboard

    public void OpenFullDasobhard()
    {
        ////  DisplayAutomaticLogText(3f, "Loading, please wait");
        //  for (int i = 0; i < ContractController.Instance.Contracts.Count; i++)
        //  {
        //     if(ContractController.Instance.Contracts[i].contractStats.isContractStarted)
        //      {
        //          Debug.Log("amanm be");
        //          //  LogInAndRegisterScreen.enabled = false;
        //          // here the wait time needs to respond to the real wait time later when the data will be loaded from the db

        //        //  StartCoroutine(OpenDashboardAfterLogIn(0, FullDashboard, true));
        //      }
        //  }
        //  

        //  //   StartCoroutine(GG());
    }

    private void Update()
    {
        if (noDataLogIn)
        {
            OpenDashBoard();
            noDataLogIn = false;
        }
        if (isLogged)
        {
            OpenDashBoard();
            isLogged = false;
        }
        //if (ContractController.Instance.Contracts == null) return;
        //for (int i = 0; i < ContractController.Instance.Contracts.Count; i++)
        //{
        //    if (ContractController.Instance.Contracts[i].isContractDataLoaded())
        //        OpenDashBoard();
        //   // yield return new WaitUntil(() => ContractController.Instance.Contracts[i].isContractDataLoaded().Equals(true));
        //    Debug.Log("lobotoby");
        //}
        currentTime.text = DateTime.UtcNow.ToString();
    }

    public void LoginIn()
    {
        LogPanel.enabled = true;
        LogText.text = "LOADING ...";

    }

    public void OpenAuctionUI()
    {
        ContractsAuctionUI.DOAnchorPos(new Vector2(0, 0), 0.4f);
    }

    public void CloseAuctionUI()
    {
        ContractsAuctionUI.DOAnchorPos(new Vector2(0, -1400), 0.4f);
        contractController.countDownCounter = 5;
        contractController.StartCountDown(false);
        contractController.ResetBidding();
    }

    public void OpenDashBoard()
    {

        // StartCoroutine(GG());

        LogInAndRegisterScreen.enabled = false;
        LogText.text = "";
        LogPanel.enabled = false;
        FullDashboardUI.enabled = true;
        FullDashboard.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0f, -159f), 0.5f);

    }
    //   IEnumerator GG()
    //  {
    //LogInAndRegisterScreen.enabled = false;
    // here the wait time needs to respond to the real wait time later when the data will be loaded from the db
    //StartCoroutine(OpenDashboardAfterLogIn(0, FullDashboard, true));


    // }

    /// <summary>
    /// Activates UI overtime
    /// </summary>
    /// <returns></returns>
    IEnumerator OpenDashboardAfterLogIn(float waitTime, RectTransform Screen, bool ScreenVisibility)
    {

        yield return new WaitForSeconds(waitTime);
        LogInAndRegisterScreen.enabled = false;
        LogText.text = "";
        LogPanel.enabled = false;
        FullDashboardUI.enabled = true;
        Screen.DOAnchorPos(new Vector2(0f, -159f), 0.5f);

        // THIS WAS COMMENTED 
        //  ContractController.Instance.GetDataForAllPlantsLinkedWithContracts();
    }

    #endregion

    #region ContractUI

    public void OpenContractsUI()
    {
        ContractsUI.DOAnchorPos(new Vector2(0, 0), 0.4f);
        CloseDronetUI();
        CloseFieldUI();
        CloseWeatherUI();
    }

    public void CloseContractUI()
    {
        ContractsUI.DOAnchorPos(new Vector2(0, -1400), 0.4f);
    }

    #endregion

    #region DroneUI

    public void OpenDroneUI()
    {
        DronesUI.DOAnchorPos(new Vector2(0, 0), 0.4f);
        CloseContractUI();
        CloseFieldUI();
        CloseWeatherUI();
    }
    public void CloseDronetUI()
    {
        DronesUI.DOAnchorPos(new Vector2(0, -1400), 0.4f);
    }

    #endregion

    #region FieldUI

    public void OpenFieldUI()
    {
        FieldUI.DOAnchorPos(new Vector2(0, 0), 0.4f);
        CloseDronetUI();
        CloseContractUI();
        CloseWeatherUI();
        CloseFieldOnly();
    }
    public void CloseFieldUI()
    {
        FieldUI.DOAnchorPos(new Vector2(0, -2537), 0.4f);
        CloseFieldOnly();
    }

    public void CloseFieldOnly()
    {
        FieldOnly.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-1251, -107), 0.4f);
        // 1272
    }
    #endregion

    #region WeatherUI

    public void OpenWeatherUI()
    {
        WeatherUI.DOAnchorPos(new Vector2(0, 0), 0.4f);
        CloseDronetUI();
        CloseFieldUI();
        CloseContractUI();
    }
    public void CloseWeatherUI()
    {
        WeatherUI.DOAnchorPos(new Vector2(0, -1400), 0.4f);
    }


    #endregion

    public void DashBoard()
    {
        CloseDronetUI();
        CloseContractUI();
        CloseWeatherUI();
        CloseFieldUI();
    }

    /// <summary>
    /// Log info
    /// </summary>
    /// <param name="waitTime"></param>
    /// <param name="textDescription"></param>
    public void DisplayAutomaticLogText(float waitTime, string textDescription)
    {
        StartCoroutine(LogTextCoroutine(waitTime, textDescription));
    }
    public void HideAutomaticLogText()
    {

    }
    /// <summary>
    /// Log info coroutine
    /// </summary>
    /// <param name="waitTime"></param>
    /// <param name="textDescription"></param>
    IEnumerator LogTextCoroutine(float waitTime, string textDescription)
    {
        {
            LogPanel.enabled = true;
            LogText.text = textDescription;
            yield return new WaitForSeconds(waitTime);
            LogText.text = "";
            LogPanel.enabled = false;
        }

    }
    public void HideLogTextCoroutine()
    {
        waitTime = 0;
        canHideLogText = false;
        LogText.text = "";
        LogPanel.enabled = false;

    }

    public void DisplayStaticLogText(string textDescription, bool isPanelActive)
    {
        StaticLogPanel.enabled = isPanelActive;
        LogText.text = textDescription;
    }

    public void DeleteContractDialog(bool isPanelActive)
    {
        if (isPanelActive)
        {
            DeleteContractDialogConfirmationPanel.enabled = isPanelActive;
        }
        else
        {
            DeleteContractDialogConfirmationPanel.enabled = isPanelActive;
        }
    }
}
