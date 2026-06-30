using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("攻击设置")]
    public int damage = 10;          
    public float attackCooldown = 1f; 
    
    private float nextAttackTime;
    private Animator anim; // 新增：动画控制器

    void Start()
    {
        anim = GetComponent<Animator>(); // 新增：获取组件
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Time.time >= nextAttackTime)
            {
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.TakeDamage(damage);
                    anim.SetTrigger("doAttack"); // 新增：触发攻击动画
                    nextAttackTime = Time.time + attackCooldown; 
                }
            }
        }
    }
}