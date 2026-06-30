using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class EnemyAnimation : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private Vector3 lastPosition;

    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastPosition = transform.position; 
    }

    // 🌟 核心修复：将 Update 修改为 FixedUpdate 🌟
    // 这样就和 EnemyMove.cs 里的移动频率完美对齐了
    void FixedUpdate() 
    {
        Vector3 movement = transform.position - lastPosition;
        
        bool isWalking = movement.magnitude > 0.001f;
        anim.SetBool("isWalking", isWalking);

        if (movement.x > 0.001f)
        {
            spriteRenderer.flipX = true; // 如果你的原图是朝左的，这里要改成 false
        }
        else if (movement.x < -0.001f)
        {
            spriteRenderer.flipX = false; 
        }

        lastPosition = transform.position;
    }
}