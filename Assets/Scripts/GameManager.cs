using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static bool GameIsOver;

	public Text statusText;
	private WaveSpawner waveSpawner;

	void Awake ()
	{
		GameIsOver = false;
		waveSpawner = GetComponent<WaveSpawner>();
		if (statusText != null)
			statusText.text = string.Empty;
	}

	// Update is called once per frame
	void Update () {
		if (GameIsOver)
			return;

		if (PlayerStats.Lives <= 0)
		{
			FinishGame("GAME OVER");
		}
	}

	public void WinLevel ()
	{
		FinishGame("VICTORY");
	}

	void FinishGame (string message)
	{
		if (GameIsOver)
			return;

		GameIsOver = true;
		if (statusText != null)
			statusText.text = message;
		if (waveSpawner != null)
			waveSpawner.StopSpawning();
	}

}
