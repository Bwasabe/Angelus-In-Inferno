using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StartButtonManager : MonoBehaviour
{
    private BackgroundMove backgroundMove = null;
    void Start(){
        if(!backgroundMove)backgroundMove = FindObjectOfType<BackgroundMove>();
    }
    public void OnClick(){
        backgroundMove.SetOffsetY();
        SceneManager.LoadScene("Main");
    }
}
