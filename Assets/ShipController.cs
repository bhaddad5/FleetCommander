using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
	public float Speed = 0f;
	private float maxXZDist => Speed;
	private float maxYDist => Speed / 3f;

	public GameObject DesiredPosTracker;

	public GameObject MovementRangeDisplayPrefab;
	private GameObject movementRangeDisplay;

	void Start()
	{
		movementRangeDisplay = GameObject.Instantiate(MovementRangeDisplayPrefab);
		movementRangeDisplay.transform.SetParent(transform, false);
	}

	public void ShowMovementRange()
	{
		DesiredPosTracker.SetActive(true);
		movementRangeDisplay.SetActive(true);
		movementRangeDisplay.transform.localScale = new Vector3(maxXZDist, maxYDist, maxXZDist);
	}

	public void HideMovementRange()
	{
		movementRangeDisplay.SetActive(false);
	}

	public void SetDesiredPos(Vector3 pos, Vector3 rot)
	{
		DesiredPosTracker.transform.eulerAngles = rot;

		Vector3 vec = pos - transform.position;
		Ray rayToDesiredPos = new Ray(transform.position, vec);
		DesiredPosTracker.transform.position = rayToDesiredPos.GetPoint(Mathf.Min(Speed, vec.magnitude));
	}
}
