using UnityEngine;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour {

	private Text displayText;
	private WaveSpawner waveSpawner;

	void Awake ()
	{
		displayText = GetComponent<Text>();
		waveSpawner = FindObjectOfType<WaveSpawner>();
	}

	void Update ()
	{
		if (displayText == null || GameManager.GameIsOver)
			return;

		int wave = waveSpawner == null ? 0 : waveSpawner.CurrentWave;
		int waveCount = waveSpawner == null ? 0 : waveSpawner.WaveCount;
		string activity = WaveSpawner.EnemiesAlive > 0
			? "Enemies remaining: " + WaveSpawner.EnemiesAlive
			: "Next wave in: " + (waveSpawner == null ? 0f : waveSpawner.Countdown).ToString("0.0") + "s";

		displayText.text = string.Format(
			"MONEY  ${0}\nLIVES  {1}\nWAVE  {2}/{3}\n{4}\nClick an open node to place Player Turret 1  ($100)",
			PlayerStats.Money,
			PlayerStats.Lives,
			wave,
			waveCount,
			activity);
	}

}