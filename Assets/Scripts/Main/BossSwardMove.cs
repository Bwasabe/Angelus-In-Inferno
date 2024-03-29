using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSwardMove : BulletMove
{
    protected override void Update()
    {
        transform.Translate(Vector3.down*speed*Time.deltaTime);
        Limit();
    }
    protected override void Limit()
    {
        if(transform.localPosition.y < _gameManager.MinPosition.y - 4f){
            Despawn();
        }
    }
    protected override void SetVariable()
    {
        speed = 10f;
    }
    public override void Despawn()
    {
        transform.SetParent(_gameManager.PoolManager.bossSwardPool.transform,false);
        gameObject.SetActive(false);
    }
}
