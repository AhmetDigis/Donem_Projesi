using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class EnemyController : MonoBehaviour
{
    [Header("Other Settings")]
    NavMeshAgent navMesh;
    Animator enemyAnimator;
    GameObject target;
    public GameObject mainTarget; //uzaktan vurulduğunda karakterimiz

    [Header("General Settings")]
    public float fireDistance;
    public float SuspectDistance;
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

    float health;






    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        startingPoint = transform.position;
        StartCoroutine(patrolTimeControl());
        health = 100;

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

            if (canPatrol)
            {
                isPatrol = false;
                patrolTime = StartCoroutine(patrolTimeControl());
                StopCoroutine(patrol);

            }

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

                otherObject.GetComponent<IKAnimationManager>().startHeadFollow(transform); //IKAnimation


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
            Vector3 changePosition = new Vector3(target.transform.position.x, target.transform.position.y + 1.2f, target.transform.position.z);
            Debug.DrawLine(firePoint.transform.position, changePosition, colorBlue);

            if (Time.time > feverFrequency_1)
            {

                if (hit.transform.gameObject.CompareTag("Player"))
                {


                    hit.transform.gameObject.GetComponent<PlayerController>().healthStatus(impactStrength);
                    Instantiate(effects[1], hit.point, Quaternion.LookRotation(hit.normal));



                }
                else 
                {
                   Instantiate(effects[2], hit.point, Quaternion.LookRotation(hit.normal));
                        

                }





                if (!sounds[0].isPlaying)
                {
                    sounds[0].Play();
                    effects[0].Play();
                }

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

                otherObject.GetComponent<IKAnimationManager>().stopHeadFollow(transform); //IKAnimation




                if (enemyAnimator.GetBool("run"))
                {

                    enemyAnimator.SetBool("run", false);
                    enemyAnimator.SetBool("walk", true);
                }
                else
                {
                    enemyAnimator.SetBool("walk", true);
                }




                target = otherObject.gameObject;
                navMesh.SetDestination(target.transform.position);
                isSuspect = true;
                if (canPatrol)
                {
                    StopCoroutine(patrol);
                }


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
                    if (canPatrol)
                    {
                        patrol = StartCoroutine(PatrolTecnicalProcess(patrolControl())); //bunu kaldırırsam yalnızca hedefe odaklanıyor devriyeye geri dönmüyor
                    }

                }




            }

        }


    }


    public void healthStatus(float impact)
    {

        health -= impact;
        //healthBar.fillAmount = health / 100;

        if (!isSuspect)  //beni uzaktan vurdu
        {
            enemyAnimator.SetBool("run", true);

            navMesh.SetDestination(mainTarget.transform.position);

        }



        if (health <= 0)
        {

            enemyAnimator.Play("died");
            Destroy(gameObject, 5f);

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
