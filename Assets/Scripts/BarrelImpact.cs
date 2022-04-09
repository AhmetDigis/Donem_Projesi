using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelImpact : MonoBehaviour
{
    
    public float impactStrength;
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision other) {
        
        if(other.gameObject.tag == "Player"){

            other.gameObject.GetComponent<PlayerController>().healthStatus(impactStrength);
            Destroy (GetComponent<BarrelImpact>());
        }
    }
}
