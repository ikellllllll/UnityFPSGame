using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{   
    //플레이어 움직임 속도
    public float moveSpeed = 7f;
    private CharacterController cc;
    // 중력
    private float gravity = -20f;
    //수직 속력 변수
    private float yVelocity = 0;

    public float jumpPower = 10f;

    public bool isJumping = false;

    public int hp = 100;
    private int maxHP = 100;

    public Slider hpSlider;
    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        hpSlider.value = (float)hp / (float)maxHP;
        
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;

        dir = Camera.main.transform.TransformDirection(dir);

        if (cc.collisionFlags == CollisionFlags.Below)
        {
            if (isJumping)
            {
                isJumping = false;
                yVelocity = 0;
            }
        }
        
        //Space 키 누르면 점프
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            // 캐릭터 수직 속도에 점프력 적용
            yVelocity = jumpPower;
        }
        //캐릭터 수직 속도에 중력 값을 적용
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;
        // 이동 속도에 맞춰 이동
        cc.Move(dir * moveSpeed * Time.deltaTime);
    }
    public void DamageAction(int damage)
    {
        hp -= damage;
    }
}
