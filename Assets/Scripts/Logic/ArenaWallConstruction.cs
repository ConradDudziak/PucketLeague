using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaWallConstruction : MonoBehaviour {

	public float distanceBetweenBlocks;
	public float boundsOffsetFromWall;
	public int blockCountPerWall;
	public GameObject blockPrefab;
	public GameObject upperWall;
	public GameObject lowerWall;
	public GameObject rightWall;
	public GameObject leftWall;
	public BoxCollider2D bounds;

	private float _midPoint;
	// Use this for initialization
	void Start () {
		_midPoint = distanceBetweenBlocks * blockCountPerWall / 2;

		SetBounds();
		SetUpperWall();
		SetLowerWall();
		SetRightWall();
		SetLeftWall();
	}

	private void SetBounds() {
		bounds.size = new Vector2(_midPoint * 2 + boundsOffsetFromWall, _midPoint * 2 + boundsOffsetFromWall);
	}

	private void SetUpperWall() {
		for (int i = 0; i < blockCountPerWall; i++) {
			GameObject block = Instantiate(blockPrefab, transform.position, Quaternion.identity) as GameObject;
			block.transform.SetParent(upperWall.transform);
		}
		SetBlockPositions(-_midPoint + distanceBetweenBlocks, _midPoint, distanceBetweenBlocks, 0, upperWall); // _midpoint + offset to fix overlap.
	}

	private void SetLowerWall() {
		for (int i = 0; i < blockCountPerWall; i++) {
			GameObject block = Instantiate(blockPrefab, transform.position, Quaternion.identity) as GameObject;
			block.transform.SetParent(lowerWall.transform);
		}
		SetBlockPositions(-_midPoint, -_midPoint, distanceBetweenBlocks, 0, lowerWall);
	}

	private void SetRightWall() {
		for (int i = 0; i < blockCountPerWall; i++) {
			GameObject block = Instantiate(blockPrefab, transform.position, Quaternion.identity) as GameObject;
			block.transform.SetParent(rightWall.transform);
		}
		SetBlockPositions(_midPoint, _midPoint - distanceBetweenBlocks, 0, -distanceBetweenBlocks, rightWall); // _midpoint - offset to fix overlap.
	}

	private void SetLeftWall() {
		for (int i = 0; i < blockCountPerWall; i++) {
			GameObject block = Instantiate(blockPrefab, transform.position, Quaternion.identity) as GameObject;
			block.transform.SetParent(leftWall.transform);
		}
		SetBlockPositions(-_midPoint, _midPoint, 0, -distanceBetweenBlocks, leftWall);
	}

	private void SetBlockPositions(float startX, float startY, float xOffset, float yOffset, GameObject wall) {
		Transform[] children = wall.GetComponentsInChildren<Transform>();
		children[0].transform.position = new Vector3(startX, startY, 0);
		Vector3 currentElementPos = children[0].transform.position;

		for (int i = 1; i < children.Length; i++) {
			children[i].transform.position = currentElementPos;
			currentElementPos += new Vector3(xOffset, yOffset, 0);
		}
	}
}
