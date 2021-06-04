using UnityEngine;
using UnityEngine.UI;

public class TextColor : MonoBehaviour
{
    private Text textDead = null;
    
    private float r=0;
    void Start()
    {
        textDead = gameObject.GetComponent<Text>();
    }

    void Update()
    {
        ChangeRed();
    }
    private void ChangeRed(){
        if(r == 1)return;
        textDead.color = new Color(r,0,0,1f);
        r += Time.deltaTime/2;
        
    }
}
