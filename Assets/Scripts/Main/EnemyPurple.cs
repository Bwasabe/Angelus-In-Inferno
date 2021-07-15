using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPurple : EnemyMove
{
    [SerializeField]
    private GameObject purpleBulletPrefab = null;
 
    [SerializeField]
    private float buleltDelay = 2f;
    [SerializeField]
    private Sprite purpleBulletSprite = null;
    [SerializeField]
    private Transform bulletPosition = null;


    private Vector2 diff = Vector2.zero;
    private GameObject enemyBullet = null;
    private float rotationZ = -90f;
    private bool isRight = false;
    private bool isLeft = true;

    void Start()
    {
        StartCoroutine(PurpleFire());
        SetVariable();
    }
    private void OnEnable(){
        SetVariable();
        StartCoroutine(PurpleFire());
    }
    private void OnDisable(){
        StopCoroutine(PurpleFire());
    }
    protected override void Update()
    {
        Move();
        SetHpBar();
        JudgeLR();
    }
    protected override void Move(){
        if(isLeft){
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
        else if(isRight){
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        else{
            return;
        }

    }
    private void JudgeLR(){
        if(transform.localPosition.x<GameManager.Instance.MinPosition.x +0.3f){
            isLeft = false;
            isRight = true;
        }
        if(transform.localPosition.x>GameManager.Instance.MaxPositon.x -0.3f){
            isRight = false;
            isLeft = true;
        }
    }
    public override void SetHpBar(){
        if(!enemyHpBar)return;
        enemyHpBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0,-0.9f,0));
    }
    protected override void SetVariable()
    {
        hp = 15;
        score = 1000;
        speed = 0.5f;
        buleltDelay = 2f;
    }
    protected override void HpMinus()
    {
        enemyHpBar.value -= 0.067f;
    }
    private void SpawnOrInstantiate()
    {

        if (GameManager.Instance.PoolManager.enemyBullet.transform.childCount > 0)
        {
            enemyBullet = GameManager.Instance.PoolManager.enemyBullet.transform.GetChild(0).gameObject;
            enemyBullet.SetActive(true);
            enemyBullet.transform.localScale = new Vector2(2, 2);
            enemyBullet.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
            enemyBullet.transform.SetParent(transform, false);
            enemyBullet.transform.position = transform.position;
            JudgeBullet();
        }
        else
        {
            enemyBullet = Instantiate(purpleBulletPrefab, bulletPosition);
            JudgeBullet();
            enemyBullet.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
        }


        if (enemyBullet != null)
        {
            enemyBullet.transform.SetParent(null);
        }
    }
    private void JudgeBullet(){
        enemyBullet.GetComponent<SpriteRenderer>().sprite = purpleBulletSprite;
    }
    private IEnumerator PurpleFire()
    {
        while (true)
        {
            yield return new WaitForSeconds(buleltDelay);
            for (int i = 0; i < 30; i++)
            {
                SpawnOrInstantiate();
                rotationZ += 12f;
            }
        }
    }
    protected override void Despawn()
    {
        RandomItemDrop();
        isDead = false;
        SetVariable();
        enemyHpBar.value = 1f;
        col.enabled = true;
        transform.SetParent(GameManager.Instance.PoolManager.enemyPurplePool.transform, false);
        gameObject.SetActive(false);
    }
    protected override void Dead()
    {
        spriteRenderer.material.SetColor("_Color", new Color(0f, 0f, 0f, 0f));
        col.enabled = false;
        GameManager.Instance.PurpleCount();
        Despawn();
    }
}
