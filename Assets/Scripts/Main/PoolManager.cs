using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public BulletPool bulletPool {get;private set;}
    public EnemyPool enemyPool {get; private set;}
    public EnemyPurplePool enemyPurplePool {get; private set;}
    public EnemyBullet enemyBullet {get; private set;}
    void Awake(){
        if(!bulletPool)bulletPool = FindObjectOfType<BulletPool>();
        if(!enemyPool)enemyPool = FindObjectOfType<EnemyPool>();
        if(!enemyPurplePool)enemyPurplePool = FindObjectOfType<EnemyPurplePool>();
        if(!enemyBullet)enemyBullet = FindObjectOfType<EnemyBullet>();
    }
}
