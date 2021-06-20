using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBox : MonoBehaviour
{
    void Awake(){
        gameObject.SetActive(false);
    }
    public IEnumerator Warning(){
        gameObject.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        gameObject.SetActive(false);
    }
}
