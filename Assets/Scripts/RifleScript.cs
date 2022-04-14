using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (Time.time > feverFrequency_1){
                feverFrequency_1 = Time.time + feverFrequency_2;
                rifleFire();
            }
                       
        }

    }


    void rifleFire()
    {
        RaycastHit hit;
        if (Physics.Raycast(MyCamera.transform.position, MyCamera.transform.forward, out hit, range))
        {

            effects[0].Play();
            sounds[0].Play();
            Instantiate(effects[1], hit.point, Quaternion.LookRotation(hit.normal));
            Debug.Log(hit.transform.gameObject.name);

        }


    }

}
