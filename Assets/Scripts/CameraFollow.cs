using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraFollow : MonoBehaviour {
    public Transform target;
    public float followOffsetBack = 3f;
    public float followOffsetUp = 1.95f;
    public float followOffsetRight = 0;
    private float lookUpDownOffset;

    
    private void OnEnable() {
        PlayerController.OnCameraLookUpDown+=LookUpDown;
    }

    private void OnDisable() {
        PlayerController.OnCameraLookUpDown-=LookUpDown;
    }

    
    private void Update() {
        transform.position = target.position + Vector3.up * followOffsetUp + target.right * followOffsetRight + (-target.forward * followOffsetBack);
        transform.rotation = Quaternion.LookRotation(target.forward, target.up) * Quaternion.Euler(lookUpDownOffset, 0 , 0);;
    }

    public void LookUpDown(float value) {
        lookUpDownOffset += -value; 
    }

}
