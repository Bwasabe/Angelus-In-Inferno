using System.Collections;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("ΓΡΎΛ")]
    [SerializeField]
    private GameObject bulletPrefab = null;
    [SerializeField]
    private Sprite[] bulletSprite = null;
    [SerializeField]
    private Transform bulletPosition = null;
    [SerializeField]
    private float bulletDelay = 0.3f;
    [SerializeField]
    private Vector2 bulletScale = Vector2.one;



    [Header("ΗΓ·ΉΐΜΎξ")]
    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    private GameObject wingSkillPrefab = null;
    [SerializeField]
    private Sprite[] changeSprite = null;
    [SerializeField]
    private Sprite[] skillSprite = null;
    [SerializeField]
    private GameObject angelRing = null;
    [SerializeField]
    private Sprite[] angelRingSprite = null;

    [Header("»ηΏξ΅ε")]
    [SerializeField]
    private AudioClip[] audioClip = null;

    private Vector2 playerPosition = Vector2.zero;

    private GameManager gameManager = null;
    private AudioSource audioSource = null;
    private SpriteRenderer spriteRenderer = null;
    private Animator animator = null;
    private GameObject bullet = null;
    private GameObject skill = null;
    private Vector2 mousePosition = new Vector2(0, -3f);
    private Vector2 distance = Vector2.zero;
    private bool isDamaged = false;
    public bool isFastDelay = false;
    public bool isSkill = false;
    public bool isAngel = true;
    public bool isDevil = false;
    public bool isDSkill = false;

    IEnumerator Start()
    {
        if (!gameManager) gameManager = FindObjectOfType<GameManager>();
        if (!spriteRenderer) spriteRenderer = GetComponent<SpriteRenderer>();
        if (!animator) animator = GetComponent<Animator>();
        if (!audioSource) audioSource = GetComponent<AudioSource>();
        animator.enabled = false;
        angelRing.SetActive(false);
        speed = 5f;
        yield return new WaitForSeconds(0.5f);
        speed = 10f;
        StartCoroutine(AFire());
    }

    void Update()
    {
        OnClick();
    }
    private void OnClick()
    {
        if (Input.GetMouseButton(0))
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition/*Input.GetTouch(0).position*/);

        if (/*Input.GetTouch(0).phase == TouchPhase.Began*/ Input.GetMouseButtonDown(0))
        {
            distance = (Vector2)transform.position - mousePosition; //= Camera.main.ScreenToWorldPoint(Input.mousePosition/*Input.GetTouch(0).position*/));
        }

        //if(Input.GetTouch(0).phase == TouchPhase.Ended)
        //{
        //    dis = (Vector2)transform.position - (mousePos = Camera.main.ScreenToWorldPoint(Input.GetTouch(1).position));          //μ²μΉ???? ? ?΄?Ό!μ²μΉ???? ? ?΄?Ό!μ²μΉ???? ? ?΄?Ό!μ²μΉ???? ? ?΄?Ό!μ²μΉ???? ? ?΄?Ό!μ²μΉ???? ? ?΄?Ό!
        //}

        playerPosition = mousePosition + distance;
        playerPosition.x = Mathf.Clamp(playerPosition.x, gameManager.MinPosition.x, gameManager.MaxPositon.x);
        playerPosition.y = Mathf.Clamp(playerPosition.y, gameManager.MinPosition.y, gameManager.MaxPositon.y);

        transform.position = Vector2.MoveTowards(transform.position, playerPosition, Time.deltaTime * speed);

        // if (Input.GetKeyDown(KeyCode.W))
        // {
        //     if (isSkill) return;
        //     if (isAngel)
        //     {
        //         if (isFastDelay) return;
        //         PlayFastSkill();
        //     }
        //     else if(isDevil){
        //         if(isDSkill)return;
        //         PlayDSkill();
        //     }
        // }
        // if (Input.GetKeyDown(KeyCode.Q))
        // {
        //     if(isDSkill)return;
        //     if (isSkill) return;
        //     WingSkill();
        // }
    }
    private IEnumerator PlayFastAnimation()
    {
        if(isAngel){
            angelRing.GetComponent<SpriteRenderer>().sprite = angelRingSprite[0];
        }
        else if(isDevil){
            angelRing.GetComponent<SpriteRenderer>().sprite = angelRingSprite[1];
        }
        angelRing.SetActive(true);
        animator.enabled = true;
        animator.Play("Player.idle");
        yield return new WaitForSeconds(0.6f);
    }
    private IEnumerator PlayFastSkill()
    {
        audioSource.PlayOneShot(audioClip[0]);
        yield return new WaitForSeconds(0.6f);
        FastDelay();
    }
    public void PlayDSkill(){
        isDSkill = true;
        StartCoroutine(PlayDSkillSound());
        StartCoroutine(PlayFastAnimation());
    }
    private IEnumerator PlayDSkillSound(){
        audioSource.PlayOneShot(audioClip[2]);
        yield return new WaitForSeconds(0.2f);
        FastDelay();
    }
    public void PlayFastSound()
    {
        isFastDelay = true;
        StartCoroutine(PlayFastSkill());
        StartCoroutine(PlayFastAnimation());
    }
    private void FastDelay()
    {
        if (gameManager.delayCount >= 1)
        {
            if(isAngel){
                StartCoroutine(UFire());
            }
            else if(isDevil){
                StartCoroutine(DFire());
            }
            gameManager.delayCount -= 1;
        }
    }
    private IEnumerator DFire(){
        bulletDelay = 0.5f;
        yield return new WaitForSeconds(5f);
        bulletDelay = 0.3f;
        isDSkill = false;
        yield return new WaitForSeconds(0.6f);
        animator.StopPlayback();
        animator.enabled = false;
        angelRing.SetActive(false);
        }
    private IEnumerator UFire()
    {
        bulletDelay = 0.1f;
        yield return new WaitForSeconds(5f);
        //angelRing.SetActive(false);
        bulletDelay = 0.3f;
        isFastDelay = false;
        yield return new WaitForSeconds(0.6f);
        animator.StopPlayback();
        animator.enabled = false;
        angelRing.SetActive(false);
    }
    public void WingSkill()
    {
        if (gameManager.changeCount >= 1)
        {
            gameManager.changeCount -= 1;
            isSkill = true;
            
            StartCoroutine(PlayWingSound());
        }
    }
    private IEnumerator PlayWingSound()
    {
        SkillSpawnOrInstantiate();
        if (isAngel)
        {
            audioSource.PlayOneShot(audioClip[1]);
            yield return new WaitForSeconds(1f);
            audioSource.Stop();
        }
        else if (isDevil)
        {
            audioSource.PlayOneShot(audioClip[2]);
            yield return new WaitForSeconds(0.2f);
        }
        StartCoroutine(ChangePlayer());
    }
    private IEnumerator ChangePlayer()
    {
        if (isAngel)
        {
            isAngel= false;
            isDevil = true;
            spriteRenderer.sprite = changeSprite[1];
            for (int i = 2; i <= 4; i++)
            {
                transform.localScale = new Vector2(i, i);
                if (i == 4)
                {
                    spriteRenderer.material.SetColor("_Color", new Color(1f, 0f, 0f, 1f));
                }
                yield return new WaitForSeconds(1.3f);
            }
            transform.localScale = new Vector2(2, 2);
            spriteRenderer.material.SetColor("_Color", new Color(1f, 1f, 1f, 1f));
            spriteRenderer.sprite = changeSprite[2];
        }
        else if (isDevil)
        {
            isAngel = true;
            isDevil = false;
            spriteRenderer.sprite = changeSprite[3];
            for (int i = 4; i >= 2; i--)
            {
                transform.localScale = new Vector2(i, i);
                if (i == 2)
                {
                    spriteRenderer.material.SetColor("_Color", new Color(0f, 0.8f, 1f, 1f));
                }
                yield return new WaitForSeconds(1.3f);
            }
            transform.localScale = new Vector2(2, 2);
            spriteRenderer.material.SetColor("_Color", new Color(1f, 1f, 1f, 1f));
            spriteRenderer.sprite = changeSprite[0];
        }
        isSkill = false;
        StartCoroutine(AFire());
    }
    private void SkillSpawnOrInstantiate()
    {
        if (gameManager.PoolManager.skillPool.transform.childCount > 0)
        {
            skill = gameManager.PoolManager.skillPool.transform.GetChild(0).gameObject;
            JudgeSkill();
            skill.SetActive(true);
            skill.transform.rotation = Quaternion.identity;
            skill.transform.SetParent(bulletPosition, false);
            skill.transform.position = bulletPosition.position;
        }
        else
        {
            skill = Instantiate(wingSkillPrefab, bulletPosition);
            JudgeSkill();
        }
        if (skill != null)
        {
            skill.transform.SetParent(null);
        }
    }
    private void JudgeSkill()
    {
        if (isAngel)
        {
            skill.GetComponent<SpriteRenderer>().sprite = skillSprite[0];
        }
        else if (isDevil)
        {
            skill.GetComponent<SpriteRenderer>().sprite = skillSprite[1];
        }
    }
    private IEnumerator AFire()
    {
        while (true)
        {
            if (isSkill) yield break;
            SpawnOrInstantiate();
            yield return new WaitForSeconds(bulletDelay);
        }
    }
    private void SpawnOrInstantiate()
    {
        if (gameManager.PoolManager.bulletPool.transform.childCount > 0)
        {
            bullet = gameManager.PoolManager.bulletPool.transform.GetChild(0).gameObject;
            bullet.layer = LayerMask.NameToLayer("Player");
            JudgeBullet();
            bullet.SetActive(true);
            bullet.transform.rotation = Quaternion.identity;
            bullet.transform.SetParent(bulletPosition, false);
            bullet.transform.position = bulletPosition.position;
            bullet.transform.localScale = Vector2.one;
        }
        else
        {
            bullet = Instantiate(bulletPrefab, bulletPosition);
            JudgeBullet();
        }
        if (bullet != null)
        {
            bullet.transform.SetParent(null);
        }
    }
    private void JudgeBullet()
    {
        if (isAngel)
        {
            bullet.GetComponent<SpriteRenderer>().sprite = bulletSprite[0];
        }
        else if (isDevil)
        {
            bullet.GetComponent<SpriteRenderer>().sprite = bulletSprite[1];
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isSkill) return;
        if (collision.CompareTag("Enemy"))
        {
            if (isDamaged) return;
            isDamaged = true;
            StartCoroutine(Damaged());
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isSkill) return;
        if (isDamaged) return;
        if (collision.CompareTag("Enemy") || collision.CompareTag("Boss"))
        {
            isDamaged = true;
            StartCoroutine(Damaged());
        }
    }
    private IEnumerator Damaged()
    {
        gameManager.Dead();
        for (int i = 0; i < 5; i++)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
        isDamaged = false;
    }
}
