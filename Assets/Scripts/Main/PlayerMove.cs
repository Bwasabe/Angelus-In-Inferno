using System.Collections;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("총알")]
    [SerializeField]
    private GameObject bulletPrefab = null;
    [SerializeField]
    private Transform bulletPosition = null;
    [SerializeField]
    private float bulletDelay = 0.3f;
    [SerializeField]
    private Vector2 bulletScale = Vector2.one;
    [SerializeField]
    private Sprite playerRingSprite = null;


    [Header("플레이어")]
    [SerializeField]
    private float speed = 10f;
    private Vector2 playerPosition = Vector2.zero; 

    private GameManager gameManager = null;
    private SpriteRenderer spriteRenderer = null;
    private Animator animator = null;
    private GameObject bullet = null;
    private bool isRing = false;
    void Start()
    {
        if(!gameManager)gameManager = FindObjectOfType<GameManager>();
        if(!spriteRenderer)spriteRenderer = GetComponent<SpriteRenderer>();
        if(!animator)animator = GetComponent<Animator>();
        isRing = true;
        StartCoroutine(AFire());

    }
    
     void Update()
    {
        OnClick();
    }
     private void OnClick()
    {
        if (Input.GetMouseButton(0))
        {
            playerPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            playerPosition.x = Mathf.Clamp(playerPosition.x, gameManager.MinPosition.x, gameManager.MaxPositon.x);
            playerPosition.y = Mathf.Clamp(playerPosition.y, gameManager.MinPosition.y, gameManager.MaxPositon.y);    
            transform.position = Vector2.MoveTowards(transform.position, playerPosition, speed * Time.deltaTime);
        }
    }
    private IEnumerator AFire(){
        while(true){
            SpawnOrInstantiate();
            yield return new WaitForSeconds(bulletDelay);
        }
    }
    private void SpawnOrInstantiate(){
        if(gameManager.PoolManager.transform.childCount > 0){
            bullet = gameManager.PoolManager.transform.GetChild(0).gameObject;
            bullet.layer = LayerMask.NameToLayer("Player");
            JudgeBullet();
            bullet.SetActive(true);
            bullet.transform.rotation = Quaternion.identity;
            bullet.transform.SetParent(bulletPosition,false);
            bullet.transform.position = bulletPosition.position;
        }
        else{
            bullet = Instantiate(bulletPrefab,bulletPosition);
        }
        if(bullet != null){
            bullet.transform.SetParent(null);
        }
    }
    private void JudgeBullet(){
        if(isRing == true)
        bullet.GetComponent<SpriteRenderer>().sprite = playerRingSprite;
    }
}
