using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour {

	public GameObject turretPrefab;
	public GameObject bulletPrefab;
	public int cost = 100;
	public Vector3 positionOffset = new Vector3(0f, 0.5f, 0f);

	private GameObject turret;

	void OnMouseDown ()
	{
		if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
			return;
		if (GameManager.GameIsOver || turret != null || IsOnPath())
			return;
		if (turretPrefab == null || bulletPrefab == null || cost < 0 || PlayerStats.Money < cost)
			return;

		GameObject placedTurret = Instantiate(turretPrefab, transform.position + positionOffset, Quaternion.identity);
		Turret turretBehavior = placedTurret.GetComponent<Turret>();
		if (turretBehavior == null)
			turretBehavior = placedTurret.AddComponent<Turret>();
		if (!turretBehavior.Configure(bulletPrefab))
		{
			Destroy(placedTurret);
			return;
		}

		placedTurret.name = "Player Turret 1";
		turret = placedTurret;
		PlayerStats.Money -= cost;
	}

	bool IsOnPath ()
	{
		if (Waypoints.points == null)
			return false;

		foreach (Transform point in Waypoints.points)
		{
			if (point == null)
				continue;

			Vector3 delta = point.position - transform.position;
			if (Mathf.Abs(delta.x) < 1.9f && Mathf.Abs(delta.z) < 1.9f)
				return true;
		}

		return false;
	}

}