using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class EnemyMove : MonoBehaviour
{
    [SerializeField]
    private long score = 0;
    [SerializeField]
    private int hp = 0;
    [SerializeField]
    private float speed = 7f;
    [SerializeField]
    private Slider enemyHpBar = null;

    private GameManager gameManager = null;
    private SkillBox skillBox = null;
    private Collider2D col = null;
    protected SpriteRenderer spriteRenderer = null;
    private bool isRush = false;
    private bool isDead = false;
    private bool isDamaged = false;
    private bool isOverlap = false;
    public  int random = 0;
    
    private int enemyIdx;
    void Start()
    {
        SetVariable();
        if (!gameManager) gameManager = FindObjectOfType<GameManager>();
        if (!spriteRenderer) spriteRenderer = GetComponent<SpriteRenderer>();
        if (!col) col = GetComponent<Collider2D>();
        if (!skillBox) skillBox = FindObjectOfType<SkillBox>();
        skillBox.gameObject.SetActive(false);
    }
    private void SetVariable()
    {
        hp = 11;
        score = 100;
        speed = 3f;
    }

    void Update()
    {
        if (isDead) return;
        ReSpawn();
        if (transform.localPosition.y <= 3f && !isRush)
        {
            isRush = true;
            StartCoroutine(Rush());
        }


        transform.Translate(Vector2.down * speed * Time.deltaTime);

    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;


        if (collision.CompareTag("Bullet"))
        {

            collision.GetComponent<BulletMove>().Despawn();

            if (hp > 1)
            {
                if (isDamaged) return;
                isDamaged = true;
                StartCoroutine(Damaged());
                return;
            }

            isDead = true;
            gameManager.AddScore(score);
            StartCoroutine(Dead());
        }
    }
   

    private IEnumerator Damaged()
    {
        enemyHpBar.value -= 0.1f;
        hp--;
        spriteRenderer.material.SetColor("_Color", new Color(1f, 1f, 1f, 0.5f));
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.material.SetColor("_Color", new Color(0f, 0f, 0f, 0.7f));
        isDamaged = false;
    }
    private IEnumerator Dead()
    {
        spriteRenderer.material.SetColor("_Color", new Color(0f, 0f, 0f, 0f));
        col.enabled = false;
        yield return new WaitForSeconds(0f);     //애니메이션 추가해야함
        Despawn();
    }
    private IEnumerator Rush()
    {
        speed = 0f;
        StartCoroutine(skillBox.Warning());
        for (int i = 0; i < 10; i++)
        {
            spriteRenderer.material.SetColor("_Color", new Color(1f, 0f, 0f, 0.8f));
            yield return new WaitForSeconds(0.05f);
            spriteRenderer.material.SetColor("_Color", new Color(0f, 0f, 0f, 0f));
            yield return new WaitForSeconds(0.05f);
        }
        spriteRenderer.material.SetColor("_Color", new Color(1f, 0f, 0f, 1f));
        yield return new WaitForSeconds(0.3f);
        speed = 30f;

    }
    private void ReSpawn()
    {
        if (transform.localPosition.y < gameManager.MinPosition.y)
        {
            isRush = false;
            spriteRenderer.material.SetColor("_Color", new Color(0f, 0f, 0f, 0f));
            speed = 5f;
            transform.localPosition = new Vector2(transform.localPosition.x, gameManager.MaxPositon.y + 2f);
        }
    }
    public void Despawn()
    {
        isDead = false;
        isRush = false;
        isDamaged = false;
        skillBox.gameObject.SetActive(false);
        SetVariable();
        col.enabled = true;
        transform.SetParent(gameManager.PoolManager.enemyPool.transform, false);
        gameObject.SetActive(false);
        gameManager.SetEnemyPositionDead(enemyIdx);
    }

    public void SetData(int idx){
        enemyIdx = idx;
    }
}
