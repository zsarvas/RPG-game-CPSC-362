using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartConsumable : MonoBehaviour
{
 
    [SerializeField]
    public int RotateSpeed;
    public AudioSource _CollectSound;
    public GameObject ThisHeart;
    void Update(){
        RotateSpeed=2;
        transform.Rotate(0,RotateSpeed,0,Space.World);
    }
    private void OnTriggerEnter(Collider other){
    

            _CollectSound.Play();

            HealthManagment.HealthValue+=1;

            Destroy(this.gameObject);
        
    
    }
}
