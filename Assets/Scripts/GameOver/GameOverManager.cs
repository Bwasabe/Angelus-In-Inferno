using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameOverManager : MonoBehaviour
{
    [SerializeField]
    private Text textBest = null;
    [SerializeField]
    private Text textScore = null;
    

    void Start()
    {
        textBest.text = string.Format("BEST\n{0}",PlayerPrefs.GetInt("BEST",0));
        textScore.text = string.Format("SCORE\n{0}",PlayerPrefs.GetInt("SCORE",0));
    }
    public void OnClickRetry(){
        SceneManager.LoadScene("Main");
    }
    public void OnClickLobby(){
        SceneManager.LoadScene("Start");
    }
}
