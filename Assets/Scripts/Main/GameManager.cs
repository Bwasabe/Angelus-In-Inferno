using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public Vector2 MinPosition { get; private set; }
    public Vector2 MaxPositon { get; private set; }
    public PoolManager PoolManager { get; private set; }
    public PlayerMove Player { get; private set; }
    public BackgroundMusic backgroundMusic {get; private set;}

    [Header("텍스트")]
    [SerializeField]
    private Text textScore = null;
    [SerializeField]
    private Text textHighScore = null;
    [SerializeField]
    private Text textLife = null;
    [SerializeField]
    private Text textFastCount = null;
    [SerializeField]
    private Text textChangeCount = null;

    [Header("적")]
    [SerializeField]
    private GameObject enemyFirePrefab = null;
    [SerializeField]
    private GameObject enemyPurplePrefab = null;
    [SerializeField]
    private GameObject bossPrefab = null;

    [SerializeField]
    private int life = 3;
    [SerializeField]
    private Canvas[] canvas = null;
    [SerializeField]
    private Transform[] enemySpawn = null;

    private GameObject enemy = null;
    private SpriteRenderer spriteRenderer = null;
    private EnemyMove enemyMove = null;
    private EnemyPurple enemyPurple = null;
    private ButtonManager buttonManager = null;
    private bool isExit;
    private bool isBoss;
    public bool isStop = false;
    public bool isCount = false;
    public int delayCount = 5;
    public int changeCount = 2;
    private float score = 0f;
    private float highScore = 0f;
    private float timeScore = 0f;
    private float purple = 2500;
    private float boss = 20000f;
    private int fireCount = 0;
    private int purpleCount = 0;
    private int bossCount = 0;


    private List<int> randomrange = new List<int> { 0, 1, 2, 3, 4, 5 };

    void Start()
    {
        if (!Player) Player = FindObjectOfType<PlayerMove>();
        if (!PoolManager) PoolManager = FindObjectOfType<PoolManager>();
        if (!spriteRenderer) spriteRenderer = GetComponent<SpriteRenderer>();
        if (!enemyMove) enemyMove = FindObjectOfType<EnemyMove>();
        if (!enemyPurple) enemyPurple = FindObjectOfType<EnemyPurple>();
        if (!buttonManager) buttonManager = FindObjectOfType<ButtonManager>();
        if (!backgroundMusic) backgroundMusic = FindObjectOfType<BackgroundMusic>();
        //new Vector2(-2.35f, -4.393f);
        //new Vector2(2.35f, 4.35f);
        highScore = PlayerPrefs.GetInt("BEST", 0);
        SetVariable();
        UpdateUI();
        MinPosition = new Vector2(-Camera.main.aspect * Camera.main.orthographicSize + 0.3f, -Camera.main.orthographicSize);
        MaxPositon = new Vector2(Camera.main.aspect * Camera.main.orthographicSize - 0.3f, Camera.main.orthographicSize);
        StartCoroutine(SpawningFire());
        StartCoroutine(SpawningPurple());
        StartCoroutine(SpawningBoss());
        canvas[0].enabled = true;
        canvas[1].enabled = false;
        fireCount = PlayerPrefs.GetInt("FIRE",0);
        purpleCount = PlayerPrefs.GetInt("PURPLE",0);
        bossCount = PlayerPrefs.GetInt("BOSS",0);
    }
    void Update()
    {
        timeScore = 10 * Time.deltaTime;
        score += timeScore;
        AddScore((long)timeScore);
        UpdateUI();
        JudgeScore();
        OnClickEsc();
    }
    private void OnClickEsc()
    {
        if(isCount)return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isStop)
            {
                isStop = false;
                canvas[1].enabled = false;
                canvas[2].enabled = true;
                StartCoroutine(buttonManager.CountDown());
                return;
            }
            Time.timeScale = 0f;
            Time.fixedDeltaTime = 0f;
            isStop = true;
            canvas[0].enabled = false;
            canvas[1].enabled = true;
        }
    }
    private void SetVariable()
    {
        life = 10;
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
        textHighScore.text = string.Format("Best {0 : 0}", highScore);
        textScore.text = string.Format("Score {0 : 0}", score);
        textLife.text = string.Format("Life {0}", life);
        textFastCount.text = string.Format("{0}", delayCount);
        textChangeCount.text = string.Format("{0}", changeCount);
    }
    private IEnumerator SpawningFire()
    {
        float spawningDelay = 0f;
        float enemyX = 0;

        while (true)
        {
            yield return new WaitWhile(() => isBoss);
            yield return new WaitWhile(() => isExit);
            if (randomrange.Count <= 0)
            {
                yield return new WaitForSeconds(spawningDelay);
                continue;
            }
            int randoma = Random.Range(0, randomrange.Count);
            int num = randomrange[randoma];
            randomrange.RemoveAt(randoma);
            enemyX = enemySpawn[num].position.x;
            spawningDelay = Random.Range(3.8f, 4.3f);

            EnemyFireSpawnOrInstantiate(enemyX, num);
            yield return new WaitForSeconds(spawningDelay);
        }
    }
    public void StopSpawning()
    {
        StopCoroutine(SpawningFire());
        StopCoroutine(SpawningPurple());
    }
    public void Startspawning()
    {
        StartCoroutine(SpawningFire());
        StartCoroutine(SpawningPurple());
    }
    private IEnumerator SpawningPurple()
    {
        while (true)
        {
            yield return new WaitWhile(() => isBoss);
            yield return new WaitUntil(() => isExit);
            yield return new WaitForSeconds(1f);
            EnemyPurpleSpawnOrInstantiate();
            isExit = false;
        }
    }
    private void JudgeScore()
    {
        if (score >= boss)
        {
            isBoss = true;
            boss += 20000;
        }
        if (score >= purple)
        {
            purple += 2500;
            if (isBoss) return;
            isExit = true;
        }
    }

    private void EnemyFireSpawnOrInstantiate(float enemyX, int idx)
    {

        if (PoolManager.enemyPool.transform.childCount > 0)
        {
            enemy = PoolManager.enemyPool.transform.GetChild(0).gameObject;
            //enemy.layer = LayerMask.NameToLayer("Enemy");
            //JudgeEnemy();
            enemy.SetActive(true);
            enemy.transform.rotation = Quaternion.identity;
            enemy.transform.position = new Vector2(enemyX, 6f);

        }
        else
        {
            enemy = Instantiate(enemyFirePrefab, new Vector2(enemyX, 6f), Quaternion.identity);
        }
        if (enemy != null)
        {
            enemy.transform.SetParent(null);
            EnemyMove move = enemy.GetComponent<EnemyMove>();
            move.SetHpBar();
            move.SetData(idx);
        }
    }
    private void EnemyPurpleSpawnOrInstantiate()
    {

        if (PoolManager.enemyPurplePool.transform.childCount > 0)
        {
            enemy = PoolManager.enemyPurplePool.transform.GetChild(0).gameObject;
            //enemy.layer = LayerMask.NameToLayer("Enemy");
            //JudgeEnemy();
            enemy.SetActive(true);
            enemy.transform.rotation = Quaternion.identity;
            enemy.transform.position = new Vector2(-2f, 3.7f);
        }
        else
        {
            enemy = Instantiate(enemyPurplePrefab, new Vector2(2f, 3.7f), Quaternion.identity);
        }
        if (enemy != null)
        {
            enemy.transform.SetParent(null);
            EnemyPurple pMove = enemy.GetComponent<EnemyPurple>();
            pMove.SetHpBar();
        }
    }
    private void BossSpawnOrInstantiate()
    {
        if (PoolManager.bossPool.transform.childCount > 0)
        {
            enemy = PoolManager.bossPool.transform.GetChild(0).gameObject;
            enemy.SetActive(true);
            enemy.transform.rotation = Quaternion.identity;
            enemy.transform.position = new Vector2(0f, 6.71f);
        }
        else
        {
            enemy = Instantiate(bossPrefab, new Vector2(0f, 6.71f), Quaternion.identity);
        }
        if (enemy != null)
        {
            enemy.transform.SetParent(null);
        }
    }
    private IEnumerator SpawningBoss()
    {
        yield return new WaitUntil(() => isBoss);
        yield return new WaitForSeconds(1f);
        BossSpawnOrInstantiate();

    }
    public void FalseBoss()
    {
        isBoss = false;
    }
    public void SetEnemyPositionDead(int idx)
    {
        randomrange.Add(idx);
    }

    public void FireCount()
    {
        fireCount += 1;
        PlayerPrefs.SetInt("FIRE", fireCount);
    }
    public void PurpleCount()
    {
        purpleCount += 1;
        PlayerPrefs.SetInt("PURPLE", purpleCount);
    }
    public void BossCount()
    {
        bossCount += 1;
        PlayerPrefs.SetInt("BOSS", bossCount);
    }
    public void Dead()
    {
        life--;
        if (life <= 0)
        {
            buttonManager.TimerSet();
            PlayerPrefs.SetInt("SCORE", (int)score);
            SceneManager.LoadScene("GameOver");
        }
        UpdateUI();
    }
}
