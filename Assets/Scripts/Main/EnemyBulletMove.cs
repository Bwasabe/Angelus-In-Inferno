using UnityEngine;

public class EnemyBulletMove : BulletMove
{
    protected override void Limit()
    {
        if(transform.localPosition.y > _gameManager.MaxPositon.y+0.5f){
            Despawn();
        }
        if(transform.localPosition.y <_gameManager.MinPosition.y-0.5f){
            Despawn();
        }
        if(transform.localPosition.x<_gameManager.MinPosition.x-0.5f){
            Despawn();
        }
        if(transform.localPosition.x>_gameManager.MaxPositon.x+0.5f){
            Despawn();
        }
    }
    public override void Despawn()
    {
        transform.SetParent(_gameManager.PoolManager.enemyBullet.transform,false);
        gameObject.SetActive(false);
    }
    protected override void SetVariable()
    {
        speed = 5f;
    }
 
}
