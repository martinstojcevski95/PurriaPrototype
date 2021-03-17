using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FieldManager : MonoBehaviour
{
    
    public  int FieldContractID;
    public bool isSet;
    // Start is called before the first frame update
    private void Start()
    {

    }

    public void SetUI(int FieldContractID)
    {
        this.FieldContractID = FieldContractID;
        transform.GetComponentInChildren<Text>().text = "Field for contract " + FieldContractID;

    }

    private void Update()
    {
        
    }


}
