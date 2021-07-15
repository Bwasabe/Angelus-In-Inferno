using UnityEngine;

public class EnemyBulletMove : BulletMove
{
    protected override void Limit()
    {
        if(transform.localPosition.y > GameManager.Instance.MaxPositon.y+0.5f){
            Despawn();
        }
        if(transform.localPosition.y <GameManager.Instance.MinPosition.y-0.5f){
            Despawn();
        }
        if(transform.localPosition.x<GameManager.Instance.MinPosition.x-0.5f){
            Despawn();
        }
        if(transform.localPosition.x>GameManager.Instance.MaxPositon.x+0.5f){
            Despawn();
        }
    }
    public override void Despawn()
    {
        transform.SetParent(GameManager.Instance.PoolManager.enemyBullet.transform,false);
        gameObject.SetActive(false);
    }
    protected override void SetVariable()
    {
        speed = 5f;
    }
 
}
