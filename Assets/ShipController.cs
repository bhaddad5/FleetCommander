using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
	public float Speed = 0f;
	private float maxXZDist => Speed;
	private float maxYDist => Speed / 3f;

	public GameObject DesiredPosTrackerPrefab;
	private GameObject desiredPosTracker;

	public GameObject MovementRangeDisplayPrefab;
	private GameObject movementRangeDisplay;

	void Start()
	{
		movementRangeDisplay = GameObject.Instantiate(MovementRangeDisplayPrefab);
		movementRangeDisplay.SetActive(false);

		desiredPosTracker = GameObject.Instantiate(DesiredPosTrackerPrefab);
		desiredPosTracker.SetActive(false);
	}

	public void ShowMovementRange()
	{
		desiredPosTracker.SetActive(true);
		movementRangeDisplay.SetActive(true);
		movementRangeDisplay.transform.position = transform.position;
		movementRangeDisplay.transform.localScale = new Vector3(maxXZDist, maxYDist, maxXZDist) * 2f;
	}

	public void HideMovementRange()
	{
		movementRangeDisplay.SetActive(false);
	}

	private Vector3 startPos;
	private Vector3 startRot;
	public void MoveShip(float percentage)
	{
		if (desiredPosTracker.activeInHierarchy)
		{
			startPos = transform.position;
			startRot = transform.eulerAngles;
			desiredPosTracker.SetActive(false);
		}
		
		transform.position = Vector3.Lerp(startPos, desiredPosTracker.transform.position, percentage);
		transform.eulerAngles = Vector3.Lerp(startRot, desiredPosTracker.transform.eulerAngles, percentage);
	}

	public void SetDesiredPos(Vector3 pos, Vector3 rot)
	{
		desiredPosTracker.transform.eulerAngles = rot;
		desiredPosTracker.transform.position = pos;
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
