using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class StartButtonManager : MonoBehaviour
{
    [Header("캔버스들")]
    [SerializeField]
    private Canvas quitCanvas = null;
    [SerializeField]
    private Canvas mainCanvas = null;
    [SerializeField]
    private Canvas chooseCanvas = null;
    [SerializeField]
    private Canvas[] playerCanvas = null;
    [SerializeField]
    private Canvas[] enemyCanvas = null;
    [SerializeField]
    private Canvas lrCanvas = null;
    [SerializeField]
    private Canvas xCanvas = null;

    [Header("음악 버튼")]
    [SerializeField]
    private SpriteState[] musicSpriteState = null;
    [SerializeField]
    private Sprite[] musicButtonImage = null;
    [SerializeField]
    private Button musicButton = null;

    [Header("버튼")]
    [SerializeField]
    private Button[] lrButton = null;

    [Header("텍스트")]
    [SerializeField]
    private Text[] textPTime = null;
    [SerializeField]
    private Text[] textSecret = null;
    [SerializeField]
    private Text[] textEnemyCount = null;

    private BackgroundMove backgroundMove = null;
    private BackgroundMusic backgroundMusic = null;
    private bool isEsc = false;
    private bool isMute = false;
    private int hour = 0, min = 0, sec = 0;
    private int fireCount = 0, purpleCount = 0, bossCount = 0;
    private int i = 0;
    void Update()
    {
        CheckESC();
    }
    void Start()
    {
        if (!backgroundMove) backgroundMove = FindObjectOfType<BackgroundMove>();
        if (!backgroundMusic) backgroundMusic = FindObjectOfType<BackgroundMusic>();
        quitCanvas.enabled = false;
        chooseCanvas.enabled = false;
        CanvasEnabledFalse();
        xCanvas.enabled = false;
        hour = PlayerPrefs.GetInt("TIMER", 0) / 3600;
        min = (PlayerPrefs.GetInt("TIMER", 0) - (hour * 3600)) / 60;
        //min = (PlayerPrefs.GetInt("Timer", 0) % 3600) / 60;
        //sec = (PlayerPrefs.GetInt("Timer", 0) % 3600) - (min * 60);
        sec = (PlayerPrefs.GetInt("TIMER", 0) - (hour * 3600) - (min * 60));
        fireCount = PlayerPrefs.GetInt("FIRE", 0);
        purpleCount = PlayerPrefs.GetInt("PURPLE", 0);
        bossCount = PlayerPrefs.GetInt("BOSS", 0);

    }
    private void CheckESC()
    {
        if (isEsc && Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(OnOffEsc());
        }
    }
    private IEnumerator OnOffEsc()
    {
        quitCanvas.enabled = true;
        isEsc = true;
        yield return new WaitForSeconds(1.5f);
        isEsc = false;
        quitCanvas.enabled = false;
    }
    public void OnClick()
    {
        //if (isdisappear) return;
        backgroundMove.SetOffsetY();
        SceneManager.LoadScene("Main");
    }
    public void OnClickQuit()
    {
        //if (isdisappear) return;
        Application.Quit();
    }
    public void OnClickTips()
    {
        //if (isdisappear) return;
        xCanvas.enabled = true;
        ExplainCanvas();
    }
    private void ExplainCanvas()
    {
        //isdisappear = true;
        //isdisappear = false;
        mainCanvas.enabled = false;
        chooseCanvas.enabled = true;
        //StartCoroutine(chooseCanvasIdle.StartCanvas());
    }
    public void OnClickPlayer()
    {
        chooseCanvas.enabled = false;
        lrCanvas.enabled = true;
        lrButton[0].enabled = false;
        lrButton[0].image.enabled = false;
        playerCanvas[0].enabled = true;
        UpdateUI();

    }
    public void OnClickLButton()
    {
        lrButton[1].enabled = true;
        lrButton[1].image.enabled = true;
        switch (i)
        {
            case 1:
                lrButton[0].enabled = false;
                lrButton[0].image.enabled = false;
                playerCanvas[1].enabled = false;
                playerCanvas[0].enabled = true;
                break;
            case 2:
                playerCanvas[2].enabled = false;
                playerCanvas[1].enabled = true;
                break;
        }
        i -= 1;
    }
    public void OnClickRButton()
    {
        lrButton[0].enabled = true;
        lrButton[0].image.enabled = true;
        switch (i)
        {
            case 0:
                playerCanvas[0].enabled = false;
                playerCanvas[1].enabled = true;
                break;
            case 1:
                playerCanvas[1].enabled = false;
                playerCanvas[2].enabled = true;
                lrButton[1].enabled = false;
                lrButton[1].image.enabled = false;
                break;
        }
        i += 1;
    }
    public void OnClickXButton()
    {
        if (chooseCanvas.enabled == true)
        {
            chooseCanvas.enabled = false;
            mainCanvas.enabled = true;
            xCanvas.enabled = false;
            xCanvas.enabled = false;
        }
        else
        {
            CanvasEnabledFalse();
            chooseCanvas.enabled = true;
        }
    }
    public void OnClickFireButton()
    {
        UpdateUI();
        chooseCanvas.enabled = false;
        enemyCanvas[0].enabled = true;
    }
    public void OnClickPurpleButton()
    {
        UpdateUI();
        chooseCanvas.enabled = false;
        enemyCanvas[1].enabled = true;
    }
    public void OnClickBossButton()
    {
        UpdateUI();
        chooseCanvas.enabled = false;
        enemyCanvas[2].enabled = true;
    }
    private void CanvasEnabledFalse()
    {
        playerCanvas[0].enabled = false;
        playerCanvas[1].enabled = false;
        playerCanvas[2].enabled = false;
        enemyCanvas[0].enabled = false;
        enemyCanvas[1].enabled = false;
        enemyCanvas[2].enabled = false;
        lrCanvas.enabled = false;
    }
    private void UpdateUI()
    {
        textPTime[0].text = string.Format("플레이시간: {0}시간", PlayerPrefs.GetInt("TIMER", 0) / 3600);
        textPTime[1].text = string.Format("{0}분", (PlayerPrefs.GetInt("TIMER", 0) - (hour * 3600)) / 60);
        textPTime[2].text = string.Format("{0}초", (PlayerPrefs.GetInt("TIMER", 0) - (hour * 3600) - (min * 60)));
        textEnemyCount[0].text = string.Format("{0}회", PlayerPrefs.GetInt("FIRE", 0));
        textEnemyCount[1].text = string.Format("{0}회", PlayerPrefs.GetInt("PURPLE", 0));
        textEnemyCount[2].text = string.Format("{0}회", PlayerPrefs.GetInt("BOSS", 0));
        if (min >= 5)
        {
            textSecret[0].text = string.Format("사실 안젤루스는 원래\n탈모를 치료중이었으며 현재는\n방아쇠를 저주중이라고 합니다");
        }
        if (fireCount >= 5)
        {
            textSecret[1].text = string.Format("현재 파이어는 바이올렛을 몰래\n사모하고 있다고 합니다 지옥에서\n100살차이는 궁합도 안본다고 하네요\n(속닥속닥)");
        }
        if (purpleCount >= 5)
        {
            textSecret[2].text = string.Format("바이올렛은 사실 파이어가 자신을\n좋아하고 있다는 사실을 알고있으며\n현재 매우 고민중이라고 하네요");
        }
        if (bossCount >= 1)
        {
            textSecret[3].text = string.Format("카이세 주니어 3세는\n겜마고 도트장인 '어시온' 에 의해\n태어났으며 현재는 독립중입니다");
        }
    }
    public void OnClickMusic()
    {
        if(!isMute){
            backgroundMusic.StopBackgroundMusic();
            isMute = true;
            
            musicButton.image.sprite = musicButtonImage[1];
            musicButton.spriteState = musicSpriteState[1];
        }
        else if(isMute){
            backgroundMusic.StartBackgroundMusic();
            isMute = false;
            musicButton.image.sprite = musicButtonImage[0];
            musicButton.spriteState = musicSpriteState[0];
        }
    }

}
