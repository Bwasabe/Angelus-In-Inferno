using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPurpleMove : BulletMove
{
    [SerializeField]
    private GameObject purpleBulletPrefab = null;
    [SerializeField]
    private Sprite purpleSprite = null;
    [SerializeField]
    private Transform bulletPosition = null;
    private GameObject purpleBullet = null;
    private float rotationZ = 24f;
    private float maxX = 1.7f;
    private float minX = -1.7f;
    private float randomY = 0f;



    private void OnEnable(){
        randomY = Random.Range(-2f , 1f);
        rotationZ = 24f;
    }
    protected override void SetVariable(){
        speed = 5f;
    }
    protected override void Limit(){
       if(transform.localPosition.x > maxX){
            Despawn();
        }
        if(transform.localPosition.x < minX){
            Despawn();
        }
        if(transform.localPosition.y < randomY){
            Despawn();
        }
    }
    public override void Despawn(){
        transform.rotation = Quaternion.identity;
 
        for (int i = 0; i < 15; i++)
        {
            SpawnOrInstantiate();
            rotationZ += 24f;
        }
        rotationZ = 24f;
        transform.SetParent(gameManager.PoolManager.bossPurplePool.transform,false);
        gameObject.SetActive(false);
    }
    private void SpawnOrInstantiate()
    {

        if (gameManager.PoolManager.enemyBullet.transform.childCount > 0)
        {
            purpleBullet = gameManager.PoolManager.enemyBullet.transform.GetChild(0).gameObject;
            purpleBullet.SetActive(true);
            //purpleBullet.transform.localScale = new Vector2(2,2);
            purpleBullet.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
            //purpleBullet.transform.SetParent(transform, false);
            purpleBullet.transform.position = transform.position;
            JudgeBullet();
        }
        else
        {
            purpleBullet = Instantiate(purpleBulletPrefab, bulletPosition);
            JudgeBullet();
            purpleBullet.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);

        }


        if (purpleBullet != null)
        {
            purpleBullet.transform.SetParent(null);
        }
    }
    private void JudgeBullet(){
        purpleBullet.GetComponent<SpriteRenderer>().sprite = purpleSprite;
    }

}
