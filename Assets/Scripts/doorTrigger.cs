using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorTrigger : MonoBehaviour
{
    public Animator boss;
    public Animator wife;
    public Animator child;

    public GameObject manager;



    private void OnTriggerEnter(Collider other) {
        if(other.tag=="Player"){

            GetComponent<Animator>().SetBool("isOpen",true);
            Destroy (GetComponentInChildren<SphereCollider>());

            boss.GetComponent<Animator>().Play("GameOver");
            wife.GetComponent<Animator>().SetTrigger("ishappy");
            child.GetComponent<Animator>().SetTrigger("ishappy");

            StartCoroutine(GameOver(10));
        }
    }

    IEnumerator GameOver(int secondValue)
    {
        

        yield return new WaitForSeconds(secondValue);

        manager.GetComponent<GameManager>().win();

    }

}
