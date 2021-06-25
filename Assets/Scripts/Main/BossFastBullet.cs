using UnityEngine;

public class BossFastBullet : EnemyBulletMove
{
    protected override void SetVariable()
    {
        speed = 30f;
    }
    public override void Despawn()
    {
        transform.SetParent(gameManager.PoolManager.bossBigBullet.transform,false);
        gameObject.SetActive(false);
    }
}
