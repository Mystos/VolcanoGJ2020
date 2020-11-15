using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaRock : MonoBehaviour
{
    public Rigidbody rigidbody;
    public float maxLifeTime = 15;

    float lifeTime = 0f;

    // Update is called once per frame
    void Update()
    {
        lifeTime += Time.deltaTime;
        if (lifeTime > maxLifeTime)
            Destroy(gameObject);
    }

    public void Launch(Vector3 direction, float force)
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.AddForce(force * direction);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == GameManager.Instance.vegetalTag)
        {
            GameManager.Instance.Score--;
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
