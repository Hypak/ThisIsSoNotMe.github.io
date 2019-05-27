using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature {

	public float xPos;
	public float yPos;
	public float xVel;
	public float yVel;
	public float oldXVel;
	public float oldYVel;

	public float mass;
	public float radius;

	public float boost;
	public float dir;
	public float turn;

	public float bounciness;

	public Creature (float xPos, float yPos) {
		this.xPos = xPos;
		this.yPos = yPos;
		xVel = 0;
		yVel = 0;
		oldXVel = 0;
		oldYVel = 0;
		mass = 5;
		radius = 4;

		dir = 0;
		boost = 0.2f;
		turn = 0.1f;

		bounciness = 0.2f;

	}
	public void Update (List<Planet> planets) {

		foreach (Planet p in planets) {
			AttractTo (p);
		}

		float xForce = xVel - oldXVel;
		float yForce = yVel - oldYVel;

		xVel += boost * Mathf.Sin (dir);
		yVel += boost * Mathf.Cos (dir);
		dir += turn * Random.Range (-1f, 1f);

		xPos += (xVel + oldXVel) / 2;
		yPos += (yVel + oldYVel) / 2;
		CheckColl (planets);

		oldXVel = xVel;
		oldYVel = yVel;
	}
	public void AttractTo (Planet planet) {
		float xDiff = planet.xPos - xPos;
		float yDiff = planet.yPos - yPos;

		float squareDist = xDiff * xDiff + yDiff * yDiff;
		float accel = planet.mass / squareDist;
		float dir = Mathf.Atan2 (xDiff, yDiff);
		xVel += accel * Mathf.Sin (dir);
		yVel += accel * Mathf.Cos (dir);
	}
	public void CheckColl (List<Planet> planets) {
		foreach (Planet p in planets) {
			float xDiff = p.xPos - xPos;
			float yDiff = p.yPos - yPos;
			float dist = Mathf.Sqrt (xDiff * xDiff + yDiff * yDiff);
			float minDist = radius + p.radius;
			if (dist < minDist) {
				float dir = Mathf.Atan2 (xDiff, yDiff);
				xPos = p.xPos - minDist * Mathf.Sin (dir);
				yPos = p.yPos - minDist * Mathf.Cos (dir);

				//Direction that the part of planet being collided with is facing
				float planeDir = dir + Mathf.PI / 2;
				float playerDir = Mathf.Atan2 (xVel, yVel);
				playerDir -= planeDir;
				float magnitude = Mathf.Cos (playerDir) * Mathf.Sqrt (xVel * xVel + yVel * yVel);
				float bounceMag = Mathf.Sin (playerDir) * Mathf.Sqrt (xVel * xVel + yVel * yVel) * -bounciness;
				xVel = magnitude * Mathf.Sin (planeDir) + bounceMag * Mathf.Sin (planeDir + Mathf.PI / 2);
				yVel = magnitude * Mathf.Cos (planeDir) + bounceMag * Mathf.Cos (planeDir + Mathf.PI / 2);

			}
		}
	}
}
