using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaRock : MonoBehaviour
{
    public Rigidbody rigidbody;
    public float maxLifeTime = 15;

    float lifeTime = 0f;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            Launch(transform.forward, GameManager.Instance.maxForce);

        lifeTime += Time.deltaTime;
        if (lifeTime > maxLifeTime)
            Destroy(gameObject);
    }

    public void Launch(Vector3 direction, float force)
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.AddForce(force * direction);
    }
}
