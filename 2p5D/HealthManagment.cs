using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManagment : MonoBehaviour{
    public static int HealthValue;
    public int InternalHealth;
    public GameObject Heart1;
    public GameObject Heart2;
    public GameObject Heart3;
    // Start is called before the first frame update
    void Start()
    {
       HealthValue=1; 
    }

    // Update is called once per frame
    void Update()
    {
        InternalHealth= HealthValue;
        if(HealthValue ==1){
            Heart1.SetActive(true);
        }
        if(HealthValue ==2){
            Heart2.SetActive(true);
        
         }
         if(HealthValue ==3){
            Heart3.SetActive(true);
         }
        
    }
}
