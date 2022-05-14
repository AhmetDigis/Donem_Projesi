using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class doorTrigger : MonoBehaviour
{
    public Animator boss;
    public Animator wife;
    public Animator child;

    public GameObject manager;
    public GameObject targetGrabPoint;

    


    private void OnTriggerEnter(Collider other) {
        if(other.tag=="Player"){

            GetComponent<Animator>().SetBool("isOpen",true);
           

            boss.GetComponent<Animator>().Play("GameOver");
            wife.GetComponent<Animator>().SetTrigger("ishappy");
            child.GetComponent<Animator>().SetTrigger("ishappy");
            
            
            other.GetComponent<IKAnimationManager>().GrabItem(targetGrabPoint);
            
            
            StartCoroutine(GameOver(10));

            
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag=="Player"){

            other.GetComponent<IKAnimationManager>().ReleaseItem(targetGrabPoint);
            Destroy(GetComponent<SphereCollider>());

            
        }
    }

    
    IEnumerator GameOver(int secondValue)
    {
        

        yield return new WaitForSeconds(secondValue);

        manager.GetComponent<GameManager>().win();

    }

}
