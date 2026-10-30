using UnityEngine;

public class Node : MonoBehaviour {

	public GameObject turretPrefab;
	public GameObject bulletPrefab;
	public Material turretMaterial;
	public int cost = 100;
	public Vector3 positionOffset = new Vector3(0f, 0.5f, 0f);

	private GameObject turret;

	public bool TryPlace (out string message)
	{
		if (GameManager.GameIsOver)
			message = "The game has ended.";
		else if (turret != null)
			message = "That node is already occupied.";
		else if (turretPrefab == null || bulletPrefab == null || cost < 0)
			message = "Turret placement is not configured on this node.";
		else if (PlayerStats.Money < cost)
			message = "Not enough money. Player Turret 1 costs $" + cost + ".";
		else
			message = null;

		if (message != null)
			return false;

		GameObject placedTurret = Instantiate(turretPrefab, transform.position + positionOffset, Quaternion.identity);
		if (turretMaterial != null)
		{
			foreach (Renderer renderer in placedTurret.GetComponentsInChildren<Renderer>(true))
				renderer.sharedMaterial = turretMaterial;
		}

		Turret turretBehavior = placedTurret.GetComponent<Turret>();
		if (turretBehavior == null)
			turretBehavior = placedTurret.AddComponent<Turret>();
		if (!turretBehavior.Configure(bulletPrefab))
		{
			Destroy(placedTurret);
			message = "The turret could not be configured.";
			return false;
		}

		placedTurret.name = "Player Turret 1";
		turret = placedTurret;
		PlayerStats.Money -= cost;
		message = null;
		return true;
	}

}