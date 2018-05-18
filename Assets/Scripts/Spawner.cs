using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
	public float spawnTime = 5f;		// The amount of time between each spawn.
	public float spawnDelay = 3f;		// The amount of time before spawning starts.
	public GameObject[] enemies;		// Array of enemy prefabs.
    public bool status;

	void Start ()
	{
        status = false;
		// Start calling the Spawn function repeatedly after a delay .
		InvokeRepeating("Spawn", spawnDelay, spawnTime);
	}


	void Spawn ()
	{
        if (status)
        {
            // Instantiate a random enemy.
            int enemyIndex = Random.Range(0, enemies.Length);
            Instantiate(enemies[enemyIndex], transform.position, transform.rotation);

            // Play the spawning effect from all of the particle systems.
            foreach (ParticleSystem p in GetComponentsInChildren<ParticleSystem>())
            {
                p.Play();
            }
        }
	}
}
