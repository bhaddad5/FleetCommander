using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerPointer : MonoBehaviour
{
	public GameObject BeamPrefab;
	private GameObject beam;

	private GameObject tracker;

	void Start()
	{
		beam = Instantiate(BeamPrefab);
		beam.transform.SetParent(transform, false);

		tracker.transform.SetParent(transform, false);
	}

	private ShipController pickedUpShip;

	// Update is called once per frame
	void Update ()
	{
		if(pickedUpShip == null)
			PointAtShips();
		else HandlePickedUpShip();
	}

	private void PointAtShips()
	{
		RaycastHit hit;
		Physics.Raycast(new Ray(transform.position, transform.forward), out hit);

		float dist = hit.distance;
		if (hit.transform == null)
			dist = 1000f;

		beam.transform.localScale = new Vector3(.01f, .01f, dist);

		ShipController pointedShip = hit.transform?.GetComponent<ShipController>();

		if (pointedShip != null && SteamVR_Controller.Input((int) GetComponent<SteamVR_TrackedObject>().index).GetHairTriggerDown())
		{
			pickedUpShip = pointedShip;
			pickedUpShip.ShowMovementRange();
			tracker.transform.position = hit.transform.position;
			tracker.transform.eulerAngles = hit.transform.eulerAngles;
		}
	}

	private void HandlePickedUpShip()
	{
		beam.transform.localScale = new Vector3(0, 0, 0);

		if (SteamVR_Controller.Input((int) GetComponent<SteamVR_TrackedObject>().index).GetHairTriggerUp())
		{
			Debug.Log("HIT!!!");
			pickedUpShip.HideMovementRange();
			pickedUpShip = null;
		}
		else
		{
			pickedUpShip.SetDesiredPos(tracker.transform.position, tracker.transform.eulerAngles);
		}
	}

}
