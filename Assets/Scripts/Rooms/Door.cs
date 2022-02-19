using Cinemachine;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Door : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D nextRoom;
    [SerializeField] private CinemachineVirtualCamera vcam;
    [SerializeField] private bool isLast;

    private bool youWin = false;

    [SerializeField] private GameObject youWinTheGame;

    
    private CinemachineConfiner2D confiner;
    void Awake(){
        if (vcam != null)
        {
            this.confiner = vcam.GetComponent<CinemachineConfiner2D>();
            Debug.Log(this.confiner);
        }
        if (youWinTheGame != null){
            youWinTheGame.SetActive(false);
            Debug.Log("SetWin");
        }
    }
    
    private void Update(){
        if (youWin){
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            
            //If Q is hit, quit the game
            if (Input.GetKeyDown(KeyCode.Q))
            {
                print("Application Quit");
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                #else
                    Application.Quit();
                #endif
            }
        }

    }


    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player"){
            Debug.Log("AAAA");
            if (isLast){
                youWinTheGame.SetActive(true);
            }
            this.confiner.m_BoundingShape2D = this.nextRoom;
            StartCoroutine("DisableDoor"); 

        }
    }
    private IEnumerator DisableDoor()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("Disabling Door");
        this.GetComponent<Collider2D>().isTrigger = false;
    }
}
