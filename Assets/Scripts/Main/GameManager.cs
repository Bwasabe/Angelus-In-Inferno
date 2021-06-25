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

    [Header("몬스터")]
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


    private GameObject enemy = null;
    private SpriteRenderer spriteRenderer = null;
    private EnemyMove enemyMove = null;
    private bool isExit;
    private bool isBoss;
    public bool isStop = false;
    public int delayCount = 5;
    public int changeCount = 2;
    private float score = 0f;
    private float highScore = 0f;
    private float timeScore = 0f;
    private float purple = 2500;
    private float boss = 20000f;


    private List<int> randomrange = new List<int> { 1, 2, 3, 4, 5, 6 };

    void Start()
    {
        if (!Player) Player = FindObjectOfType<PlayerMove>();
        if (!PoolManager) PoolManager = FindObjectOfType<PoolManager>();
        if (!spriteRenderer) spriteRenderer = GetComponent<SpriteRenderer>();
        if (!enemyMove) enemyMove = FindObjectOfType<EnemyMove>();
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isStop)
            {
                Time.timeScale = 1f;
                Time.fixedDeltaTime = 0.02f * Time.timeScale;
                isStop = false;
                canvas[0].enabled = true;
                canvas[1].enabled = false;
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
    public IEnumerator SpawningFire()
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
            switch (num)
            {

                case 1: enemyX = -2.3015f; break;
                case 2: enemyX = -1.3805f; break;
                case 3: enemyX = -0.4595f; break;
                case 4: enemyX = 0.4615f; break;
                case 5: enemyX = 1.3825f; break;
                case 6: enemyX = 2.3035f; break;

            }
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
            isExit = true;
            purple += 2500;
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

    // private void JudgeEnemy(){
    //     if(isEnemyFire == true){
    //         enemy.GetComponent<SpriteRenderer>().sprite = enemyFireSprite;
    //         enemy.transform.localScale = new Vector2(0.5f,0.5f);
    //         enemy.GetComponent<CircleCollider2D>().radius = 1f;
    //     }
    //     // if(isEnemyPurple == true){
    //     //     enemy.GetComponent<SpriteRenderer>().sprite = enemyFireSprite;
    //     //     enemy.transform.localScale = new Vector2(0.75f,0.75f);
    //     // }
    // }
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
