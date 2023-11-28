using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class EnemyFSM : MonoBehaviour
{
    enum EnemyStatus
    {
        Idle,
        Move,
        Attack,
        Return,
        Damaged,
        Die
    }

    //Component
    private EnemyStatus m_Status;
    public float findDistance = 8f;
    public float attackDistance = 2f;
    public float moveSpeed = 5f;
    public float moveDistance = 20f;
    
    private Transform player;
    private CharacterController cc;
    
    private float currentTime = 0f;
    private float attackDelay = 0.5f;
    private int attackPower = 3;
    private int hp = 100;
    private int maxHP = 100;
    
    private Vector3 initPos; //초기 위치
    public Slider hpBar;

    // Start is called before the first frame update
    void Start()
    {
        m_Status = EnemyStatus.Idle;
        player = GameObject.FindWithTag("Player").transform;
        cc = GetComponent<CharacterController>();
        initPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        hpBar.transform.position = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
        hpBar.value = (float)hp / (float)maxHP;
        switch (m_Status)
        {
            case EnemyStatus.Idle:
                Idle();
                break;
            case EnemyStatus.Move:
                Move();
                break;
            case EnemyStatus.Attack:
                Attack();
                break;
            case EnemyStatus.Return:
                Return();
                break;
            case EnemyStatus.Damaged:
                Damaged();
                break;
            case EnemyStatus.Die:
                Die();
                break;
        }
    }

    private void Idle()
    {
        if (Vector3.Distance(transform.position, player.position) < findDistance) //플레이어와 적의 거리가 탐색 거리 내에 있을 경우
        {
            m_Status = EnemyStatus.Move;
            Debug.Log("상태전환: Idle -> Move");
        }
    }

    private void Move()
    {
        if (Vector3.Distance(transform.position, initPos) > moveDistance) // 적이 이동 가능한 범위보다 멀리 따라갔을 경우
        {
            m_Status = EnemyStatus.Return;
            Debug.Log("상태전환: Move -> Return");
        }
        else if (Vector3.Distance(transform.position, player.position) > attackDistance) // 플레이어와 적의 거리가 공격 범위 밖이라면
        {
            Vector3 dir = (player.position - transform.position).normalized; //플레이어와 적 방향 벡터 구한 후 정규화

            cc.Move(dir * moveSpeed * Time.deltaTime);
        }
        else
        {
            m_Status = EnemyStatus.Attack;
            Debug.Log("상태전환: Move -> Attack");
        }
    }

    private void Attack() //공격하는 부분
    {
        if (Vector3.Distance(transform.position, player.position) <= attackDistance) //플레이어와 적의 거리가 공격 범위 내라면
        {
            currentTime += Time.deltaTime; //게임 속 시간을 계속 카운트
            
            if (currentTime > attackDelay) //공격 쿨타임
            {
                player.GetComponent<PlayerMove>().DamageAction(attackPower); //PlayerMove 스크립트에서 함수 호출 및 파라미터 인자를 통한 플레이어 데미지
                //애니메이션 실행 [트리거 인수 사용]
                currentTime = 0;
            }
        }
        else
        {
            m_Status = EnemyStatus.Move;//재추격
            Debug.Log("상태전환: Attack -> Move");
            // currentTime = attackDelay; // 재추격 끝나면 바로 공격할 수 있게
        }
    }

    public void AttackDamage()
    {
        player.GetComponent<PlayerMove>().DamageAction(attackPower); //PlayerMove 스크립트에서 함수 호출 및 파라미터 인자를 통한 플레이어 데미지
    }

    private void Damaged()
    {
        StartCoroutine(DamageProcess());//코루틴 함수 호출
    }

    private void Return()
    {
        if (Vector3.Distance(transform.position, initPos) > 0.1f) // 초기 위치에서 현재 위치가 0.1f 보다 큰 경우
        {
            Vector3 dir = (initPos - transform.position).normalized;

            cc.Move(dir * moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = initPos; //아니라면 초기 위치로 텔포
            hp = maxHP; //체력 초기화
            m_Status = EnemyStatus.Idle; //대기 상태로 초기화
            Debug.Log("상태전환: Return -> Idle");
        }
    }

    private void Die()
    {
        StopAllCoroutines();//죽으면 모든 코루틴 함수를 멈춘다.

        StartCoroutine(DieProcess());
    }
    public void DamageAction(int damage)
    {
        hp -= damage;
    }
    //코루틴 사용
    IEnumerator DamageProcess() //데미지 처리용 함수
    {
        hpBar.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        m_Status = EnemyStatus.Move;
        Debug.Log("상태전환: Damaged -> Move");
    }

    public void HitEnemy(int hitPower)
    {
        hp -= hitPower;

        if (hp > 0)
        {
            m_Status = EnemyStatus.Damaged;
            Debug.Log("상태전환: Any State -> Damaged");
            Damaged();
        }
        else
        {
            m_Status = EnemyStatus.Die;
            Debug.Log("상태전환: Any State -> Die");
            Die();
        }
    }

    IEnumerator DieProcess()
    {
        hpBar.gameObject.SetActive(false);
        cc.enabled = false;
        yield return new WaitForSeconds(2f);
        Debug.Log("소멸");
        Destroy(gameObject);
    }

    
}
