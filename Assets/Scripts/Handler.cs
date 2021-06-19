using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Handler : MonoBehaviour
{
    public GameObject topPaneli;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void playButton(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    public void selectBalls(){
        topPaneli.SetActive(true);

    }
    public void exitButton(){
        topPaneli.SetActive(false);
    }
}
