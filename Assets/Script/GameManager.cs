using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //싱글톤 변수
    public static GameManager gm;

    private PlayerMove player;

    private void Awake()
    {
        if (gm == null)
        {
            gm = this; //static 공간 내에 GameManager가 없으면 넣어주기.
        }
    }

    public enum GameState
    {
        Ready,
        Run,
        GameOver
    }

    public GameState gState;

    public GameObject gameLabel;

    private Text gameText;
    // Start is called before the first frame update
    void Start()
    {
        gState = GameState.Ready;//기본 게임 시작 시 Ready
        gameText = gameLabel.GetComponent<Text>();

        gameText.text = "Ready...";

        gameText.color = new Color32(225, 185, 0, 255);

        StartCoroutine(ReadyToStart());

        player = GameObject.Find("Player").GetComponent<PlayerMove>();
    }

    void Update()
    {
        if (GameManager.gm.gState != GameManager.GameState.Run) //게임 중이 아니라면 반환
        {
            return;
        }

        if (player.hp <= 0) //플레이어 사망 시 게임 종료
        {
            gameLabel.SetActive(true); //게임 라벨 활성화

            gameText.text = "Game Over";

            gameText.color = new Color32(255, 0, 0, 255);

            gState = GameState.GameOver;
        }
    }

    IEnumerator ReadyToStart()
    {
        //2초 대기
        yield return new WaitForSeconds(2f);

        gameText.text = "Go!!";
        
        //0.5초간 대기
        yield return new WaitForSeconds(0.5f);
        
        gameLabel.SetActive(false);//대기 후 비활성화

        gState = GameState.Run;
    }
    
}
