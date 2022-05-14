using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class IKAnimationManager : MonoBehaviour
{
    public Transform headIKDummy;

    public Transform target;

    public Rig rigscript;


    
    public void startHeadFollow(Transform atarget){
        target = atarget;
        rigscript.weight=1;

    }

    public void stopHeadFollow(Transform atarget){
        if(target = atarget){
            target =null;
            rigscript.weight=0;

        }
        

    }

    // Update is called once per frame
    void Update()
    {
        if(target){

            headIKDummy.transform.position= target.position;
        }
    }
}
