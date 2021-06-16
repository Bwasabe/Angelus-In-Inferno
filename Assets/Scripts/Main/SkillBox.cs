using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBox : MonoBehaviour
{
    
    public IEnumerator Warning(){
        gameObject.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        gameObject.SetActive(false);
    }
}
