using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMove : MonoBehaviour
{
    [SerializeField]
    private Sprite[] sprite;
    private GameManager gameManager = null;
    private EnemyMove enemyMove = null;
    private CircleCollider2D circol = null;
    private SpriteRenderer spriteRenderer = null;
    private int index = 0;
    

    void Awake(){
        if(!gameManager)gameManager = FindObjectOfType<GameManager>();
        if(!enemyMove)enemyMove = FindObjectOfType<EnemyMove>();
        if(!circol)circol = FindObjectOfType<CircleCollider2D>();
        if(!spriteRenderer)spriteRenderer = FindObjectOfType<SpriteRenderer>();
    }
    void Update(){
        Limit();
    }
    void OnEnable(){
        JudgeItem();
    }
    private void Limit(){
        if(transform.localPosition.y < gameManager.MinPosition.y - 1f){
            Despawn();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision){

        if(collision.CompareTag("Player")){
            JudgeIndex();
            Despawn();
        }
    }
    public void JudgeItem(){
        index = Random.Range(0,4);
        if(index >= 1) index =1;
        spriteRenderer.sprite = sprite[index];

            //  if(enemyMove.isFast == true){
            //     circol.radius = 1f;
            //     gameManager.changeCount += 1;
            // }
            // if(index == 2){
            //     circol.radius = 0.16f;
            //     gameManager.delayCount += 1;
            // }
    }
    private void JudgeIndex(){
        switch(index){
            case 0:{
                gameManager.changeCount += 1; 
                break;
            }
            case 1: {
                gameManager.delayCount += 1; 
                break;
            }
        }
    }
    public void ItemIndex(int num){
        index = num;
    }
    private void Despawn(){
        gameObject.SetActive(false);
        transform.SetParent(gameManager.PoolManager.fastSkillPool.transform , false);
    }
}