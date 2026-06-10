using System.Collections;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("移动设置")]
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 mousePos;
    private Camera viewCamera;

    [Header("玩家UI Rect")]
    public RectTransform healthFillRect; 
    public RectTransform dashFillRect;
    public RectTransform skillFillRect; 
    public float healthBarFullWidth = 200f; 
    public float dashBarFullWidth = 150f;
    public float skillBarFullWidth = 150f; 
    
    [Header("UI文本")]
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI dashText;
    public TextMeshProUGUI skillText; 

    [Header("冲刺设置")]
    public float dashSpeed = 15f;    
    public float dashDuration = 0.2f; 
    public float dashCooldown = 1f;   
    private float dashCooldownTimer;
    private bool isDashing;
    private bool canDash = true;

    [Header("普通攻击")]
    public GameObject slashEffectPrefab; 
    public Transform attackPoint;         
    public float attackRange = 1.5f;      
    public float attackCooldown = 0.3f;   
    public int attackDamage = 10;         
    public LayerMask enemyLayers;        
    private bool isAttacking;

    [Header("右键大招设置")]
    public GameObject skillPrefab; // 大招特效预制体槽位
    public float skillRange = 4f; 
    public int skillDamage = 20; 
    public float skillCooldown = 5f; 
    private float skillCooldownTimer;
    private bool canSkill = true;
    private bool isSkilling;

    private int maxHealth = 100;
    private int currentHealth;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        viewCamera = Camera.main; 
        currentHealth = maxHealth;

        UpdateHealthUI();
        UpdateDashUI(1f);
        UpdateSkillUI(1f);

        if (enemyLayers == 0) enemyLayers = LayerMask.GetMask("Enemy");
    }

    void Update()
    {
        HandleTimers();

        if (isDashing || isAttacking || isSkilling)
        {
            moveInput = Vector2.zero;
            return;
        }

        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput = moveInput.normalized;
        mousePos = viewCamera.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetKeyDown(KeyCode.Space) && canDash) StartCoroutine(Dash());
        if (Input.GetMouseButtonDown(0) && !isAttacking) StartCoroutine(Attack());
        if (Input.GetMouseButtonDown(1) && canSkill) StartCoroutine(ReleaseSkill()); 
    }

    void FixedUpdate()
    {
        if (isDashing || isSkilling) return;
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }

    void HandleTimers()
    {
        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
            UpdateDashUI(1f - (dashCooldownTimer / dashCooldown));
            if (dashText != null) dashText.text = Mathf.Max(0f, dashCooldownTimer).ToString("F1") + "s";
        }
        else if (!isDashing)
        {
            canDash = true;
            UpdateDashUI(1f);
            if (dashText != null) dashText.text = "READY";
        }

        if (skillCooldownTimer > 0)
        {
            skillCooldownTimer -= Time.deltaTime;
            UpdateSkillUI(1f - (skillCooldownTimer / skillCooldown));
            if (skillText != null) skillText.text = Mathf.Max(0f, skillCooldownTimer).ToString("F1") + "s";
        }
        else if (!isSkilling)
        {
            canSkill = true;
            UpdateSkillUI(1f);
            if (skillText != null) skillText.text = "SKILL";
        }
    }

    void UpdateHealthUI()
    {
        if (healthFillRect != null)
        {
            float ratio = (float)currentHealth / maxHealth;
            healthFillRect.sizeDelta = new Vector2(healthBarFullWidth * ratio, healthFillRect.sizeDelta.y);
        }
        if (healthText != null) healthText.text = currentHealth + " / " + maxHealth;
    }

    void UpdateDashUI(float ratio)
    {
        if (dashFillRect != null) dashFillRect.sizeDelta = new Vector2(dashBarFullWidth * ratio, dashFillRect.sizeDelta.y);
    }

    void UpdateSkillUI(float ratio)
    {
        if (skillFillRect != null) skillFillRect.sizeDelta = new Vector2(skillBarFullWidth * ratio, skillFillRect.sizeDelta.y);
    }

    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        Vector2 dashDirection = moveInput == Vector2.zero ? ((Vector2)mousePos - rb.position).normalized : moveInput;
        rb.linearVelocity = dashDirection * dashSpeed;
        yield return new WaitForSeconds(dashDuration);
        rb.linearVelocity = Vector2.zero;
        isDashing = false;
        dashCooldownTimer = dashCooldown;
    }
    
    IEnumerator Attack()
    {
        isAttacking = true;
        Vector2 attackDirection = (mousePos - (Vector2)attackPoint.position).normalized;
        Vector2 finalAttackPoint = (Vector2)attackPoint.position + attackDirection * attackRange;

        if (slashEffectPrefab != null)
        {
            float angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
            GameObject slash = Instantiate(slashEffectPrefab, finalAttackPoint, Quaternion.Euler(0, 0, angle));
            Destroy(slash, 1f);
        }
        
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(finalAttackPoint, attackRange * 0.5f, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null) enemyHealth.TakeDamage(attackDamage);
        }

        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    IEnumerator ReleaseSkill()
    {
        canSkill = false;
        isSkilling = true;
        rb.linearVelocity = Vector2.zero; 

        // 在玩家中心生成大招预制体特效
        if (skillPrefab != null)
        {
            GameObject vfx = Instantiate(skillPrefab, transform.position, Quaternion.identity);
            Destroy(vfx, 1.5f); // 1.5秒后自动销毁（可根据粒子实际时长微调）
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, skillRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null) enemyHealth.TakeDamage(skillDamage);
        }

        yield return new WaitForSeconds(0.3f); 
        isSkilling = false;
        skillCooldownTimer = skillCooldown;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthUI();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (attackPoint != null) Gizmos.DrawWireSphere(attackPoint.position, attackRange * 0.5f);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, skillRange);
    }
}