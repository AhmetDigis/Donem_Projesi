using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    NavMeshAgent navMesh;
    Animator enemyAnimator;
    GameObject target;
    float fireDistance = 7;
    float SuspectDistance = 10;

    Vector3 startingPoint;
    bool isFire = false;
    bool isSuspect = false;


    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        //enemyAnimator.SetBool("walk", true);
        startingPoint = transform.position;
    }


    void LateUpdate()
    {
        SuspectRange();
        FiringRange();

    }

    void FiringRange()
    {

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, fireDistance);


        foreach (var otherObject in hitColliders)
        {

            if (otherObject.gameObject.CompareTag("Player"))
            {
                enemyAnimator.SetBool("walk", false);
                navMesh.isStopped=true;
                enemyAnimator.SetBool("fireIdle", true);

            }
            else
            {               
                enemyAnimator.SetBool("fireIdle", false);
                
            }

        }


    }


    void SuspectRange()
    {

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, SuspectDistance);


        foreach (var otherObject in hitColliders)
        {

            if (otherObject.gameObject.CompareTag("Player"))
            {
                isSuspect = true;
                enemyAnimator.SetBool("walk", true);
                target = otherObject.gameObject;
                navMesh.SetDestination(otherObject.transform.position);

            }
            else
            {
                target = null;
                isSuspect = false;
                if (transform.position != startingPoint)
                {

                    navMesh.stoppingDistance = 1;
                    navMesh.SetDestination(startingPoint);
                    if (navMesh.remainingDistance <= 1)
                    {
                        enemyAnimator.SetBool("walk", false);
                        transform.rotation = Quaternion.Euler(0, 180, 0);
                    }

                }

            }

        }


    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fireDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, SuspectDistance);
    }

    void Update()
    {

    }
}
