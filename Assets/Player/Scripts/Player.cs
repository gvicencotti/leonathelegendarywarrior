using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;

    [Header("Ground Layer")]
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isJumping;
    private bool isAttacking;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        HandleInput();
        UpdateAnimationState();
        HandleAttack();
        UpdateAnimationState();
    }

    private void HandleInput()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        Move(horizontalInput);

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            Jump();
        }
    }

    private void Move(float horizontalInput)
    {
        // Impede movimento se o personagem estiver no estado de ataque
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            // Leve movimento durante o ataque na direção em que o personagem está virado
            rb.linearVelocityX = 0.5f * Mathf.Sign(transform.localScale.x);
            return;
        }

        // Define o movimento horizontal
        rb.linearVelocityX = horizontalInput * moveSpeed;

        // Vira o sprite do personagem com base na direção do movimento
        if (horizontalInput != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(horizontalInput), 1, 1);
        }
    }

    private void Jump()
    {
        rb.linearVelocityY = jumpForce;
        isJumping = true;
        animator.SetBool("isJumping", true);
    }

    private void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            isAttacking = true;
            animator.SetBool("isAttacking", true);

            // Obtém a duração da animação de ataque no estado atual
            float attackDuration = animator.GetCurrentAnimatorStateInfo(0).length;
            Invoke(nameof(ResetAttack), attackDuration); // Ajusta automaticamente para o tempo correto
        }
    }

    private void ResetAttack()
    {
        isAttacking = false;
        animator.SetBool("isAttacking", false);
    }


    private void UpdateAnimationState()
    {
        bool isGrounded = IsGrounded();
        bool isRunning = Mathf.Abs(rb.linearVelocityX) > 0.1f && !isJumping && !isAttacking;

        animator.SetBool("isRunning", isRunning);

        if (isGrounded && Mathf.Abs(rb.linearVelocityY) < 0.1f)
        {
            isJumping = false;
            animator.SetBool("isJumping", false);
        }
    }

    private bool IsGrounded()
    {
        return rb.IsTouchingLayers(groundLayer);
    }
}