using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameOverManager : MonoBehaviour
{
    [SerializeField]
    private Text textBest = null;
    

    void Start()
    {
        textBest.text = string.Format("BEST\n{0}",PlayerPrefs.GetInt("BEST",0));
    }

    public void OnClickRetry(){
        SceneManager.LoadScene("Main");
    }
}
