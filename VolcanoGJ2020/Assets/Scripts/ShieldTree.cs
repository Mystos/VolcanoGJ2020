using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldTree : MonoBehaviour
{
    private void Update()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, GameManager.Instance.shieldRadius, GameManager.Instance.fireBallLayer);
        for (int i = 0; i < cols.Length; i++)
            Destroy(cols[i].gameObject);
    }
}
