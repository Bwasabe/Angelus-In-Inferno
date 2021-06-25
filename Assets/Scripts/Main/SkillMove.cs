using UnityEngine;

public class SkillMove : MonoBehaviour
{
    [SerializeField]
    private float speed = 0.5f;
    private GameManager gameManager = null;
    private PlayerMove playerMove = null;
    void Start(){
        if(!gameManager)gameManager = FindObjectOfType<GameManager>();
        if(!playerMove)playerMove = FindObjectOfType<PlayerMove>();  
    }
    void Update(){
        Move();
        Limit();
    }
    private void Move(){
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }
    private void Limit(){
        if(transform.localPosition.y > gameManager.MaxPositon.y + 5f ){
            Despawn();
        }
    }
    private void Despawn(){
        transform.SetParent(gameManager.PoolManager.skillPool.transform, false);
        gameObject.SetActive(false);
    }
}
