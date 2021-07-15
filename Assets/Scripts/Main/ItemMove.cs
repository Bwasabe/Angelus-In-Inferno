using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMove : MonoBehaviour
{
    [SerializeField]
    private Sprite[] sprite;
    private EnemyMove enemyMove = null;
    private CircleCollider2D circol = null;
    private SpriteRenderer spriteRenderer = null;
    private int index = 0;


    void Awake()
    {
        if (!enemyMove) enemyMove = FindObjectOfType<EnemyMove>();
        if (!circol) circol = FindObjectOfType<CircleCollider2D>();
        if (!spriteRenderer) spriteRenderer = FindObjectOfType<SpriteRenderer>();
    }
    void Update()
    {
        Limit();
    }
    void OnEnable()
    {
        JudgeItem();
    }
    private void Limit()
    {
        if (transform.localPosition.y < GameManager.Instance.MinPosition.y - 1f)
        {
            Despawn();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            JudgeIndex();
            Despawn();
        }
    }
    public void JudgeItem()
    {
        index = Random.Range(0, 4);

        if (index >= 1) index = 1;
        if (GameManager.Instance.Player.isAngel)
        {
            spriteRenderer.sprite = sprite[index];
        }
        else if(GameManager.Instance.Player.isDevil){
            spriteRenderer.sprite = sprite[index+2];
        }

        //  if(enemyMove.isFast == true){
        //     circol.radius = 1f;
        //     GameManager.Instance.changeCount += 1;
        // }
        // if(index == 2){
        //     circol.radius = 0.16f;
        //     GameManager.Instance.delayCount += 1;
        // }
    }
    private void JudgeIndex()
    {
        switch (index)
        {
            case 0:
                {
                    GameManager.Instance.changeCount += 1;
                    break;
                }
            case 1:
                {
                    GameManager.Instance.delayCount += 1;
                    break;
                }
        }
    }
    public void ItemIndex(int num)
    {
        index = num;
    }
    private void Despawn()
    {
        gameObject.SetActive(false);
        transform.SetParent(GameManager.Instance.PoolManager.fastSkillPool.transform, false);
    }
}