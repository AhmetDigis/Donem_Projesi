using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("General Settings")]
    NavMeshAgent navMesh;
    Animator enemyAnimator;
    GameObject target;
    float fireDistance = 7;
    float SuspectDistance = 10;
    Vector3 startingPoint;

    [Header("Patrol Settings")]
    public GameObject[] PatrolPoints_1;
    public GameObject[] PatrolPoints_2;
    public GameObject[] PatrolPoints_3;
    public bool isPatrol;
    Coroutine patrol;


    bool isFire = false;
    bool isSuspect = false;


    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        startingPoint = transform.position;

    }



    IEnumerator PatrolTecnicalProcess()
    {

        int totalPoint = PatrolPoints_1.Length - 1;
        int initialValue = 0;


        while (true)
        {

            if (Vector3.Distance(transform.position, PatrolPoints_1[initialValue].transform.position) <= 1f)
            {
                if (totalPoint > initialValue)
                {

                    ++initialValue;
                    navMesh.SetDestination(PatrolPoints_1[initialValue].transform.position);

                }
                else
                {
                    navMesh.stoppingDistance = 1;
                    navMesh.SetDestination(startingPoint);
                    if (navMesh.remainingDistance <= 1)
                    {
                        enemyAnimator.SetBool("walk", false);
                        transform.rotation = Quaternion.Euler(0, 180, 0);
                        isPatrol = false;
                        StopCoroutine(patrol);
                    }
                }

            }
            else
            {
                if (totalPoint > initialValue)
                {

                    navMesh.SetDestination(PatrolPoints_1[initialValue].transform.position);

                }
            }

            yield return null;
        }

    }



    void LateUpdate()
    {

        if (isPatrol)
        {
            patrol = StartCoroutine(PatrolTecnicalProcess());
        }

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
                navMesh.isStopped = true;
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


}
