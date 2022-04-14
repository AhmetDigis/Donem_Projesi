using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if(other.tag=="Player"){

            GetComponent<Animator>().SetBool("isOpen",true);
            Destroy (GetComponentInChildren<SphereCollider>());

        }
    }

}
