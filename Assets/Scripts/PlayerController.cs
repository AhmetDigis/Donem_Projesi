using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public Animator playerAnimator;


    private float multiplier = 1;
    private int weaponCount = 3;
    public int lastLayerIndex = 0;

    public static Action<float> OnCameraLookUpDown;

    Rigidbody playerJump;

    public Image healthBar;

    public static float health;

    public GameObject[] weapons;


    void Start()
    {
        health = 100;
    }

    // Update is called once per frame
    void Update()
    {
        float ver = Input.GetAxis("Vertical");
        float hor = Input.GetAxis("Horizontal");
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        float throwGrenade = Input.GetAxis("Fire2");



        if (Input.GetKey(KeyCode.LeftShift))
        {

            multiplier = 2;
        }
        else
        {
            multiplier = 1;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerAnimator.SetTrigger("Jump");


        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            lastLayerIndex = 0;
            SetAnimationLayerForUpperBody(0);

        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {

            lastLayerIndex = 2;
            playerAnimator.SetTrigger("choseRifle");
            SetAnimationLayerForUpperBody(2);


        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            lastLayerIndex = 3;
            playerAnimator.SetTrigger("chosePistol");
            SetAnimationLayerForUpperBody(3);


        }

        if (throwGrenade > 0)
        {

            SetAnimationLayerForUpperBody(1);
            playerAnimator.SetTrigger("throwGrenade");
        }



        playerAnimator.SetFloat("BackAndForward", ver * multiplier);
        playerAnimator.SetFloat("LeftAndRight", hor * multiplier);

        transform.rotation *= Quaternion.Euler(0, mouseX, 0);
        OnCameraLookUpDown(mouseY);

    }

    public void ResetThrowGrenade()
    {
        Debug.Log("bombayı bitiri " + lastLayerIndex);

        SetAnimationLayerForUpperBody(lastLayerIndex);
        //playerAnimator.SetLayerWeight(1,0);

    }


    private void SetAnimationLayerForUpperBody(int aLayerIndex)
    {

        Debug.Log("Upperbody'i setlerken: " + lastLayerIndex);
        for (int i = 0; i < weaponCount; i++)
        {
            playerAnimator.SetLayerWeight(i + 1, 0);
            weapons[i].SetActive(false);
        }
        playerAnimator.SetLayerWeight(aLayerIndex, 1);
        weapons[aLayerIndex - 1].SetActive(true);
    }

    public void healthStatus(float impact){

        health -= impact;
        healthBar.fillAmount = health /100;
    }


}
