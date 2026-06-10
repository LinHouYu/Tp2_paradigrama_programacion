using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("攻击设置")]
    public int damage = 10;           
    public float attackCooldown = 1f;  
    
    private float nextAttackTime;

    //当敌人持续碰撞到任何物体时触发
    void OnCollisionStay2D(Collision2D collision)
    {
        //检查碰撞到的物体是不是玩家
        if (collision.gameObject.CompareTag("Player"))
        {
            //检查冷却时间是否已到
            if (Time.time >= nextAttackTime)
            {
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.TakeDamage(damage);
                    nextAttackTime = Time.time + attackCooldown; //刷新下一次可以伤害的时间
                }
            }
        }
    }
}