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
    [SerializeField]
    private Sprite[] changeSkillbutton = null;
    [SerializeField]
    private SpriteState[] spriteStates = null;
    [SerializeField]
    private Button skillButton = null;
    [SerializeField]
    private Button fastButton = null;

    private GameManager gameManager = null;
    private Animator animator = null;
    private IEnumerator coroutine = null;
    private bool isAngel = true;
    private float timer = 0f;
    void Start()
    {
        if (!gameManager) gameManager = FindObjectOfType<GameManager>();
        //textCount.enabled = false;
        canvas[2].enabled = false;
        coroutine = CountDown();
        timer = PlayerPrefs.GetInt("TIMER", 0);
    }
    void Update()
    {
        timer += Time.deltaTime;
        Debug.Log(PlayerPrefs.GetInt("TIMER",0));
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
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        canvas[2].enabled = false;
        canvas[0].enabled = true;

    }
    public void OnClickReset()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        PlayerPrefs.SetInt("TIMER", (int)timer);
        SceneManager.LoadScene("Main");
    }
    public void OnClickExit()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        PlayerPrefs.SetInt("TIMER", (int)timer);
        SceneManager.LoadScene("Start");
    }
    public void FastOnClick()
    {
        if (gameManager.Player.isSkill) return;
        if (gameManager.Player.isAngel)
        {
            if (gameManager.Player.isFastDelay) return;
            gameManager.Player.PlayFastSound();
        }
        else if (gameManager.Player.isDevil)
        {
            if (gameManager.Player.isDSkill) return;
            gameManager.Player.PlayDSkill();
        }
    }
    public void WingOnClick()
    {
        if (gameManager.Player.isDSkill) return;
        if (gameManager.Player.isSkill) return;
        if (isAngel)
        {
            skillButton.spriteState = spriteStates[0];
            skillButton.image.sprite = changeSkillbutton[0];
            fastButton.image.sprite = changeSkillbutton[2];
            fastButton.spriteState = spriteStates[2];
            isAngel = false;
        }
        else
        {
            skillButton.spriteState = spriteStates[1];
            skillButton.image.sprite = changeSkillbutton[1];
            fastButton.spriteState = spriteStates[3];
            fastButton.image.sprite = changeSkillbutton[3];
            isAngel = true;
        }
        gameManager.Player.WingSkill();
    }
}
