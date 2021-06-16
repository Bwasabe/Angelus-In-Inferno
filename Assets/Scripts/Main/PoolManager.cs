using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public BulletPool bulletPool {get;private set;}
    public EnemyPool enemyPool {get; private set;}
    void Awake(){
        if(!bulletPool)bulletPool = FindObjectOfType<BulletPool>();
        if(!enemyPool)enemyPool = FindObjectOfType<EnemyPool>();
    }
}
