using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ButtonManager : MonoBehaviour
{
    [SerializeField]
    private Canvas[] canvas = null;
    [SerializeField]
    private Text textCount = null;

    private GameManager gameManager = null;
    private PlayerMove playerMove = null;
    private Animator animator = null;
    private IEnumerator coroutine = null;
    void Start()
    {
        if (!gameManager) gameManager = FindObjectOfType<GameManager>();
        if (!playerMove) playerMove = FindObjectOfType<PlayerMove>();
        //textCount.enabled = false;
        canvas[2].enabled = false;
        coroutine = CountDown();
    }
    public void OnClickStop()
    {
        StopCoroutine(coroutine);
        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0f;
        textCount.text = string.Format("3");
        gameManager.isStop = true;
        canvas[0].enabled = false;
        canvas[1].enabled = true;
    }
    public void OnClickBack()
    {
        gameManager.isStop = false;
        canvas[2].enabled = true;
        canvas[1].enabled = false;
        StartCoroutine(CountDown());
    }
    public IEnumerator CountDown()
    {
        // for(int i = 3 ; i>0 ; i--){
        //     textCount.text = string.Format("{0}",i);
        //     yield return new WaitForSecondsRealtime(1f);
        // }
        // textCount.text = string.Format("3");
        // Time.timeScale = 1f;
        // Time.fixedDeltaTime = 0.02f * Time.timeScale;
        // canvas[1].enabled = false;
        for (int i = 3; i > 0; i--)
        {
            float k = 1;
            textCount.text = string.Format("{0}", i);
            for (int j = 100; j > 1; j--)
            {
                textCount.transform.localScale = new Vector2(k, k);
                yield return new WaitForSecondsRealtime(0.01f);
                k -= 0.01f;
            }
        }
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f*Time.timeScale;
        canvas[2].enabled = false;
        canvas[0].enabled = true;

    }
    public void OnClickReset()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        SceneManager.LoadScene("Main");
    }
    public void OnClickExit()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        SceneManager.LoadScene("Start");
    }
    public void FastOnClick()
    {
        if (playerMove.isSkill) return;
        if (playerMove.isFastDelay) return;
        playerMove.PlayFastSound();
    }
    public void WingOnClick()
    {
        if (playerMove.isSkill) return;
        playerMove.WingSkill();
    }
}
