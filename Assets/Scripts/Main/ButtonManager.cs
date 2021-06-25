using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ButtonManager : MonoBehaviour
{
    [SerializeField]
    private Canvas[] canvas = null;

    private GameManager gameManager = null;
    private PlayerMove playerMove = null;
    void Start()
    {
        if (!gameManager) gameManager = FindObjectOfType<GameManager>();
        if (!playerMove) playerMove = FindObjectOfType<PlayerMove>();
    }
    public void OnClickStop()
    {
        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0f;
        gameManager.isStop = true;
        canvas[0].enabled = false;
        canvas[1].enabled = true;
    }
    public void OnClickReStart()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        gameManager.isStop = false;
        canvas[0].enabled = true;
        canvas[1].enabled = false;
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
