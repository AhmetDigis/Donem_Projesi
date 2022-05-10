using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class RifleScript : MonoBehaviour
{
    [Header("Settings")]
    float feverFrequency_1;
    public float feverFrequency_2;
    public float range;

    [Header("Settings")]

    public AudioSource[] sounds;

    [Header("Effects")]

    public ParticleSystem[] effects;

    [Header("General Settings")]
    public Camera MyCamera;

    int totalBullet = 300;

    int magazineCapacity = 30;
    int bulletsRemaining;
    float impactStrength = 25;
    public TextMeshProUGUI Rounds_Total;
    public TextMeshProUGUI Bullets_Remaining;

    public Animator playerReload;



    void Start()
    {
        bulletsRemaining = magazineCapacity;
        Rounds_Total.text = totalBullet.ToString();
        Bullets_Remaining.text = magazineCapacity.ToString();
    }


    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            StartCoroutine(ReloadControl(4));
        }



        if (Input.GetKey(KeyCode.Mouse0))
        {

            if (Time.time > feverFrequency_1 && bulletsRemaining != 0)
            {
                rifleFire();
                feverFrequency_1 = Time.time + feverFrequency_2;

            }
            if (bulletsRemaining == 0)
            {

                sounds[1].Play();

            }

        }

    }


    void rifleFire()
    {

        bulletsRemaining--;
        Bullets_Remaining.text = bulletsRemaining.ToString();
        effects[0].Play();
        sounds[0].Play();

        RaycastHit hit;
        if (Physics.Raycast(MyCamera.transform.position, MyCamera.transform.forward, out hit, range))
        {


            if (hit.transform.gameObject.CompareTag("Enemy"))
            {
                hit.transform.gameObject.GetComponent<EnemyController>().healthStatus(impactStrength);
                Instantiate(effects[2], hit.point, Quaternion.LookRotation(hit.normal));
            }
            else
            {
                Instantiate(effects[1], hit.point, Quaternion.LookRotation(hit.normal));

            }


            

        }


    }

    IEnumerator ReloadControl(int secondValue)
    {
        if (bulletsRemaining < magazineCapacity && totalBullet != 0)
        {
            playerReload.SetLayerWeight(4, 1);
            playerReload.SetTrigger("choseReload");
            if (!sounds[2].isPlaying)
            {
                sounds[2].Play();
            }
            yield return new WaitForSeconds(secondValue);
            ReloadTecnikalFunction();


        }

    }

    void ReloadTecnikalFunction()
    {

        if (bulletsRemaining == 0)
        { //mermi yok

            if (totalBullet <= magazineCapacity)
            {
                bulletsRemaining = totalBullet;
                totalBullet = 0;
            }
            else
            {
                totalBullet -= magazineCapacity;
                bulletsRemaining = magazineCapacity;
            }


        }
        else
        { //mermi var

            if (totalBullet <= magazineCapacity)
            {
                int lastBullet = bulletsRemaining + totalBullet;

                if (lastBullet > magazineCapacity)
                {

                    bulletsRemaining = magazineCapacity;
                    totalBullet = lastBullet - magazineCapacity;
                }
                else
                {

                    bulletsRemaining += totalBullet;
                    totalBullet = 0;

                }
            }
            else
            {
                int currentBullet = magazineCapacity - bulletsRemaining;
                totalBullet -= currentBullet;
                bulletsRemaining = magazineCapacity;
            }


        }

        Rounds_Total.text = totalBullet.ToString();
        Bullets_Remaining.text = magazineCapacity.ToString();

    }


}
