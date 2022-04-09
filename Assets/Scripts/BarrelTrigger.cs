using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelTrigger : MonoBehaviour
{
   public Rigidbody[] barrels;

   public float forceMagnitute;

   private int barrelCount=6;

 

   private void OnTriggerEnter(Collider other) {
       if(other.tag=="Player"){

            for (int i = 0; i < barrelCount; i++){

               barrels[i].useGravity=true;
               barrels[i].AddForce(barrels[i].transform.up * forceMagnitute);
               gameObject.SetActive(false);
           }

       }
   }
}
