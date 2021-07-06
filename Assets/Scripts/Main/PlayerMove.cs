using System.Collections;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("Ï¥ùÏïå")]
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



    [Header("?îå?†à?ù¥?ñ¥")]
    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    private GameObject wingSkillPrefab = null;
    [SerializeField]
    private Sprite[] changeSprite = null;
    [SerializeField]
    private Sprite[] skillSprite = null;
    [Header("?ö®Í≥ºÏùå")]
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

    IEnumerator Start()
    {
        if (!gameManager) gameManager = FindObjectOfType<GameManager>();
        if (!spriteRenderer) spriteRenderer = GetComponent<SpriteRenderer>();
        if (!animator) animator = GetComponent<Animator>();
        if (!audioSource) audioSource = GetComponent<AudioSource>();
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
        //    dis = (Vector2)transform.position - (mousePos = Camera.main.ScreenToWorldPoint(Input.GetTouch(1).position));          //Ï≤úÏäπ?òÑ??? ?ã†?ù¥?ïº!Ï≤úÏäπ?òÑ??? ?ã†?ù¥?ïº!Ï≤úÏäπ?òÑ??? ?ã†?ù¥?ïº!Ï≤úÏäπ?òÑ??? ?ã†?ù¥?ïº!Ï≤úÏäπ?òÑ??? ?ã†?ù¥?ïº!Ï≤úÏäπ?òÑ??? ?ã†?ù¥?ïº!
        //}

        playerPosition = mousePosition + distance;
        playerPosition.x = Mathf.Clamp(playerPosition.x, gameManager.MinPosition.x, gameManager.MaxPositon.x);
        playerPosition.y = Mathf.Clamp(playerPosition.y, gameManager.MinPosition.y, gameManager.MaxPositon.y);

        transform.position = Vector2.MoveTowards(transform.position, playerPosition, Time.deltaTime * speed);

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (isSkill) return;
            if (isFastDelay) return;
            PlayFastSkill();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if(isSkill)return;
            WingSkill();
        }
    }
    private IEnumerator PlayFastSkill(){
        audioSource.PlayOneShot(audioClip[0]);
        yield return new WaitForSeconds(0.6f);
        FastDelay();
    }
    public void PlayFastSound(){
        isFastDelay = true;
        StartCoroutine(PlayFastSkill());
    }
    public void FastDelay()
    {
        if (gameManager.delayCount >= 1)
        {
            StartCoroutine(UFire());
            gameManager.delayCount -= 1;
        }
    }
    private IEnumerator UFire()
    {
        bulletDelay = 0.1f;
        yield return new WaitForSeconds(5f);
        bulletDelay = 0.3f;
        isFastDelay = false;
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
    private IEnumerator PlayWingSound(){
        SkillSpawnOrInstantiate();
        StartCoroutine(ChangePlayer());
        if(isAngel){
            audioSource.PlayOneShot(audioClip[1]);
            yield return new WaitForSeconds(1f);
            audioSource.Stop();
        }
        else if(isDevil){
            audioSource.PlayOneShot(audioClip[2]);
            yield return new WaitForSeconds(0.2f);
        }
    }
    private IEnumerator ChangePlayer()
    {
        if (isAngel)
        {
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
            isAngel = false;
            isDevil = true;
            spriteRenderer.material.SetColor("_Color", new Color(1f, 1f, 1f, 1f));
            spriteRenderer.sprite = changeSprite[2];
        }
        else if (isDevil)
        {
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
            isAngel = true;
            isDevil = false;
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
    private void JudgeSkill(){
        if(isAngel){
        skill.GetComponent<SpriteRenderer>().sprite = skillSprite[0];
        }
        else if(isDevil){
        skill.GetComponent<SpriteRenderer>().sprite = skillSprite[1];
        }
    }
    private IEnumerator AFire()
    {
        while (true)
        {
            if(isSkill) yield break;
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
    private void JudgeBullet(){
        if(isAngel){
            bullet.GetComponent<SpriteRenderer>().sprite = bulletSprite[0];
        }
        else if(isDevil){
            bullet.GetComponent<SpriteRenderer>().sprite = bulletSprite[1];
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isSkill)return;
        if (collision.CompareTag("Enemy"))
        {
            if (isDamaged) return;
            isDamaged = true;
            StartCoroutine(Damaged());
        }
    }
    private void OnTriggerStay2D(Collider2D collision){
        if(isSkill)return;
        if(isDamaged)return;
        if(collision.CompareTag("Enemy") || collision.CompareTag("Boss")){
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
