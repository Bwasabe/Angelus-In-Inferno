using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPurple : EnemyMove
{
    [SerializeField]
    private GameObject purplePrefab = null;
    [SerializeField]
    private Sprite enemyBulletSprite = null;
    [SerializeField]
    private float buleltDelay = 3f;


    private Vector2 diff = Vector2.zero;
    private GameObject enemyBullet=null;
    //private float timer = 0f;
    private float rotationZ = -90f;
    
    void Start(){
        if(!gameManager)gameManager = FindObjectOfType<GameManager>();
    }
    protected override void Move(){
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        // timer += Time.deltaTime;

        // if (timer >= buleltDelay)
        // {
            

        //     timer = 0f;
            
        //     diff = gameManager.Player.transform.position - transform.position;
        //     diff.Normalize();
        //     rotationZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        //     SpawnOrInstantiate();


        // }
    }
    private void SpawnOrInstantiate()
    {

        if (gameManager.PoolManager.transform.childCount > 0)
        {
            enemyBullet = gameManager.PoolManager.bulletPool.transform.GetChild(0).gameObject;
            enemyBullet.transform.localScale = new Vector2(2, 2);
            enemyBullet.transform.SetParent(transform, false);
            enemyBullet.transform.position = transform.position;
            enemyBullet.SetActive(true);
        }
        else{
            enemyBullet = Instantiate(purplePrefab, transform);
        }


        if (enemyBullet != null)
        {
            enemyBullet.layer = LayerMask.NameToLayer("Enemy");
            enemyBullet.GetComponent<SpriteRenderer>().sprite = enemyBulletSprite;
            //enemyBullet.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ - 90f);
            enemyBullet.transform.SetParent(null);
        }
    }
    private IEnumerator PurpleFire(){
        yield return new WaitForSeconds(3f);
        for(int i=0 ; i<15; i++){
            enemyBullet.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
            SpawnOrInstantiate();
            rotationZ+=24f;
        }
    }
}
