using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerPointer : MonoBehaviour
{
	public GameObject BeamPrefab;
	private GameObject beam;

	private SteamVR_Controller.Device device => SteamVR_Controller.Input((int)GetComponent<SteamVR_TrackedObject>().index);

	private GameObject tracker;

	void Start()
	{
		beam = Instantiate(BeamPrefab);
		beam.transform.SetParent(transform, false);

		tracker = new GameObject("Tracker");
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

		if (pointedShip != null && device.GetHairTriggerDown())
		{
			pickedUpShip = pointedShip;
			pickedUpShip.ShowMovementRange();
			tracker.transform.position = hit.transform.position;
			tracker.transform.eulerAngles = hit.transform.eulerAngles;
		}
	}

	Vector2 prevAxis = Vector2.zero;
	private void HandlePickedUpShip()
	{
		beam.transform.localScale = new Vector3(0, 0, 0);

		if (device.GetHairTriggerUp())
		{
			pickedUpShip.HideMovementRange();
			pickedUpShip = null;
		}
		else
		{
			if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Touchpad))
			{
				prevAxis = device.GetAxis();
			}

			if (device.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
			{
				var axis = device.GetAxis();
				var axisChange = axis - prevAxis;
				prevAxis = axis;
				tracker.transform.position += transform.forward * axisChange.y * .5f;
			}
			

			tracker.transform.position = pickedUpShip.ConstrainDesiredPos(tracker.transform.position);

			pickedUpShip.SetDesiredPos(tracker.transform.position, tracker.transform.eulerAngles);

			
		}
	}

}
