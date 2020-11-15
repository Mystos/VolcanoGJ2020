using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volcano : MonoBehaviour
{
    public GameObject lavaRockPrefab;

    public void StartEruption()
    {
        int count = Random.Range(GameManager.Instance.minLavaCount, GameManager.Instance.minLavaCount + 1);
        AudioManager.instance.Play("Explosion");
        for (int i = 0; i < count; i++)
        {
            float t = Random.Range(0.2f, 1);
            StartCoroutine(SpawnLava(t));
        }
    }

    public IEnumerator SpawnLava(float time)
    {
        yield return new WaitForSeconds(time);
        LavaRock lavaRock = Instantiate(lavaRockPrefab, transform.position, Quaternion.identity).GetComponent<LavaRock>();
        float force = Random.Range(GameManager.Instance.minForce, GameManager.Instance.maxForce);
        Vector3 randomPos = new Vector3(Random.Range(-GameManager.Instance.launchRadius, GameManager.Instance.launchRadius), 0, Random.Range(-GameManager.Instance.launchRadius, GameManager.Instance.launchRadius));
        lavaRock.Launch(randomPos.normalized + Vector3.up, force);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 0.5f);
        if (GameManager.Instance == null)
            return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.up, GameManager.Instance.launchRadius);
    }
}
