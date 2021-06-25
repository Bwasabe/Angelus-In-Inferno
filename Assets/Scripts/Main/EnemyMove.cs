using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnemyMove : MonoBehaviour
{
    [SerializeField]
    protected long score = 0;
    [SerializeField]
    protected int hp = 0;
    [SerializeField]
    protected float speed = 7f;
    [SerializeField]
    protected Slider enemyHpBar = null;
    [Header("아이템")]
    [SerializeField]
    private GameObject fastItemPrefab = null;
    [SerializeField]
    private Sprite fastItemSprite = null;
    [SerializeField]
    private GameObject changeItemPrefab = null;
    [SerializeField]
    private Sprite changeItemSprite = null;
    [Header("소리")]
    [SerializeField]
    private AudioClip clip = null;


    protected GameManager gameManager = null;
    private PlayerMove playerMove = null;
    private SkillBox skillBox = null;
    private ItemMove itemMove = null;
    private AudioSource audioSource = null;
    protected Collider2D col = null;
    protected SpriteRenderer spriteRenderer = null;
    protected GameObject item = null;
    private bool isRush = false;
    protected bool isDead = false;
    protected int random = 0;



    private int enemyIdx;
    void Awake()
    {
        SetVariable();
        SetHpBar();
        if (!gameManager) gameManager = FindObjectOfType<GameManager>();
        if (!spriteRenderer) spriteRenderer = GetComponent<SpriteRenderer>();
        if (!col) col = GetComponent<Collider2D>();
        if (!skillBox) skillBox = FindObjectOfType<SkillBox>();
        if (!playerMove) playerMove = FindObjectOfType<PlayerMove>();
        if (!audioSource) audioSource = GetComponent<AudioSource>();
        //skillBox.gameObject.SetActive(false);

    }
    private void OnEnable()
    {
        SetHpBar();
    }
    protected virtual void SetVariable()
    {
        hp = 11;
        score = 200;
        speed = 3f;
    }
    protected virtual void SetHpBar()
    {
        if (!enemyHpBar) return;
        enemyHpBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, -0.6f, 0));
    }

    void Update()
    {
        SetHpBar();
        if (isDead) return;
        ReSpawn();
        if (transform.localPosition.y <= 3f && !isRush)
        {
            isRush = true;
            StartCoroutine(Rush());
        }

        transform.Translate(Vector2.down * speed * Time.deltaTime);

    }
    private void Move()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        if (collision.CompareTag("Boss"))
        {
            Despawn();
        }
        if (collision.CompareTag("Skill"))
        {
            StartCoroutine(Damanged());
        }
        if (collision.CompareTag("Bullet"))
        {

            collision.GetComponent<BulletMove>().Despawn();

            if (hp > 1)
            {
                StartCoroutine(Damanged());
                return;
            }

            isDead = true;
            gameManager.AddScore(score);
            Dead();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Skill"))
        {
            if (isDead) return;
            if (hp > 1)
            {
                StartCoroutine(Damanged());
                return;
            }
            isDead = true;
            gameManager.AddScore(score);
            Dead();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Skill"))
        {
            StopCoroutine(Damanged());
        }
    }
    private IEnumerator Damanged()
    {
        HpMinus();
        hp--;
        spriteRenderer.material.SetColor("_Color" , new Color(1f,1f,1f,0f));
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.material.SetColor("_Color",new Color(0f,0f,0f,0f));

    }
    protected virtual void HpMinus()
    {
        enemyHpBar.value -= 0.091f;
    }

    private void Dead()
    {
        spriteRenderer.material.SetColor("_Color", new Color(0f, 0f, 0f, 0f));
        col.enabled = false;
        audioSource.PlayOneShot(clip);
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
    protected virtual void Despawn()
    {
        RandomItemDrop();
        isDead = false;
        isRush = false;
        skillBox.gameObject.SetActive(false);
        SetVariable();
        enemyHpBar.value = 1f;
        col.enabled = true;
        transform.SetParent(gameManager.PoolManager.enemyPool.transform, false);
        gameObject.SetActive(false);
        gameManager.SetEnemyPositionDead(enemyIdx);
    }
    protected void RandomItemDrop()
    {
        int random = Random.Range(1, 11);
        if (random <= 1)
        {
            ItemSpawnOrInstantiate();
        }
        else
        {
            return;
        }

    }

    protected void ItemSpawnOrInstantiate()
    {
        if (gameManager.PoolManager.fastSkillPool.transform.childCount > 0)
        {
            item = gameManager.PoolManager.fastSkillPool.transform.GetChild(0).gameObject;
            //enemy.layer = LayerMask.NameToLayer("Enemy");
            //JudgeItem();
            item.SetActive(true);
            item.transform.rotation = Quaternion.identity;
            item.transform.position = transform.localPosition;
        }
        else
        {
            //JudgeItem();
            //item = Instantiate(changeItemPrefab, transform.localPosition, Quaternion.identity);
            item = Instantiate(fastItemPrefab, transform.localPosition, Quaternion.identity);

        }
        if (item != null)
        {
            item.transform.SetParent(null);
        }

    }

    public void SetData(int idx)
    {
        enemyIdx = idx;
    }

}
