using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StartButtonManager : MonoBehaviour
{
    [SerializeField]
    private Canvas quitCanvas = null;
    private bool isEsc = false;
    private BackgroundMove backgroundMove = null;
    void Update(){

    }
    void Start(){
        if(!backgroundMove)backgroundMove = FindObjectOfType<BackgroundMove>();
    }
    private IEnumerator CheckESC(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            isEsc = true;
            yield return new WaitForSeconds(1f);
            isEsc = false;
        }
    }
    public void OnClick(){
        backgroundMove.SetOffsetY();
        SceneManager.LoadScene("Main");
    }
    public void OnClickQuit(){
        Application.Quit();
    }
}
