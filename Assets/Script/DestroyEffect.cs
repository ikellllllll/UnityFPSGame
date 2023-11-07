using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
    public float destoryTime = 1.5f;

    private float currentTime = 0;

    // Update is called once per frame
    void Update()
    {
        if (currentTime > destoryTime)
        {
            Destroy(gameObject);
        }
        //경과 시간 누적
        currentTime += Time.deltaTime;
    }
}
