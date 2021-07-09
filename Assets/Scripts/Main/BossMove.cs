using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMove : EnemyMove
{
    [SerializeField]
    private Sprite bossBulletSprite = null;
    [SerializeField]
    private GameObject bossBulletPrefab = null;
    [SerializeField]
    private GameObject bossBigBulletPrefab = null;
    [SerializeField]
    private GameObject bossPurpleFirePrefab = null;
    [SerializeField]
    private GameObject bossSwardPrefab = null;
    [SerializeField]
    private GameObject arrow = null;
    [SerializeField]
    private Canvas bossCanvas = null;
    [SerializeField]
    private GameObject[] laser = null;
    [SerializeField]
    private GameObject bSkillBox = null;

    private GameObject bossBullet = null;
    private GameObject bossBigBullet = null;
    private GameObject bossSward = null;

    private ArrowMove arrowMove = null;
    private Animator animator = null;
    private float bRotationZ = 0f;
    private float pRotationZ = 0f;
    private Vector2 diff = Vector2.zero;
    private bool isBig = false;
    private bool isFire = false;
    private bool isRFire = false;
    private bool isSward = false;
    private bool isCharge = false;
    private bool bIsRush = false;
    private bool isLeft = false;
    private bool isRight = false;
    private float ebspeed = 0f;
    IEnumerator Start()
    {
        if (!arrowMove) arrowMove = FindObjectOfType<ArrowMove>();
        if (!animator) animator = GetComponent<Animator>();
        animator.enabled = false;
        gameManager.StopSpawning();
        speed = 3f;
        StartCoroutine(FillHp());
        yield return new WaitForSeconds(1.4f);
        speed = 0f;
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(RandomFire());
    }


    // private void OnEnable()
    // {
    //     //gameManager.StopSpawning();
    // }
    private void OnDisable()
    {
        gameManager.FalseBoss();
        //gameManager.Startspawning();
    }

    protected override void Update()
    {
        if (bIsRush)
        {
            transform.localPosition = new Vector2(transform.localPosition.x, gameManager.Player.transform.localPosition.y);
        }
        else if (isLeft)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
        else if (isRight)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        else
        {
            Move();
        }
    }
    public override void SetHpBar()
    {
        if (!enemyHpBar) return;
        enemyHpBar.transform.position = Camera.main.WorldToScreenPoint(new Vector3(0, 4.85f, 0));

    }
    private IEnumerator FillHp()
    {
        enemyHpBar.value = 0f;
        for (int i = 1; i <= 100; i++)
        {
            enemyHpBar.value += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
    }
    protected override void HpMinus()
    {
        //enemyHpBar.value -= 0.01f;
        enemyHpBar.value -= 0.0034f;
    }
    protected override void SetVariable()
    {
        hp = 300;
        score = 5000;
        speed = 3f;
    }
    private IEnumerator RandomFire()
    {
        yield return new WaitWhile(() => isRFire);
        yield return new WaitWhile(() => isFire);
        yield return new WaitWhile(() => isSward);
        yield return new WaitWhile(() => isCharge);
        //yield return new WaitForSeconds(1.5f);
        int random = Random.Range(1, 5);
        switch (random)
        {
            case 1:
                isRFire = true;
                yield return new WaitForSeconds(1f);
                StartCoroutine(BossFire());
                break;
            case 2:
                isFire = true;
                yield return new WaitForSeconds(1f);
                StartCoroutine(BossPurpleFire());
                break;
            case 3:
                isSward = true;
                yield return new WaitForSeconds(1f);
                StartCoroutine(BossSward());
                break;
            case 4:
                isCharge = true;
                yield return new WaitForSeconds(1f);
                StartCoroutine(BossRush());
                break;
        }
        StartCoroutine(RandomFire());
    }
    private void SpawnOrInstantiate()
    {

        if (gameManager.PoolManager.enemyBullet.transform.childCount > 0)
        {
            bossBullet = gameManager.PoolManager.enemyBullet.transform.GetChild(0).gameObject;
            JudgeBullet();
            bossBullet.transform.SetParent(transform, false);
            bossBullet.transform.position = transform.position;
            bossBullet.SetActive(true);
            bossBullet.transform.rotation = Quaternion.Euler(0f, 0f, bRotationZ - 180f);
        }
        else
        {
            bossBullet = Instantiate(bossBulletPrefab, new Vector2(transform.localPosition.x, transform.localPosition.y), Quaternion.identity);
            JudgeBullet();
            bossBullet.transform.rotation = Quaternion.Euler(0f, 0f, bRotationZ - 180f);
        }

        bossBullet.transform.localScale = new Vector2(2, 2);

        if (bossBullet != null)
        {
            bossBullet.transform.SetParent(null);
        }
    }
    private void FastSpawnOrInstantiate()
    {

        if (gameManager.PoolManager.bossBigBullet.transform.childCount > 0)
        {
            bossBigBullet = gameManager.PoolManager.bossBigBullet.transform.GetChild(0).gameObject;
            bossBigBullet.transform.localScale = new Vector2(4, 4);
            //JudgeBullet();
            bossBigBullet.transform.SetParent(transform, false);
            bossBigBullet.transform.position = transform.position;
            bossBigBullet.SetActive(true);
            bossBigBullet.transform.rotation = Quaternion.Euler(0f, 0f, bRotationZ - 90f);
        }
        else
        {
            bossBigBullet = Instantiate(bossBigBulletPrefab, new Vector2(transform.localPosition.x, transform.localPosition.y), Quaternion.identity);
            //JudgeBullet();
            bossBigBullet.transform.rotation = Quaternion.Euler(0f, 0f, bRotationZ - 90f);
        }


        if (bossBigBullet != null)
        {
            bossBigBullet.transform.SetParent(null);
        }
    }
    private void PurpleSpawnOrInstantiate()
    {


        if (gameManager.PoolManager.bossPurplePool.transform.childCount > 0)
        {
            bossBigBullet = gameManager.PoolManager.bossPurplePool.transform.GetChild(0).gameObject;
            //bossBigBullet.transform.localScale = new Vector2(4, 4);
            //JudgeBullet();
            bossBigBullet.transform.SetParent(transform, false);
            bossBigBullet.transform.position = transform.position;
            bossBigBullet.SetActive(true);
            bossBigBullet.transform.rotation = Quaternion.Euler(0f, 0f, pRotationZ - 180f);
        }
        else
        {
            bossBigBullet = Instantiate(bossPurpleFirePrefab, new Vector2(transform.localPosition.x, transform.localPosition.y), Quaternion.identity);
            //JudgeBullet();
            bossBigBullet.transform.rotation = Quaternion.Euler(0f, 0f, pRotationZ - 180f);
        }


        if (bossBigBullet != null)
        {
            bossBigBullet.transform.SetParent(null);
        }
    }
    private void SwardSpawnOrInstantiate(float swardX)
    {


        if (gameManager.PoolManager.bossSwardPool.transform.childCount > 0)
        {
            bossSward = gameManager.PoolManager.bossSwardPool.transform.GetChild(0).gameObject;
            bossSward.transform.SetParent(transform, false);
            bossSward.transform.position = new Vector2(swardX, 6f);
            bossSward.SetActive(true);
            //bossSward.transform.rotation = Quaternion.Euler(0f, 0f, pRotationZ);
        }
        else
        {
            bossSward = Instantiate(bossSwardPrefab, new Vector2(swardX, 6f), Quaternion.identity);
            //bossSward.transform.rotation = Quaternion.Euler(0f, 0f, pRotationZ);
        }


        if (bossSward != null)
        {
            bossSward.transform.SetParent(null);
        }
    }
    private void JudgeBullet()
    {
        bossBullet.GetComponent<SpriteRenderer>().sprite = bossBulletSprite;
    }
    private IEnumerator BossFire()
    {
        isRFire = true;
        bRotationZ = 45f;
        for (int i = 0; i < 10; i++)
        {
            SpawnOrInstantiate();
            yield return new WaitForSeconds(0.1f);
            bRotationZ -= 9f;
        }
        bRotationZ += 3f;
        for (int i = 0; i < 10; i++)
        {
            SpawnOrInstantiate();
            yield return new WaitForSeconds(0.1f);
            bRotationZ += 9f;
        }
        for (int i = 0; i < 30; i++)
        {
            bRotationZ = Random.Range(-25f, 25f);
            SpawnOrInstantiate();
            yield return new WaitForSeconds(0.1f);
        }
        arrow.gameObject.SetActive(true);
        isBig = true;
        yield return new WaitForSeconds(0.8f);
        for (int i = 0; i < 3; i++)
        {
            arrowMove.stopRotation = true;
            BigFire();
            arrow.GetComponent<SpriteRenderer>().material.SetColor("_Color", new Color(1f, 0f, 0f, 1f));
            yield return new WaitForSeconds(0.4f);
            arrow.GetComponent<SpriteRenderer>().material.SetColor("_Color", new Color(1f, 1f, 1f, 1f));
            FastSpawnOrInstantiate();
            arrowMove.stopRotation = false;
            yield return new WaitForSeconds(0.6f);
        }
        isBig = false;
        isRFire = false;
        arrow.gameObject.SetActive(false);
    }
    private void BigFire()
    {
        diff = new Vector2((gameManager.Player.transform.position - transform.position).x, (gameManager.Player.transform.position - transform.position).y);
        bRotationZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
    }
    private IEnumerator BossPurpleFire()
    {
        isFire = true;
        for (int i = 0; i < 2; i++)
        {
            yield return new WaitForSeconds(1f);
            pRotationZ = Random.Range(-25f, 25f);
            PurpleSpawnOrInstantiate();
            yield return new WaitForSeconds(1f);
            pRotationZ *= -1;
            PurpleSpawnOrInstantiate();
        }
        yield return new WaitForSeconds(0.5f);
        isFire = false;
    }
    private IEnumerator BossSward()
    {
        int[] arr = new int[3];
        animator.enabled = true;
        animator.Play("BossMove");
        yield return new WaitForSeconds(0.7f);
        int ran = Random.Range(1, 3);
        if (ran == 1)
        {
            animator.Play("LBossSward");
            arr = new int[3] { -1, 1, -1 };
        }
        if (ran == 2)
        {
            animator.Play("RBossSward");
            arr = new int[3] { 1, -1, 1 };
        }
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 3; i++)
        {
            SwardSpawnOrInstantiate(arr[i]);
            yield return new WaitForSeconds(0.5f);
        }
        isSward = false;
        animator.enabled = false;
    }
    private IEnumerator BossRush()
    {
        speed = -3f;
        yield return new WaitForSeconds(1.7f);
        speed = 0f;
        bSkillBox.SetActive(true);
        transform.localPosition = new Vector2(transform.localPosition.x + 3f, transform.localPosition.y);
        bIsRush = true;
        yield return new WaitForSeconds(1f);
        bIsRush = false;
        StartCoroutine(bossSkillBox.Warning());
        yield return new WaitForSeconds(0.4f);
        isLeft = true;
        speed = 30f;
        yield return new WaitForSeconds(0.2f);
        isLeft = false;
        speed = 0f;
        bIsRush = true;
        yield return new WaitForSeconds(0.8f);
        bIsRush = false;
        StartCoroutine(bossSkillBox.Warning());
        yield return new WaitForSeconds(0.4f);
        isRight = true;
        speed = 30f;
        yield return new WaitForSeconds(0.2f);
        isRight = false;
        speed = 3f;
        transform.localPosition = new Vector2(0f, 6.71f);
        yield return new WaitForSeconds(1.4f);
        speed = 0f;
        isCharge = false;
    }
    protected override void Despawn()
    {
        gameManager.FalseBoss();
        transform.SetParent(gameManager.PoolManager.bossPool.transform, false);
        gameObject.SetActive(false);
    }
    protected override void Dead()
    {
        spriteRenderer.material.SetColor("_Color", new Color(0f, 0f, 0f, 0f));
        col.enabled = false;
        gameManager.BossCount();
        Despawn();
    }
}
