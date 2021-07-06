using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBox : MonoBehaviour
{
    void Awake()
    {
        gameObject.SetActive(false);
    }
    public virtual IEnumerator Warning()
    {
        for (int i = 0; i < 3; i++)
        {
            gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            gameObject.SetActive(false);
            yield return new WaitForSeconds(0.1f);
        }
            gameObject.SetActive(true);
            yield return new WaitForSeconds(0.4f);
            gameObject.SetActive(false);

    }
}
