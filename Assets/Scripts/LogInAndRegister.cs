using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogInAndRegister : MonoBehaviour
{
    
    [Header("Login And Register")]
    public InputField RegistrationEmail, RegistrationPassword, LogInEmail, LogInPassword;
    public string userID;
    public string UserName;
    public User _User;

    public static LogInAndRegister Instance;

    public string user, password;
    public int AutoLogIn;




    void Awake()
    {
        DontDestroyOnLoad(this);
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(DATABASEURL);
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        AutoLogIn = PlayerPrefs.GetInt("autolog");
        //PlayerPrefs.DeleteKey("autolog");

    }



    // Update is called once per frame
    void Update()
    {
        if (AutoLogIn == 1)
        {
            LogInEmail.text = PlayerPrefs.GetString("user");
            LogInPassword.text = PlayerPrefs.GetString("password");

        }
    }




    /// <summary>
    /// Registering a user in the firebase db
    /// </summary>
    public void Registration()
    {


        auth.CreateUserWithEmailAndPasswordAsync(RegistrationEmail.text, RegistrationPassword.text).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            // Firebase user has been created.
            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
            UserName = newUser.UserId;

            SetUserToDB(newUser.UserId);
            UIController.Instance.CloseRegister();
            UIManager.Instance.OpenLogin();

        });
    }



    /// <summary>
    /// Login a user with the registered credentials from the db
    /// </summary>
    public void LogIn()
    {
        UIController.Instance.LoginIn();
        auth.SignInWithEmailAndPasswordAsync(LogInEmail.text, LogInPassword.text).ContinueWith(task =>
              {
                  if (task.IsCanceled)
                  {
                      Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                      return;
                  }
                  if (task.IsFaulted)
                  {
                      Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                      return;
                  }
                  Firebase.Auth.FirebaseUser newUser = task.Result;
                  Debug.LogFormat("User signed in successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
                  UserName = newUser.UserId;


                  ContractController.Instance.GetDataForAllContracts();
                  ContractController.Instance.GridsData();
              });

        SetLogInCreds();
      //  UIController.Instance.OpenFullDasobhard();
    }


    /// <summary>
    /// Set the user into the db
    /// </summary>
    /// <param name="userID"></param>
    public void SetUserToDB(string userName)
    {
        User user = new User();
        user.UserName = userName;
        user.UserID = userID;
        string ToJson = JsonUtility.ToJson(user);
        reference.Child("USER").SetValueAsync(ToJson);
    }

    /// <summary>
    /// Set the credentials to be remembered for every next log in
    /// </summary>
    void SetLogInCreds()
    {

        PlayerPrefs.SetString("user", LogInEmail.text);
        PlayerPrefs.SetString("password", LogInPassword.text);
        AutoLogIn = 1;
        PlayerPrefs.SetInt("autolog", AutoLogIn);

    }


    /// <summary>
    /// User Class
    /// </summary>
    [Serializable]
    public class User
    {
        public string UserName;
        public string UserID;
    }


    //PRIVATE VARIABLES
    DatabaseReference reference;
    string DATABASEURL = "https://purriafresh.firebaseio.com/";
    FirebaseAuth auth;
}
