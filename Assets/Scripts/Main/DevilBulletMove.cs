using UnityEngine;

public class DevilBulletMove : BulletMove
{
    protected override void Update()
    {
        transform.Translate(Vector2.left*speed * Time.deltaTime);
        Limit();
    }
    protected override void SetVariable()
    {
        speed = 15f;
    }
    protected override void Limit()
    {
        if(transform.localPosition.x < GameManager.Instance.MinPosition.x - 1f){
            Despawn();
        }
    }
    public override void Despawn()
    {
        transform.SetParent(GameManager.Instance.PoolManager.devilSkillPool.transform,false);
        gameObject.SetActive(false);
    }
}
