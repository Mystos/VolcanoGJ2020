using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaRock : MonoBehaviour
{
    public Rigidbody rigidbody;

    private float minVelocity = 0.2f;
    float lifeTime = 0f;
    public float maxLifeTime = 15;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

        }
        lifeTime += Time.deltaTime;
        if (lifeTime > maxLifeTime)
            Destroy(gameObject);
    }

    public void Launch(Vector3 direction, float force)
    {
        rigidbody.AddForce(force * direction);
    }
}
