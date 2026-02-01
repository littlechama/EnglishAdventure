using System;
using UnityEngine;
using UnityEngine.InputSystem; // Input System

public class PlayerController : MonoBehaviour
{
    [Header("移動設定")]
    public float walkSpeed = 2f;
    public float dashSpeed = 3f;
    public float jumpForce = 4f;

    [Header("接地判定設定")]
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator anim;

    // 入力値を保存する変数
    private Vector2 moveInput;
    private bool isDash;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnDash(InputValue value)
    {
        // ボタンを押している間は true (1)、離すと false (0)
        isDash = value.isPressed;
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded)
        {
            // ダッシュ中ならジャンプ力アップのロジック（元のコードを踏襲）
            float addedForce = isDash ? jumpForce * 0.3f : 0f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce + addedForce);
        }
    }
    // ---------------------------------------------------------

    void Update()
    {
        // 接地判定
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        // アニメーション更新
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("verticalspeed", rb.linearVelocity.y);
        anim.SetFloat("speed", Mathf.Abs(rb.linearVelocity.x));
    }

    void FixedUpdate()
    {
        // 現在のスピード決定
        float currentSpeed = isDash ? dashSpeed : walkSpeed;
        // 移動実行 (moveInput.x は -1.0 から 1.0 の値が入る)
        rb.linearVelocity = new Vector2(moveInput.x * currentSpeed, rb.linearVelocity.y);
        // 向きの反転
        if (moveInput.x > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput.x < 0) transform.localScale = new Vector3(-1, 1, 1);
    }
}