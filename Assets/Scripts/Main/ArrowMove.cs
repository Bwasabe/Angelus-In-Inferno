using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMove : MonoBehaviour
{
    private Vector2 diff = Vector2.zero;
    private float rotationZ = 180f;
    public bool stopRotation = false;

    void Start(){
        gameObject.SetActive(false);
    }
    void Update(){
        if(stopRotation)return;
        diff =GameManager.Instance.Player.transform.position -transform.position;
        rotationZ = Mathf.Atan2(diff.y,diff.x)*Mathf.Rad2Deg;
        gameObject.transform.rotation = Quaternion.Euler(0f,0f,rotationZ-90f);
    }
}
