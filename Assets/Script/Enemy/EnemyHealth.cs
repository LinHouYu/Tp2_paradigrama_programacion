using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("血量设置")]
    [Tooltip("敌人的最大血量。如果玩家每次攻击造成10点伤害，设为20点就是打两下死")]
    public int maxHealth = 20;

    public int scoreValue = 10;
    
    private int currentHealth;
    private Animator anim; 

    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>(); 
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log(gameObject.name + " 受到了 " + damageAmount + " 点伤害，剩余血量: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            anim.SetTrigger("doHit"); 
        }
    }

    void Die()
    {
        if(ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(scoreValue);
            Debug.Log(gameObject.name + " 被击败了！");
        }
        
        // 1. 播放死亡动画
        anim.SetTrigger("doDie");

        // 2. 禁用移动和攻击脚本
        GetComponent<EnemyMove>().enabled = false;
        GetComponent<EnemyAttack>().enabled = false;
        GetComponent<Collider2D>().enabled = false; // 禁用碰撞体

        // 🌟 核心修复：彻底冻结刚体物理效果，实现原地死亡 🌟
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero; // 清除残留的移动速度
            rb.simulated = false; // 彻底关闭这个物体的物理模拟（不受重力、碰撞任何影响）
        }

        // 3. 延迟 1.5 秒销毁尸体 (具体时间根据你的 Die 动画长度决定)
        Destroy(gameObject, 1.5f); 
    }
}