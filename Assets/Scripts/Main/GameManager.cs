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
    public PlayerMove Player {get;private set;}



    [Header("텍스트")]
    [SerializeField]
    private Text textScore = null;
    [SerializeField]
    private Text textHighScore = null;
    [SerializeField]
    private Text textLife = null;
    [Header("몬스터")]
    [SerializeField]
    private GameObject enemyFirePrefab = null;
    [SerializeField]
    private Sprite enemyFireSprite = null;
    [SerializeField]
    private GameObject enemyPurplePrefab = null;
    [SerializeField]
    private Sprite enemyPurpleSprite = null;

    [SerializeField]
    private int life = 3;


    private GameObject enemy = null;
    private SpriteRenderer spriteRenderer = null;
    private EnemyMove enemyMove = null;
    private bool isEnemyFire = false;
    private bool isEnemyPurple = false;
    private bool isExit = false;
    private float score = 0f;
    private float highScore = 0f;
    private float timeScore = 0f;
    

    private List<int> randomrange = new List<int> {1,2,3,4,5,6};
    

    void Start()
    {
        if (!PoolManager) PoolManager = FindObjectOfType<PoolManager>();
        if(!spriteRenderer)spriteRenderer = GetComponent<SpriteRenderer>();
        if(!enemyMove)enemyMove = FindObjectOfType<EnemyMove>();
        
        highScore = PlayerPrefs.GetInt("BEST", 0);
        SetVariable();
        UpdateUI();
        MinPosition = new Vector2(-2.35f, -4.393f);
        MaxPositon = new Vector2(2.35f, 4.35f);
        isEnemyFire = true;
        StartCoroutine(SpawningFire());
    }
    void Update(){
        timeScore = 10*Time.deltaTime;
        score += timeScore;
        AddScore((long)timeScore);
        UpdateUI();
        if(score >= 3000){
            isEnemyPurple = true;
        }
    }
    private void SetVariable(){
        life = 1000;   
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
    }
    private IEnumerator SpawningFire()
    {
        float spawningDelay = 0f;
        float enemyX = 0;
        
        while (true)
        {
            if(randomrange.Count <= 0){
                yield return new WaitForSeconds(spawningDelay);
                continue;
            }
            int randoma = Random.Range(0,randomrange.Count);
            int num = randomrange[randoma];
            randomrange.RemoveAt(randoma);
            switch(num){

                case 1: enemyX = -2.3015f; break;
                case 2: enemyX = -1.3805f; break;
                case 3: enemyX = -0.4595f; break;
                case 4: enemyX = 0.4615f; break;
                case 5: enemyX = 1.3825f; break;
                case 6: enemyX = 2.3035f; break;

            }                        
            spawningDelay = Random.Range(4f, 5f);
            
            EnemyFireSpawnOrInstantiate(enemyX, num);
            yield return new WaitForSeconds(spawningDelay);
        }
    }
    private void EnemyFireSpawnOrInstantiate(float enemyX, int idx){
        
        if(PoolManager.enemyPool.transform.childCount > 0){
            enemy = PoolManager.enemyPool.transform.GetChild(0).gameObject;
            enemy.layer = LayerMask.NameToLayer("Enemy");
            JudgeEnemy();
            enemy.SetActive(true);
            enemy.transform.rotation = Quaternion.identity;
            enemy.transform.position = new Vector2(enemyX,6f);
            
        }
        else{
            enemy = Instantiate(enemyFirePrefab,new Vector2(enemyX,6f),Quaternion.identity);
        }
        if(enemy != null){
            enemy.transform.SetParent(null);
            EnemyMove move = enemy.GetComponent<EnemyMove>();
            move.SetData(idx);
        }
    }
    private void EnemyPurpleSpawnOrInstantiate(){
        
        if(PoolManager.enemyPool.transform.childCount > 0){
            enemy = PoolManager.enemyPurplePool.transform.GetChild(0).gameObject;
            //enemy.layer = LayerMask.NameToLayer("Enemy");
            //JudgeEnemy();

            enemy.SetActive(true);
            enemy.transform.rotation = Quaternion.identity;
            enemy.transform.position = new Vector2(-2f,3.7f);
         }
        else{
            enemy = Instantiate(enemyFirePrefab,new Vector2(-2f,3.7f),Quaternion.identity);
        }
        if(enemy != null){
            enemy.transform.SetParent(null);
        }
    }
    public void SetEnemyPositionDead(int idx)
    {
        randomrange.Add(idx);
    }
    private void JudgeEnemy(){
        if(isEnemyFire == true){
            enemy.GetComponent<SpriteRenderer>().sprite = enemyFireSprite;
            enemy.transform.localScale = new Vector2(0.5f,0.5f);
            enemy.GetComponent<CircleCollider2D>().radius = 1f;
        }
        // if(isEnemyPurple == true){
        //     enemy.GetComponent<SpriteRenderer>().sprite = enemyFireSprite;
        //     enemy.transform.localScale = new Vector2(0.75f,0.75f);
        // }
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
