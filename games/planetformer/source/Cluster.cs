using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cluster : MonoBehaviour {

	public int size;
	public int visitedCount;

	public float width;
	public float height;
	public float xPos;
	public float yPos;

	public Cluster (int size, int visitedCount/*, float width, float height, float xPos, float yPos*/) {
		this.size = size;
		this.visitedCount = visitedCount;

		/*
		this.width = width;
		this.height = height;
		this.xPos = xPos;
		this.yPos = yPos;
		*/
	}
	public Cluster () {
		
	}

}
