using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAction : MonoBehaviour
{
    public GameObject bombEffect;
    private GameObject target;
    private int hitPower = 10;

    private void Update()
    {
        target = GameObject.FindWithTag("Enemy");
    }

    private void OnCollisionEnter(Collision other)
    {
        GameObject eff = Instantiate(bombEffect);

        eff.transform.position = transform.position;

        if (other.gameObject.Equals(target))
        {
            target.GetComponent<EnemyFSM>().HitEnemy(hitPower);
        }

        Destroy(this.gameObject);
    }
}
