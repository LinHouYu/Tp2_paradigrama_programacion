using UnityEngine;

// 同样自动添加 Rigidbody2D，防止敌人穿模
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMove : MonoBehaviour
{
    [Header("敌人设置")]
    public float moveSpeed = 3f; 
    
    private Transform playerTransform; // 存储玩家的 Transform (位置信息)
    private Rigidbody2D rb;

    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();

        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        if (player != null)
        {
            playerTransform = player.transform; // 找到玩家后，把位置信息存下来
        }
        else
        {
            Debug.LogWarning("场景中没有找到标签为 'Player' 的物体！请检查玩家的 Tag 设置。");
        }
    }

    
    void FixedUpdate()
    {
        
        if (playerTransform != null)
        {
    
            Vector2 direction = (playerTransform.position - transform.position);
            
            
            direction = direction.normalized;

            
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
        }
    }
}