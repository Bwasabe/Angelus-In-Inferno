using System.Collections;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField]
    private long score = 0;
    [SerializeField]
    private int hp = 0;
    [SerializeField]
    private float speed = 7f;

    private GameManager gameManager = null;
    private Collider2D col = null;
    protected SpriteRenderer spriteRenderer = null;
    private bool isRush = false;
    private bool isDead = false;
    private bool isDamaged = false;

    void Start()
    {
        SetVariable();
        if (!gameManager) gameManager = FindObjectOfType<GameManager>();
        if (!spriteRenderer) spriteRenderer = GetComponent<SpriteRenderer>();
        if (!col) col = GetComponent<Collider2D>();
    }
    private void SetVariable()
    {
        hp = 5;
        score = 100;
        speed = 3f;
    }

    void Update()
    {
        if (isDead) return;
        Limit();
        if (transform.localPosition.y <= 3f&&!isRush)
        {
            Debug.Log("우와앙 멈춰!");
            isRush = true;
            StartCoroutine(Rush());
        }
        
        
        transform.Translate(Vector2.down * speed * Time.deltaTime);
        
    }
    private void Limit()
    {
        if (transform.localPosition.y < gameManager.MinPosition.y - 1f)
        {
            Destroy(gameObject);
        }
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

        hp--;
        spriteRenderer.material.SetColor("_Color", new Color(1f, 0f, 0f, 0.5f));
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.material.SetColor("_Color", new Color(0f, 0f, 0f, 0f));
        isDamaged = false;
    }
    private IEnumerator Dead()
    {
        spriteRenderer.material.SetColor("_Color", new Color(0f, 0f, 0f, 0f));
        col.enabled = false;
        yield return new WaitForSeconds(0f);     //애니메이션 추가해야함
        Destroy(gameObject);
        isDead = false;
    }
    private IEnumerator Rush()
    {
        speed=0f;
        for (int i = 0; i < 8; i++)
        {
            spriteRenderer.material.SetColor("_Color", new Color(1f, 0f, 0f, 0.7f));        
            yield return new WaitForSeconds(0.05f);
            spriteRenderer.material.SetColor("_Color", new Color(0f, 0f, 0f, 0f));
            yield return new WaitForSeconds(0.05f);
        }
        spriteRenderer.material.SetColor("_Color", new Color(1f, 0f, 0f, 1f));        
        yield return new WaitForSeconds(0.3f);
        speed = 30f;
        
        
           
        

    }


}
