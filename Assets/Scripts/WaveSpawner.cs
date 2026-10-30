using UnityEngine;
using System.Collections;

public class WaveSpawner : MonoBehaviour {

	public static int EnemiesAlive = 0;

	public Wave[] waves;

	public Transform spawnPoint;

	public float timeBetweenWaves = 5f;
	private float countdown = 2f;

	public GameManager gameManager;

	private int waveIndex = 0;
	private bool isStopped;
	public int CurrentWave { get; private set; }
	public int WaveCount { get { return waves == null ? 0 : waves.Length; } }
	public float Countdown { get { return countdown; } }

	void Start ()
	{
		EnemiesAlive = 0;
		if (waves == null)
		{
			Debug.LogError("WaveSpawner has no wave list.");
			StopSpawning();
			return;
		}
		CurrentWave = waves.Length > 0 ? 1 : 0;
		if (spawnPoint == null)
		{
			Debug.LogError("WaveSpawner has no spawn point.");
			StopSpawning();
		}
	}

	void Update ()
	{
		if (isStopped || GameManager.GameIsOver)
			return;

		if (EnemiesAlive > 0)
		{
			return;
		}

		if (waveIndex >= waves.Length)
		{
			if (gameManager != null)
				gameManager.WinLevel();
			else
				StopSpawning();
			return;
		}

		if (countdown <= 0f)
		{
			StartCoroutine(SpawnWave());
			countdown = timeBetweenWaves;
			return;
		}

		countdown -= Time.deltaTime;

		countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);

	}

	public void StopSpawning ()
	{
		isStopped = true;
		StopAllCoroutines();
		enabled = false;
	}

	IEnumerator SpawnWave ()
	{
		Wave wave = waves[waveIndex];
		if (wave == null || wave.enemy == null || wave.count < 0)
		{
			Debug.LogError("WaveSpawner contains an invalid wave at index " + waveIndex + ".");
			waveIndex++;
			yield break;
		}

		PlayerStats.Rounds++;
		CurrentWave = waveIndex + 1;

		EnemiesAlive = wave.count;

		for (int i = 0; i < wave.count; i++)
		{
			SpawnEnemy(wave.enemy);
			yield return new WaitForSeconds(1f / Mathf.Max(wave.rate, 0.01f));
		}

		waveIndex++;
	}

	void SpawnEnemy (GameObject enemy)
	{
		Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
	}

}
