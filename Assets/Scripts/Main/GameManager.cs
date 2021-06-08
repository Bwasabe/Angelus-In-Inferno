using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public Vector2 MinPosition { get; private set; }
    public Vector2 MaxPositon { get; private set; }

    [Header("텍스트")]
    [SerializeField]
    private Text textScore = null;
    [SerializeField]
    private Text textHighScore = null;
    [SerializeField]
    private Text textLife = null;
    [Header("몬스터")]
    [SerializeField]
    private GameObject firePrefab = null;

    private int life = 3;
    private long score = 0;
    private long highScore = 0;
    public PoolManager PoolManager { get; private set; }

    void Awake()
    {
        if (!PoolManager) PoolManager = FindObjectOfType<PoolManager>();
        highScore = PlayerPrefs.GetInt("BEST", 0);
        UpdateUI();
        MinPosition = new Vector2(-2.5f, -4.45f);
        MaxPositon = new Vector2(2.5f, 4.45f);
        StartCoroutine(SpawningFire());
    }

    public void AddScore(long addScore)
    {
        score += addScore;
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("BEST", (int)highScore);
        }
        UpdateUI();
    }
    private void UpdateUI()
    {
        textHighScore.text = string.Format("Best {0}", highScore);
        textScore.text = string.Format("Score {0}", score);
        textLife.text = string.Format("Life {0}", life);
    }
    private IEnumerator SpawningFire()
    {
        float randomX = 0f;
        float spawningDelay = 0f;
        while (true)
        {
            randomX = Random.Range(-2.5f, 2.5f);
            spawningDelay = Random.Range(1f, 1.5f);
            //for(int i = 0 ; i< 1 ; i++){
            Instantiate(firePrefab, new Vector2(randomX, 6f), Quaternion.identity);
            yield return new WaitForSeconds(spawningDelay);
            //}
            yield return new WaitForSeconds(1f);
        }
    }
    public void Dead()
    {
        life--;
        if (life <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }
        UpdateUI();
    }
}
