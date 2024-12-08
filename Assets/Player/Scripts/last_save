using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration = 0.5f;

    [Header("Ground Layer")]
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isJumping;
    private bool isDashAttacking;
    private bool isGrounded;
    private float dashTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        HandleInput();
        UpdateAnimationState();
    }

    private void FixedUpdate()
    {
        if (isDashAttacking)
        {
            DashMove();
        }
        CheckGroundStatus();
    }

    private void HandleInput()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        Move(horizontalInput);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        if (Input.GetMouseButtonDown(0) && !isDashAttacking)
        {
            HandleAttack();
        }

        if (Input.GetMouseButtonDown(1) && !isDashAttacking && !isJumping)
        {
            HandleDashAttack(horizontalInput);
        }
    }

    private void Move(float horizontalInput)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && !isDashAttacking)
        {
            return;
        }

        rb.linearVelocityX = horizontalInput * moveSpeed;
        rb.linearVelocityY = rb.linearVelocityY;

        if (horizontalInput != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(horizontalInput), 1, 1);
        }
    }

    private void DashMove()
    {
        rb.linearVelocityX = Mathf.Sign(transform.localScale.x) * dashSpeed;
        rb.linearVelocityY = rb.linearVelocityY;
        dashTime += Time.deltaTime;

        if (dashTime >= dashDuration)
        {
            ResetDashAttack();
        }
    }

    private void Jump()
    {
        rb.linearVelocityY = jumpForce;
        isJumping = true;
        isGrounded = false;

        animator.SetBool("isJumping", true);
        Debug.Log("Jump initiated");
    }

    private void HandleAttack()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            Debug.Log("Cannot attack, already attacking.");
            return;
        }

        animator.SetTrigger("isAttacking");
        rb.linearVelocityX = 0;

        Debug.Log("Attack Triggered");
    }

    private void HandleDashAttack(float horizontalInput)
    {
        isDashAttacking = true;
        animator.SetTrigger("isDashAttacking");

        rb.linearVelocityX = horizontalInput * moveSpeed;
        rb.linearVelocityY = rb.linearVelocityY;
        dashTime = 0;

        Debug.Log("Dash Attack Triggered");
    }

    private void ResetDashAttack()
    {
        isDashAttacking = false;
        animator.ResetTrigger("isDashAttacking");

        Debug.Log("Dash Attack Reset");
    }

    private void UpdateAnimationState()
    {
        bool isRunning = Mathf.Abs(rb.linearVelocityX) > 0.1f && !isJumping && !isDashAttacking;
        bool isFalling = !isGrounded && rb.linearVelocityY < 0;
        bool isIdle = isGrounded && Mathf.Abs(rb.linearVelocityX) < 0.1f && !isJumping;

        animator.SetBool("isRunning", isRunning && !isFalling);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isFalling", isFalling);
        animator.SetBool("isIdle", isIdle);

        if (isFalling)
        {
            Debug.Log("isFalling set to true");
        }
        else
        {
            Debug.Log("isFalling set to false");
        }

        if (isGrounded && Mathf.Abs(rb.linearVelocityY) < 0.1f)
        {
            isJumping = false;
            animator.SetBool("isJumping", false);
        }

        Debug.Log($"Animation States -> isGrounded: {isGrounded}, isJumping: {isJumping}, isFalling: {animator.GetBool("isFalling")}, isRunning: {isRunning}, isIdle: {isIdle}");
    }

    private void CheckGroundStatus()
    {
        bool wasGrounded = isGrounded;
        isGrounded = rb.IsTouchingLayers(groundLayer);

        if (isGrounded != wasGrounded)
        {
            Debug.Log($"isGrounded changed to: {isGrounded}");
        }
    }
}
