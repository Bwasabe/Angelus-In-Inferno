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
    [Header("������")]
    [SerializeField]
    private GameObject fastItemPrefab = null;

    [Header("����")]
    [SerializeField]
    private AudioClip clip = null;

    [SerializeField]
    private GameObject devilBulletPrefab = null;

    protected BossSkillBox bossSkillBox = null;
    private PlayerMove playerMove = null;
    private SkillBox skillBox = null;
    private ItemMove itemMove = null;
    private AudioSource audioSource = null;
    protected Collider2D col = null;
    protected SpriteRenderer spriteRenderer = null;
    protected GameObject item = null;
    protected GameObject devilBullet = null;
    private bool isRush = false;
    protected bool isDead = false;
    private bool isDamaged = false;
    protected int random = 0;

    protected GameManager _gameManager;


    private int enemyIdx;
    void Awake()
    {
        SetVariable();
        SetHpBar();
        if (!spriteRenderer) spriteRenderer = GetComponent<SpriteRenderer>();
        if (!col) col = GetComponent<Collider2D>();
        if (!skillBox) skillBox = FindObjectOfType<SkillBox>();
        if (!playerMove) playerMove = FindObjectOfType<PlayerMove>();
        if (!audioSource) audioSource = GetComponent<AudioSource>();
        if (!bossSkillBox) bossSkillBox = FindObjectOfType<BossSkillBox>();

        _gameManager = FindObjectOfType<GameManager>();
        //skillBox.gameObject.SetActive(false);

    }
    protected virtual void SetVariable()
    {
        hp = 12;
        score = 200;
        speed = 3f;
    }
    public virtual void SetHpBar()
    {
        enemyHpBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, -0.6f, 0));
    }

    protected virtual void Update()
    {
        SetHpBar();
        if (isDead) return;
        ReSpawn();
        if (transform.localPosition.y <= 3f && !isRush)
        {
            isRush = true;
            StartCoroutine(Rush());
        }

        Move();

    }
    protected virtual void Move()
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
            StartCoroutine(Damaged());
        }
        if (collision.CompareTag("Bullet"))
        {

            collision.GetComponent<BulletMove>().Despawn();
            if ((_gameManager.Player.isDSkill))
            {
                DevilBulletInstantiateOrSpawn();
            }
            if (hp > 1)
            {
                StartCoroutine(Damaged());
                return;
            }

            isDead = true;
            _gameManager.AddScore(score);
            Dead();
        }
        else if (collision.CompareTag("DBullet"))
        {

            if (hp > 1)
            {
                StartCoroutine(DevilDamaged());
                return;
            }

            isDead = true;
            _gameManager.AddScore(score);
            Dead();
        }

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isDead) return;
        if (collision.CompareTag("Skill"))
        {
            if (isDamaged) return;
            if (hp > 1)
            {
                isDamaged = true;
                StartCoroutine(Damaged());
                return;
            }
            isDead = true;
            _gameManager.AddScore(score);
            Dead();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Skill"))
        {
            StopCoroutine(Damaged());
        }
    }
    private IEnumerator Damaged()
    {
        HpMinus();
        hp--;
        spriteRenderer.material.SetColor("_Color", new Color(1f, 1f, 1f, 0f));
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.material.SetColor("_Color", new Color(0f, 0f, 0f, 0f));
        isDamaged = false;

    }
    private IEnumerator DevilDamaged(){
        HpMinus();
        HpMinus();
        hp--;
        hp--;
        spriteRenderer.material.SetColor("_Color", new Color(1f, 1f, 1f, 0f));
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.material.SetColor("_Color", new Color(0f, 0f, 0f, 0f));
        isDamaged = false;
    }
    protected virtual void HpMinus()
    {
        enemyHpBar.value -= 0.084f;
    }

    protected virtual void Dead()
    {
        spriteRenderer.material.SetColor("_Color", new Color(0f, 0f, 0f, 0f));
        col.enabled = false;
        _gameManager.FireCount();
        Despawn();
    }
    private void DevilBulletInstantiateOrSpawn()
    {
        if (_gameManager.PoolManager.devilSkillPool.transform.childCount > 0)
        {
            devilBullet = _gameManager.PoolManager.devilSkillPool.transform.GetChild(0).gameObject;
            devilBullet.SetActive(true);
            devilBullet.transform.position = new Vector2(3.55f, transform.localPosition.y);
        }
        else
        {
            devilBullet = Instantiate(devilBulletPrefab, new Vector2(3.55f, transform.localPosition.y), Quaternion.identity);
        }
        if (devilBullet != null)
        {
            devilBullet.transform.SetParent(null);
        }
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
        if(!_gameManager)Debug.Log("����!");
        if (transform.localPosition.y < _gameManager.MinPosition.y)
        {
            isRush = false;
            spriteRenderer.material.SetColor("_Color", new Color(0f, 0f, 0f, 0f));
            speed = 5f;
            transform.localPosition = new Vector2(transform.localPosition.x, _gameManager.MaxPositon.y + 2f);
        }
    }
    protected virtual void Despawn()
    {
        enemyHpBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 10f, 0));
        RandomItemDrop();
        isDead = false;
        isRush = false;
        skillBox.gameObject.SetActive(false);
        SetVariable();
        enemyHpBar.value = 1f;
        col.enabled = true;
        transform.SetParent(_gameManager.PoolManager.enemyPool.transform, false);
        gameObject.SetActive(false);
        _gameManager.SetEnemyPositionDead(enemyIdx);
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
        if (_gameManager.PoolManager.fastSkillPool.transform.childCount > 0)
        {
            item = _gameManager.PoolManager.fastSkillPool.transform.GetChild(0).gameObject;
            //enemy.layer = LayerMask.NameToLayer("Enemy");
            item.SetActive(true);
            item.transform.rotation = Quaternion.identity;
            item.transform.position = transform.localPosition;
        }
        else
        {
            //item = Instantiate(changeItemPrefab, transform.localPosition, Quaternion.identity);
            item = Instantiate(fastItemPrefab, transform.localPosition, Quaternion.identity);
            //JudgeItem();

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
