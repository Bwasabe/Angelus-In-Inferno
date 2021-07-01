using UnityEngine;

public class EnemyBulletMove : BulletMove
{
    protected override void Limit()
    {
        if(transform.localPosition.y > gameManager.MaxPositon.y+0.5f){
            Despawn();
        }
        if(transform.localPosition.y <gameManager.MinPosition.y-0.5f){
            Despawn();
        }
        if(transform.localPosition.x<gameManager.MinPosition.x-0.5f){
            Despawn();
        }
        if(transform.localPosition.x>gameManager.MaxPositon.x+0.5f){
            Despawn();
        }
    }
    // private void OnDisable(){
    //     transform.localScale = new Vector2(2,2);
    // }
    public override void Despawn()
    {
        transform.SetParent(gameManager.PoolManager.enemyBullet.transform,false);
        gameObject.SetActive(false);
    }
    protected override void SetVariable()
    {
        speed = 5f;
    }
 
}
