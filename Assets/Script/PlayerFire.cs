using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    public GameObject firePos;
    public GameObject bombFactory;

    public float throwPower = 15f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1)) //우클릭 시
        {
            GameObject bomb = Instantiate(bombFactory);
            bomb.transform.position = firePos.transform.position;

            Rigidbody rb = bomb.GetComponent<Rigidbody>();
            
            rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);
        }
    }
}
