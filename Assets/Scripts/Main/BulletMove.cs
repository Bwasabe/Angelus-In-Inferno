using UnityEngine;

public class BulletMove : MonoBehaviour
{
    [SerializeField]
    protected float speed = 10f;
    protected GameManager _gameManager;
    void Awake()
    {
        SetVariable();
        _gameManager = FindObjectOfType<GameManager>();
    }

    protected virtual void Update()
    {
        transform.Translate(Vector2.up*speed*Time.deltaTime);
        Limit();
    }
    protected virtual void SetVariable(){
        speed = 10f;
    }
    protected virtual void Limit(){
        if(transform.localPosition.y > _gameManager.MaxPositon.y+0.5f){
            Despawn();
        }
    }
    public virtual void Despawn(){
        transform.SetParent(_gameManager.PoolManager.bulletPool.transform,false);
        gameObject.SetActive(false);
    }
}
