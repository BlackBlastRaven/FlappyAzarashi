using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AzarashiController : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Animator animator;
    private float angle;
    private bool isDead;

    public float maxHeight;

    public float flapVelocity;

    public float relativeVelocityX;

    public GameObject sprite;
    
    // Start is called before the first frame update

    public bool IsDead()
    {
        return isDead;
    }

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = sprite.GetComponent<Animator>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //最高高度に達していなければタップを受け付ける
        if (Input.GetButtonDown("Fire1") && transform.position.y < maxHeight)
        {
            Flap();
        }
        
        //角度を反映させる
        ApplyAngle();
        
        //angleが水平以上の場合、アニメーターのflapフラグをtrueにする(羽ばたくモーションにする)(後ゲームオーバーじゃなければ)
        animator.SetBool("flap",angle >= 0.0f && !isDead);
    }

    public void Flap()
    {
        //死んだら行動不能
        if (isDead)
        {
            return;
        }
        
        //Velocityを書き換えることで上方向に加速させる
        rb2d.velocity = new Vector2(0.0f,flapVelocity);
    }

    void ApplyAngle()
    {
        //現在の速度、相対速度から進んでいる角度を求める
        float targetAngle;
        
        //死亡時は常にひっくり返る
        if (isDead)
        {
            targetAngle = 180.0f;
        }
        else
        {
            targetAngle = Mathf.Atan2(rb2d.velocity.y, relativeVelocityX) * Mathf.Rad2Deg;
        }
        
        //回転アニメーションをスムージングする
        angle = Mathf.Lerp(angle, targetAngle, Time.deltaTime * 10.0f);
        
        //Rotaitionの反映
        sprite.transform.localRotation = Quaternion.Euler(0.0f,0.0f,angle);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead)
        {
            return;
        }
        
        //なにかにぶつかったら死亡フラグを建てる
        isDead = true;
    }
}
