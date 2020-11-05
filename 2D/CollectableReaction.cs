using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableReaction : MonoBehaviour
{
    [SerializeField]
    public int _RotateSpeed;
    public AudioSource CollectSound;
    public GameObject ThisCollectable;
    void Update(){
        _RotateSpeed=2;
        transform.Rotate(0,_RotateSpeed,0,Space.World);
    }
    private void OnTriggerEnter(Collider other){
        if(other.tag=="Player"){
            CollectSound.Play();
            Destroy(this.gameObject);
        }
    }
}
