using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [Header("生成设置")]
    [Tooltip("在这里拖入敌人的预制体")]
    public GameObject enemyPrefab; 
    
    [Tooltip("每次生成敌人的时间间隔（秒）")]
    public float spawnInterval = 2f; 

    [Tooltip("场景中敌人同时存在的最大数量上限")]
    public int maxEnemyCount = 5; 

    private float timer; // 内部计时器

    void Start()
    {
        // 游戏开始时重置计时器
        timer = spawnInterval; 
    }

    void Update()
    {
        // 倒计时
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            if (GetCurrentEnemyCount() < maxEnemyCount)
            {
                SpawnEnemy(); // 数量没超标，允许生成
            }
            else
            {
                Debug.Log("敌人生成已到达了上线");
            }

            timer = spawnInterval;
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefab != null)
        {
            Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("未分配敌人预制体！请在 Inspector 面板中把敌人拖到 EnemyPrefab 槽位里。");
        }
    }

    
    int GetCurrentEnemyCount()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        
        return enemies.Length;
    }
}