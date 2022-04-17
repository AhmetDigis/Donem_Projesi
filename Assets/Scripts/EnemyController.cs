using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    NavMeshAgent navMesh;
    Animator enemyAnimator;
    public GameObject target;

    float distance=10;


    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        //enemyAnimator.SetBool("walk", true);
    }


    void LateUpdate()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, distance);
        float nearestDistance = Mathf.Infinity;
        GameObject nearestTarget = null;

        foreach (var otherObject in hitColliders)
        {

            if (otherObject.gameObject.CompareTag("Player"))
            {

                enemyAnimator.SetBool("walk", true);
                navMesh.SetDestination(otherObject.transform.position);

            }


        }


    }

    private void OnDrawGizmos() {
        Gizmos.color=Color.red;
        Gizmos.DrawWireSphere(transform.position, distance);
    }

    void Update()
    {

    }
}
