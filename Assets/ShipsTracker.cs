using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipsTracker : MonoBehaviour
{
	public List<ShipController> Ships = new List<ShipController>();

	public static bool Playing;
	private bool wasPlaying = false;
	private float playStartTime = 0;
	private const float playTime = 5f;
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (Playing)
		{
			if (!wasPlaying)
				playStartTime = Time.time;
			if (Time.time > playStartTime + playTime)
				Playing = false;
			else PlayScene((Time.time - playStartTime) / playTime);
		}
		wasPlaying = Playing;
	}

	private void PlayScene(float percentage)
	{
		Debug.Log($"Playing {percentage}");
		foreach (ShipController ship in Ships)
		{
			ship.MoveShip(percentage);
		}
	}
}
