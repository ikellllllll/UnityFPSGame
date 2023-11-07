using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform target;

    // Update is called once per frame
    void Update()
    {
        transform.forward = target.forward; //자기 자신의 방향을 카메라의 방향과 일치시킨다.
    }
}
