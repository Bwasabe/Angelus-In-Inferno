using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public BulletPool bulletPool { get; private set; }
    public EnemyPool enemyPool { get; private set; }
    public EnemyPurplePool enemyPurplePool { get; private set; }
    public EnemyBullet enemyBullet { get; private set; }
    public FastSkillPool fastSkillPool { get; private set; }
    public SkillPool skillPool { get; private set; }
    public BossPool bossPool { get; private set; }
    public BossBullet bossBullet { get; private set; }
    public BossBigBullet bossBigBullet { get; private set; }
    public BossPurplePool bossPurplePool { get; private set; }
    public BossSwardPool bossSwardPool { get; private set; }
    public DevilSkillPool devilSkillPool { get; private set; }

    void Awake()
    {
        if (!devilSkillPool) devilSkillPool = FindObjectOfType<DevilSkillPool>();
        if (!bulletPool) bulletPool = FindObjectOfType<BulletPool>();
        if (!enemyPool) enemyPool = FindObjectOfType<EnemyPool>();
        if (!enemyPurplePool) enemyPurplePool = FindObjectOfType<EnemyPurplePool>();
        if (!enemyBullet) enemyBullet = FindObjectOfType<EnemyBullet>();
        if (!fastSkillPool) fastSkillPool = FindObjectOfType<FastSkillPool>();
        if (!skillPool) skillPool = FindObjectOfType<SkillPool>();
        if (!bossPool) bossPool = FindObjectOfType<BossPool>();
        if (!bossBullet) bossBullet = FindObjectOfType<BossBullet>();
        if (!bossBigBullet) bossBigBullet = FindObjectOfType<BossBigBullet>();
        if (!bossPurplePool) bossPurplePool = FindObjectOfType<BossPurplePool>();
        if (!bossSwardPool) bossSwardPool = FindObjectOfType<BossSwardPool>();
    }
}
