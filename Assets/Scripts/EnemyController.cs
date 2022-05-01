using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Other Settings")]
    NavMeshAgent navMesh;
    Animator enemyAnimator;
    GameObject target;

    [Header("General Settings")]
    float fireDistance = 7;
    float SuspectDistance = 10;
    Vector3 startingPoint;
    bool isSuspect = false;
    bool isFire = false;

    public GameObject firePoint;

    [Header("Patrol Settings")]
    public GameObject[] PatrolPoints_1;
    public GameObject[] PatrolPoints_2;
    public GameObject[] PatrolPoints_3;
    GameObject[] activePatrolList;


    [Header("Gun Settings")]
    float feverFrequency_1;
    public float feverFrequency_2;
    public float range;
    public float impactStrength;

    [Header("Sounds")]

    public AudioSource[] sounds;

    [Header("Effects")]

    public ParticleSystem[] effects;


    bool isPatrol;
    Coroutine patrol;
    Coroutine patrolTime;
    bool patrolLock;
    public bool canPatrol;






    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        startingPoint = transform.position;
        StartCoroutine(patrolTimeControl());

    }

    GameObject[] patrolControl()
    {

        int rendomValue = Random.Range(1, 3);

        switch (rendomValue)
        {

            case 1:
                activePatrolList = PatrolPoints_1;
                break;
            case 2:
                activePatrolList = PatrolPoints_2;
                break;
            case 3:
                activePatrolList = PatrolPoints_3;
                break;

        }
        return activePatrolList;

    }


    IEnumerator patrolTimeControl()
    {

        while (true && !isPatrol && canPatrol)
        {

            yield return new WaitForSeconds(5f);
            patrolLock = true;
            StopCoroutine(patrolTime);

        }


    }




    IEnumerator PatrolTecnicalProcess(GameObject[] incomingObject)
    {
        navMesh.isStopped = false;
        patrolLock = false;
        isPatrol = true;
        enemyAnimator.SetBool("walk", true);
        int totalPoint = incomingObject.Length - 1;
        int initialValue = 0;
        navMesh.SetDestination(incomingObject[initialValue].transform.position);


        while (true && canPatrol)
        {

            if (Vector3.Distance(transform.position, incomingObject[initialValue].transform.position) <= 1f)
            {
                if (totalPoint > initialValue)
                {

                    ++initialValue;
                    navMesh.SetDestination(incomingObject[initialValue].transform.position);

                }
                else
                {
                    navMesh.stoppingDistance = 1;
                    navMesh.SetDestination(startingPoint);

                }

            }
            else
            {
                if (totalPoint > initialValue)
                {

                    navMesh.SetDestination(incomingObject[initialValue].transform.position);

                }
            }

            yield return null;
        }

    }



    void LateUpdate()
    {

        if (navMesh.stoppingDistance == 1 && navMesh.remainingDistance <= 1)
        {
            navMesh.isStopped = false;
            enemyAnimator.SetBool("walk", false);
            transform.rotation = Quaternion.Euler(0, 180, 0);
            isPatrol = false;
            patrolTime = StartCoroutine(patrolTimeControl());
            StopCoroutine(patrol);
            navMesh.stoppingDistance = 0;
            navMesh.isStopped = true;
        }




        if (patrolLock && canPatrol)
        {
            patrol = StartCoroutine(PatrolTecnicalProcess(patrolControl()));


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

                
                
                RifleFire(otherObject.gameObject);


            }
            else
            {
                if (isFire)
                {

                    enemyAnimator.SetBool("fireIdle", false);
                    navMesh.isStopped = false;
                    enemyAnimator.SetBool("walk", true);


                    isFire = false;



                }


            }

        }


    }

    void RifleFire(GameObject target)
    {

        isFire = true;


        Vector3 distanceObjects = target.gameObject.transform.position - transform.position;
        Quaternion rotationObject = Quaternion.LookRotation(distanceObjects, Vector3.up);
        transform.rotation = rotationObject;
        enemyAnimator.SetBool("walk", false);
        navMesh.isStopped = true;
        enemyAnimator.SetBool("fireIdle", true);

        RaycastHit hit;

        if (Physics.Raycast(firePoint.transform.position, firePoint.transform.forward, out hit, range))
        {
            Color colorBlue = Color.blue;
            Vector3 changePosition=new Vector3(target.transform.position.x,target.transform.position.y+1.2f,target.transform.position.z);
            Debug.DrawLine(firePoint.transform.position, changePosition, colorBlue);

            if (Time.time > feverFrequency_1)
            {
                
                hit.transform.gameObject.GetComponent<PlayerController>().healthStatus(impactStrength);
                Instantiate(effects[1], hit.point, Quaternion.LookRotation(hit.normal));
                feverFrequency_1 = Time.time + feverFrequency_2;

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

                enemyAnimator.SetBool("walk", true);
                target = otherObject.gameObject;
                navMesh.SetDestination(target.transform.position);
                isSuspect = true;
                StopCoroutine(patrol);

            }
            else
            {
                if (isSuspect)
                {

                    target = null;


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

                    isSuspect = false;
                    patrol = StartCoroutine(PatrolTecnicalProcess(patrolControl())); //bunu kaldırırsam yalnızca hedefe odaklanıyor devriyeye geri dönmüyor
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
