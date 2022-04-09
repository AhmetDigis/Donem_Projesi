using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningTrigger : MonoBehaviour
{
   
   public GameObject warningMessage;
   
   private void OnTriggerEnter(Collider other) {
        if(other.tag=="Player"){

            warningMessage.SetActive(true);

        }
    }

    
    private void OnTriggerExit(Collider other)
    {
        warningMessage.SetActive(false);
    }
   
}
