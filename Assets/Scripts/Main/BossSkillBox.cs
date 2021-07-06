using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkillBox : SkillBox
{
    
    public override IEnumerator Warning()
    {
            gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            gameObject.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            gameObject.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            gameObject.SetActive(false);
    }
}
