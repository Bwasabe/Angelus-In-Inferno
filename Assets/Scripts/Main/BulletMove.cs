using UnityEngine;

public class BulletMove : MonoBehaviour
{
    [SerializeField]
    protected float speed = 10f;

    protected GameManager gameManager = null;
    void Awake()
    {
        if(!gameManager)gameManager = FindObjectOfType<GameManager>();
        SetVariable();
    }

    void Update()
    {
        transform.Translate(Vector2.up*speed*Time.deltaTime);
        Limit();
    }
    protected virtual void SetVariable(){
        speed = 10f;
    }
    protected virtual void Limit(){
        if(transform.localPosition.y > gameManager.MaxPositon.y+0.5f){
            Despawn();
        }
    }
    public virtual void Despawn(){
        //transform.localScale = new Vector2(1,1);
        transform.SetParent(gameManager.PoolManager.bulletPool.transform,false);
        gameObject.SetActive(false);
    }
}
