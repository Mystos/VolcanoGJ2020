using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    Transform directionTransform;

    // Start is called before the first frame update
    void Start()
    {
        float f = Random.Range(GameManager.Instance.minForce, GameManager.Instance.maxForce);
        Vector3 direction = directionTransform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
