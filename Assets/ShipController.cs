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
		movementRangeDisplay.SetActive(false);
	}

	public void ShowMovementRange()
	{
		DesiredPosTracker.SetActive(true);
		movementRangeDisplay.SetActive(true);
		movementRangeDisplay.transform.position = transform.position;
		movementRangeDisplay.transform.localScale = new Vector3(maxXZDist, maxYDist, maxXZDist) * 2f;
	}

	public void HideMovementRange()
	{
		movementRangeDisplay.SetActive(false);
	}

	public void SetDesiredPos(Vector3 pos, Vector3 rot)
	{
		DesiredPosTracker.transform.eulerAngles = rot;

		

		DesiredPosTracker.transform.position = pos;
	}

	public Vector3 ConstrainDesiredPos(Vector3 pos)
	{
		Vector3 xzVec = pos - transform.position;
		xzVec.y = 0;
		Ray xzRayToDesiredPos = new Ray(transform.position, xzVec);
		Vector3 xzPos = xzRayToDesiredPos.GetPoint(Mathf.Min(maxXZDist, xzVec.magnitude));

		Vector3 yVec = pos - transform.position;
		yVec.x = 0;
		yVec.z = 0;
		Ray yRayToDesiredPos = new Ray(transform.position, yVec);
		Vector3 yPos = yRayToDesiredPos.GetPoint(Mathf.Min(maxYDist, yVec.magnitude));

		return new Vector3(xzPos.x, yPos.y, xzPos.z);
	}
}
