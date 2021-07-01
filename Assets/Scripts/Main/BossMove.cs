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
    private GameObject arrow = null;
    [SerializeField]
    private Canvas bossCanvas = null;

    private GameObject bossBullet = null;
    private GameObject bossBigBullet = null;
    private ArrowMove arrowMove = null;
    private float bRotationZ = 0f;
    private float pRotationZ = 0f;
    private Vector2 diff = Vector2.zero;
    private bool isBig = false;
    private bool isFire = false;
    private bool isRFire = false;
    private float ebspeed = 0f;
    IEnumerator Start()
    {
        if (!arrowMove) arrowMove = FindObjectOfType<ArrowMove>();
        gameManager.StopSpawning();
        speed = 3f;
        StartCoroutine(FillHp());
        yield return new WaitForSeconds(1.3f);
        speed = 0f;
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(RandomFire());
    }

    private void OnEnable()
    {
        //gameManager.StopSpawning();
    }
    private void OnDisable()
    {
        gameManager.FalseBoss();
        //gameManager.Startspawning();
    }

    void Update()
    {
        Move();
    }
    private void Move()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
    }
    public override void SetHpBar()
    {
        if (!enemyHpBar) return;
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
    }
    private IEnumerator RandomFire()
    {
        while (true)
        {
            yield return new WaitWhile(() => isRFire);
            yield return new WaitWhile(() => isFire);
            //yield return new WaitForSeconds(1.5f);
            int random = Random.Range(1, 3);
            switch (random)
            {

                case 1:
                    yield return new WaitForSeconds(1f);
                    StartCoroutine(BossFire());
                    break;
                case 2:
                    yield return new WaitForSeconds(1f);
                    StartCoroutine(BossPurpleFire());
                    break;
            }
        }
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
        bRotationZ +=3f;
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
    // private IEnumerator BossRush(){

    // }
    protected override void Despawn()
    {
        gameManager.FalseBoss();
        transform.SetParent(gameManager.PoolManager.bossPool.transform, false);
        gameObject.SetActive(false);
    }
    
}
