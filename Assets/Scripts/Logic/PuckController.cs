using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuckController : MonoBehaviour {

	public float force = 300;
	public float maxStretch = 5.0f;

	private LineRenderer projectileLine;
	private Rigidbody2D rigidBody2D;
	private Ray rayToMouse;
	private bool clickedOn;
	private bool canClick;
	private float maxStretchSqr;
	private Vector3 currentDrag;

	private void Start() {
		LineRendererSetup();
		rigidBody2D = GetComponent<Rigidbody2D>();
		maxStretchSqr = maxStretch * maxStretch;
		rayToMouse = new Ray(this.transform.position, Vector3.zero);

		canClick = true;
	}

	private void Update() {
		if (clickedOn) {
			Dragging();
			LineRendererUpdate();
		}
	}

	private void OnMouseDown() {
		if (canClick) {
			clickedOn = true;
			projectileLine.enabled = true;
		}
	}

	private void OnMouseUp() {
		// Vector3.back check to insure that mouseDown has been called before mouseUp.
		if (canClick && currentDrag != Vector3.back) {
			clickedOn = false;
			projectileLine.enabled = false;

			rigidBody2D.velocity = -(currentDrag - transform.position) * Time.deltaTime * force;
			currentDrag = Vector3.back;
			StartCoroutine(clickCooldown());
		}
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.collider.tag == "Block") {
			Destroy(collision.collider.gameObject);
		}
	}

	private void Dragging() {
		rayToMouse.origin = transform.position;

		Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mouseWorldPoint.z = 0f;
		Vector2 puckToMouse = mouseWorldPoint - transform.position;

		if (puckToMouse.sqrMagnitude > maxStretchSqr) {
			rayToMouse.direction = puckToMouse;
			mouseWorldPoint = rayToMouse.GetPoint(maxStretch);
		}

		currentDrag = mouseWorldPoint;
	}

	IEnumerator clickCooldown() {
		canClick = false;
		yield return new WaitForSeconds(3.0f);
		canClick = true;
	}

	private void LineRendererSetup() {
		projectileLine = GetComponent<LineRenderer>();
		projectileLine.SetPosition(0, transform.position);
		projectileLine.enabled = false;
	}

	private void LineRendererUpdate() {
		projectileLine.SetPosition(0, transform.position);
		projectileLine.SetPosition(1, currentDrag);
	}

}
