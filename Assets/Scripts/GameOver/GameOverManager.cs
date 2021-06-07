using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameOverManager : MonoBehaviour
{
    [SerializeField]
    private Text textBest = null;
    

    void Start()
    {
        textBest.text = string.Format("최고기록\n {0}",PlayerPrefs.GetInt("BEST",0));
    }

    public void OnClickRetry(){
        SceneManager.LoadScene("Main");
    }
}
