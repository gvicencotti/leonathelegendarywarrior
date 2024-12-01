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

        // Inicializa os parâmetros do Animator
        animator.SetBool("isAttacking", false);
        animator.SetBool("isJumping", false);
    }

    private void Update()
    {
        HandleInput();
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

        if (Input.GetMouseButtonDown(0)) // Botão esquerdo do mouse para atacar
        {
            Attack();
        }
    }

    private void Move(float horizontalInput)
    {
        if (isAttacking) return; // Impede movimento durante o ataque

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

    private void Attack()
    {
        if (isAttacking) return; // Evita ataques enquanto outro ataque está em andamento

        isAttacking = true;
        animator.SetBool("isAttacking", true);

        // Programe o reset do ataque após a duração da animação
        float attackDuration = 0.5f; // Ajuste conforme necessário
        Invoke(nameof(ResetAttack), attackDuration);
    }

    private void ResetAttack()
    {
        isAttacking = false;
        animator.SetBool("isAttacking", false);
    }

    private void UpdateAnimationState()
    {
        bool isGrounded = IsGrounded();
        bool isRunning = Mathf.Abs(rb.linearVelocityX) > 0.1f && !isAttacking;

        animator.SetBool("isRunning", isRunning);

        if (isGrounded && Mathf.Abs(rb.linearVelocityY) < 0.1f)
        {
            isJumping = false;
            animator.SetBool("isJumping", false);
        }
    }

    private bool IsGrounded()
    {
        // Verifica se o personagem está tocando o chão
        return rb.IsTouchingLayers(groundLayer);
    }
}
