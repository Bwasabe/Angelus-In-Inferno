using UnityEngine;

public class BulletMove : MonoBehaviour
{
    [SerializeField]
    private float speed = 10f;

    private GameManager gameManager = null;
    void Start()
    {
        if(!gameManager)gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        transform.Translate(Vector2.up*speed*Time.deltaTime);
        Limit();
    }
    private void Limit(){
        if(transform.localPosition.y > gameManager.MaxPositon.y+0.5f){
            Despawn();
        }
    }
    public void Despawn(){
        transform.localScale = new Vector2(1,1);
        transform.SetParent(gameManager.PoolManager.transform,false);
        gameObject.SetActive(false);
    }
}
