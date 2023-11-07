using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    public GameObject firePos;
    public GameObject bombFactory;
    public float throwPower = 15f;
    public GameObject bulletEffect;
    private int weaponPower = 5; //무기 공격력
    
    //피격 이펙트 파티클 시스템
    private ParticleSystem ps;

    private void Start()
    {
        ps = bulletEffect.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gm.gState != GameManager.GameState.Run) //게임 중이 아니라면 반환
        {
            return;
        }
        
        if (Input.GetMouseButtonDown(1)) //우클릭 시
        {
            GameObject bomb = Instantiate(bombFactory);
            bomb.transform.position = firePos.transform.position;

            Rigidbody rb = bomb.GetComponent<Rigidbody>();
            
            rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);
        }

        if (Input.GetMouseButtonDown(0))
        {
            //Ray -> 광선
            // 객체 생성 시 카메라 위치에 생성하고 광선 방향은 카메라 방향 기준 앞쪽으로 = 에임
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            
            // 광선이 부딪힌 대상의 정보를 저장할 변수 생성
            RaycastHit hitInfo = new RaycastHit();

            //광선을 발사한 후 만일 부딪힌 물체가 있으면 피격 이펙트 표시
            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy")) //광선에 맞은 대상의 layer가 Enemy이라면
                {
                    EnemyFSM eFsm = hitInfo.transform.GetComponent<EnemyFSM>();
                    eFsm.HitEnemy(weaponPower);
                }
                else
                {
                    //피격 이펙트의 위치를 레이가 부딪힌 지저므로 이동
                    bulletEffect.transform.position = hitInfo.point;

                    //피격 이펙트의 forward 방향을 레이가 부딪힌 지점의 법선 벡터와 일치시킨다.
                    bulletEffect.transform.forward = hitInfo.normal;
                    //피격 이펙트를 플레이
                    ps.Play();
                }
            }
        }
    }
}
