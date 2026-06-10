using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("血量设置")]
    [Tooltip("敌人的最大血量。如果玩家每次攻击造成10点伤害，设为20点就是打两下死")]
    public int maxHealth = 20;

    public int scoreValue = 10;
    
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    
    public void TakeDamage(int damageAmount)
    {

        currentHealth -= damageAmount;
        
        Debug.Log(gameObject.name + " 受到了 " + damageAmount + " 点伤害，剩余血量: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if(ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(scoreValue);
            Debug.Log(gameObject.name + " 被击败了！");
        }
        
        Destroy(gameObject);
    }
}