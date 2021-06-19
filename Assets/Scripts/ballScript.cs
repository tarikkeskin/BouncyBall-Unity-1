﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballScript : MonoBehaviour
{

    public Rigidbody2D rb;

    private bool isPressed = false;

    public float releaseTime = .15f;

    void Update()
    {
        if(isPressed){
            
            rb.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);


        }
    }
    void OnMouseDown(){
        isPressed =true;
        rb.isKinematic = true;
        //Debug.Log("Mouse click");
    }
    void OnMouseUp (){
        isPressed=false;
        rb.isKinematic = false;

        StartCoroutine(Release());
    }

    IEnumerator Release()
    {
        yield return new WaitForSeconds(releaseTime);

        GetComponent<SpringJoint2D>().enabled=false;    
    }
}
