using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPurple : EnemyMove
{
    [SerializeField]
    private GameObject purpleBulletPrefab = null;
 
    [SerializeField]
    private float buleltDelay = 3f;


    private Vector2 diff = Vector2.zero;
    private GameObject enemyBullet = null;
    private float rotationZ = -90f;
    private bool isRight = false;
    private bool isLeft = true;

    void Start()
    {
        if (!gameManager) gameManager = FindObjectOfType<GameManager>();
        StartCoroutine(PurpleFire());
        SetVariable();
    }
    private void Update()
    {
        Move();
        SetHpBar();
        JudgeLR();
    }
    private void Move(){
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
        if(transform.localPosition.x<gameManager.MinPosition.x +0.3f){
            isLeft = false;
            isRight = true;
        }
        if(transform.localPosition.x>gameManager.MaxPositon.x -0.3f){
            isRight = false;
            isLeft = true;
        }
    }
    protected override void SetHpBar(){
        enemyHpBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0,-0.9f,0));
    }
    protected override void SetVariable()
    {
        hp = 15;
        score = 1000;
        speed = 0.5f;
    }
    protected override void HpMinus()
    {
        enemyHpBar.value -= 0.067f;
    }
    private void SpawnOrInstantiate()
    {

        if (gameManager.PoolManager.enemyBullet.transform.childCount > 0)
        {
            enemyBullet = gameManager.PoolManager.enemyBullet.transform.GetChild(0).gameObject;
            enemyBullet.transform.localScale = new Vector2(2, 2);
            enemyBullet.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
            enemyBullet.transform.SetParent(transform, false);
            enemyBullet.transform.position = transform.position;
            enemyBullet.SetActive(true);
        }
        else
        {
            enemyBullet = Instantiate(purpleBulletPrefab, new Vector2(transform.localPosition.x, transform.localPosition.y) ,Quaternion.identity);
            enemyBullet.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
        }


        if (enemyBullet != null)
        {
            //enemyBullet.layer = LayerMask.NameToLayer("Enemy");
            //enemyBullet.GetComponent<SpriteRenderer>().sprite = enemyBulletSprite;
            //enemyBullet.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ - 90f);
            enemyBullet.transform.SetParent(null);
        }
    }
    private IEnumerator PurpleFire()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            for (int i = 0; i < 30; i++)
            {
                SpawnOrInstantiate();
                rotationZ += 12f;
            }
        }
    }
    // protected override void Despawn(){

    //     isRush = false;
    //     isDamaged = false;
    //     skillBox.gameObject.SetActive(false);
    //     SetVariable();
    //     enemyHpBar.value = 1f;
    //     col.enabled = true;
    //     transform.SetParent(gameManager.PoolManager.enemyPool.transform, false);
    //     gameObject.SetActive(false);
    //     gameManager.SetEnemyPositionDead(enemyIdx);
    // }
}
