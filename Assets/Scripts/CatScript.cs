using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CatScript : MonoBehaviour
{
    
    NavMeshAgent navMesh;

    public GameObject target;

    int isTrigger=0;
    
    // Start is called before the first frame update
    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
    }

     void Update()
    {
        if(isTrigger==0)
            navMesh.SetDestination(target.transform.position);
    }
  


    private void OnTriggerEnter(Collider other) {
       if(other.tag=="Player"){
           isTrigger=1;
           gameObject.GetComponent<Animator>().SetTrigger("isSit");
           navMesh.isStopped = true;

       }
   }


   private void OnTriggerExit(Collider other) {
       
        isTrigger=0;
        gameObject.GetComponent<Animator>().SetTrigger("isWalk");
        navMesh.isStopped = false;
           

       
   }
}
