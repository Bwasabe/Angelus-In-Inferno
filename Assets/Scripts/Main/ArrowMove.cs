using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMove : MonoBehaviour
{
    private GameManager gameManager = null;
    private Vector2 diff = Vector2.zero;
    private float rotationZ = 180f;
    public bool stopRotation = false;

    void Start(){
        gameManager = FindObjectOfType<GameManager>(); 
        gameObject.SetActive(false);
    }
    void Update(){
        if(stopRotation)return;
        diff =gameManager.Player.transform.position -transform.position;
        rotationZ = Mathf.Atan2(diff.y,diff.x)*Mathf.Rad2Deg;
        gameObject.transform.rotation = Quaternion.Euler(0f,0f,rotationZ-90f);
    }
}
