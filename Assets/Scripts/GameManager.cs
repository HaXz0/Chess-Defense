using UnityEngine;
using UnityEngine.EventSystems;
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
			return;
		}

		TryPlaceTurretAtPointer();
	}

	void TryPlaceTurretAtPointer ()
	{
		if (!Input.GetMouseButtonDown(0) || EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
			return;

		Camera gameCamera = Camera.main;
		if (gameCamera == null)
			gameCamera = FindObjectOfType<Camera>();
		if (gameCamera == null)
			return;

		RaycastHit[] hits = Physics.RaycastAll(gameCamera.ScreenPointToRay(Input.mousePosition));
		Node closestNode = null;
		float closestDistance = Mathf.Infinity;
		foreach (RaycastHit hit in hits)
		{
			Node node = hit.collider.GetComponentInParent<Node>();
			if (node != null && hit.distance < closestDistance)
			{
				closestNode = node;
				closestDistance = hit.distance;
			}
		}

		if (closestNode != null)
		{
			string message;
			if (closestNode.TryPlace(out message))
				Debug.Log("Player Turret 1 placed for $100.");
			else if (!string.IsNullOrEmpty(message))
				Debug.Log(message);
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
