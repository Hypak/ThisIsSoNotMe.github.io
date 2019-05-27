using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {

	public float xPos;
	public float yPos;
	public float xVel;
	public float yVel;
	public float oldXVel;
	public float oldYVel;

	public float mass;
	public float radius;

	public float boost;

	public int visitedCount;
	public int coinCount;

	public bool isColliding = false;
	public float autoZoom = 10000;

	public string readoutText = "";
	public float readoutAlpha = 0;

	public string statusText = "";
	//Speed required for status to be 'hit'
	float hitSpeed = 30;

	public Planet recentPlanet;
	float oldRecentDist = 0;
	bool onGround = false;
	bool leftPlanet = true;

	public Player (float xPos, float yPos) {
		this.xPos = xPos;
		this.yPos = yPos;
		xVel = 0;
		yVel = 0;
		oldXVel = 0;
		oldYVel = 0;
		mass = 5;
		radius = 4;

		boost = 0.3f;
		visitedCount = 0;
		coinCount = 0;
		recentPlanet = new Planet (0, 0, 0, 0, new Cluster (0, 0), new Cluster (0, 0));
	}
	public void Update (List<Planet> planets, List<Coin> coins, bool force, float forceDir, float tickCount) {

		foreach (Planet p in planets) {
			AttractTo (p, tickCount);
		}

		float xForce = xVel - oldXVel;
		float yForce = yVel - oldYVel;
		float dir = Mathf.Atan2 (xForce, yForce);

		if (force) {
			xVel += boost * Mathf.Sin (forceDir) * tickCount;
			yVel += boost * Mathf.Cos (forceDir) * tickCount;
		}

		xPos += tickCount * (xVel + oldXVel) / 2;
		yPos += tickCount * (yVel + oldYVel) / 2;

		CheckColl (planets);

		float xDiff = xPos - recentPlanet.xPos;
		float yDiff = yPos - recentPlanet.yPos;
		float dist = Mathf.Sqrt (xDiff * xDiff + yDiff * yDiff);
		if (dist < recentPlanet.radius * 2.5f) {
			if (dist > recentPlanet.radius * 1.5f) {
				if (dist > oldRecentDist) {
					statusText = "Leaving " + recentPlanet.name;
				} else {
					statusText = "Approaching " + recentPlanet.name;
				}
			}
		} else {
			statusText = "In Space";
			leftPlanet = true;
		}
		oldRecentDist = dist;

		oldXVel = xVel;
		oldYVel = yVel;
		CheckCoins (coins);
	}
	public void AttractTo (Planet planet, float tickCount) {
		float xDiff = planet.xPos - xPos;
		float yDiff = planet.yPos - yPos;

		float squareDist = xDiff * xDiff + yDiff * yDiff;
		float accel = planet.mass / squareDist;
		float dir = Mathf.Atan2 (xDiff, yDiff);
		xVel += accel * Mathf.Sin (dir) * tickCount;
		yVel += accel * Mathf.Cos (dir) * tickCount;
	}
	public void CheckColl (List<Planet> planets) {
		isColliding = false;
		float closest = float.MaxValue;
		float closestFarEdge = float.MaxValue;
		foreach (Planet p in planets) {
			float xDiff = p.xPos - xPos;
			float yDiff = p.yPos - yPos;
			float dist = Mathf.Sqrt (xDiff * xDiff + yDiff * yDiff);
			float minDist = radius + p.radius;
			if (dist < minDist) {
				recentPlanet = p;

				readoutText = p.name;

				if (leftPlanet || p != recentPlanet) {
					readoutAlpha = 1;
					leftPlanet = false;
				}


				if (!p.visited) {
					visitedCount++;
					p.SetVisited ();
				}

				float dir = Mathf.Atan2 (xDiff, yDiff);
				xPos = p.xPos - minDist * Mathf.Sin (dir);
				yPos = p.yPos - minDist * Mathf.Cos (dir);

				//Direction that the part of planet being collided with is facing
				float planeDir = dir + Mathf.PI / 2;
				float playerDir = Mathf.Atan2 (xVel, yVel);
				playerDir -= planeDir;
				float magnitude = Mathf.Cos (playerDir) * Mathf.Sqrt (xVel * xVel + yVel * yVel);
				float bounceMag = Mathf.Sin (playerDir) * Mathf.Sqrt (xVel * xVel + yVel * yVel) * -0.2f;
				xVel = magnitude * Mathf.Sin (planeDir) + bounceMag * Mathf.Sin (planeDir + Mathf.PI / 2);
				yVel = magnitude * Mathf.Cos (planeDir) + bounceMag * Mathf.Cos (planeDir + Mathf.PI / 2);

				if (onGround || magnitude + bounceMag < hitSpeed) {
					statusText = "Landed on " + p.name;
				} else {
					statusText = "Hit " + p.name;
					isColliding = true;
					Debug.Log ("Colliding " + Time.time);
				}

				onGround = true;
				return;
			} else {
				if (dist - minDist < closest) {
					closest = dist - minDist;
				}
			}
			if (dist + p.radius < closestFarEdge) {
				closestFarEdge = dist + p.radius;
			}
		}
		if (closest > 30) {
			onGround = false;
			statusText = "Orbiting " + recentPlanet.name;

		}
		autoZoom = closestFarEdge;
	}
	public void CheckCoins (List<Coin> coins) {
		for (int i = 0; i < coins.Count; i++) {
			float squareDist = (coins[i].xPos - xPos) * (coins[i].xPos - xPos) + (coins[i].yPos - yPos) * (coins[i].yPos - yPos);
			if (squareDist < 6400) {
				coinCount++;
				coins.RemoveAt (i);
			}
		}
	}

	public float GetSpeed () {
		return Mathf.Sqrt (xVel * xVel + yVel * yVel) * 45;
	}
	public float GetDir () {
		float dir = Mathf.Atan2 (xVel, yVel) * Mathf.Rad2Deg;
		if (dir < 0) {
			return dir + 360;
		}
		return dir;
	}
}
