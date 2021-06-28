using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ballLevel2 : MonoBehaviour
{

    public Rigidbody2D rb;

    private bool isPressed = false;

    public float releaseTime = .15f;

    Vector2 baslangicPos,random;

    void Start()
    {   
        rb=GetComponent<Rigidbody2D>();
        baslangicPos=transform.position;

    }

    void Update()
    {
        if(isPressed){
            
            rb.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        }
    }
    void OnMouseDown(){
        isPressed =true;
        rb.isKinematic = true;
        rb.constraints=RigidbodyConstraints2D.None;

        
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
        //rb.constraints=RigidbodyConstraints2D.None;  
    }

    void OnCollisionEnter2D(Collision2D col){

        if(col.gameObject.tag=="assagı"){
            //Debug.Log("collison");

            random=new Vector2(-6,-4);
            rb.constraints=RigidbodyConstraints2D.FreezeAll;
            //rb.constraints=RigidbodyConstraints2D.None;  
            GetComponent<SpringJoint2D>().enabled=true;

            rb.bodyType=RigidbodyType2D.Kinematic;
            transform.position=random;
            baslangicPos=random;
        }

    }
    private void OnTriggerEnter2D()
    {
        //Debug.Log("sdf");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        
        scoreScript.scoreValue +=1;
    }
}
