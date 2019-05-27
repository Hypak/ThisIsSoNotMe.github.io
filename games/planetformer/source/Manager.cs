using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {
	public Render render;
	public UnityEngine.UI.Text planetDisplay;
	//public UnityEngine.UI.Text coinDisplay;
	public UnityEngine.UI.Text speedDisplay;
	public UnityEngine.UI.Text dirDisplay;

	public UnityEngine.UI.Text readout;
	public UnityEngine.UI.Text statusText;

	public float alphaDecayRate = 0.4f;

	public int planetCount = 5000;
	public int creatureChance = 40;

	public bool autoThrust = true;

	Player player;
	List<Creature> creatures;
	List<Planet> planets;
	List<Coin> coins;

	List<Cluster> clusters;
	List<Cluster> sClusters;

	Cluster recentCluster = new Cluster (0, 0);
	Cluster recentSCluster = new Cluster (0, 0);

	public void Awake () {
		player = new Player (10350, 0);
		player.xVel = 0.5f;
		creatures = new List<Creature> ();


		planets = new List<Planet> ();
		coins = new List<Coin> ();
		float r = 10000;


		All (planets, clusters, sClusters, 10, 800000, 800000);

		/*
		float width = 400000;
		float height = 400000;
		for (int x = -6000000; x <= 6000000; x += 2000000) {
			for (int y = -6000000; y <= 6000000; y += 2000000) {
				float sizeScale = Random.Range (1f, 1.5f) * (Mathf.Abs (x) + Mathf.Abs(y) + 6000000) / 6000000;
				float density = Random.Range (128000, 384000);
				float size = Mathf.Pow (density * sizeScale, 0.6f);
				int count = (int)Mathf.Round (size / sizeScale / 3);
				planets.AddRange (GetPlanets (count, width, height, x, y, size));

			}
		}
		*/

		/*
		for (int i = 0; i < planetCount;) {
			float x = Random.Range (-width / 2, width / 2);
			float y = Random.Range (-height / 2, height / 2);
			float radius = 800 * Mathf.Pow (Random.Range (0f, 1f), -0.5f);

			bool close = false;

			foreach (Planet planet in planets) {
				float xDiff = x - planet.xPos;
				float yDiff = y - planet.yPos;
	
				float squareDist = xDiff * xDiff + yDiff * yDiff;
				float minDist = 1.5f * (radius + planet.radius);
				if (squareDist > minDist * minDist) {
					
				} else {
					close = true;
					width += 1000;
					height += 1000;
					break;

				}
			}
			if (!close) {
				i++;
				planets.Add (new Planet (
					x, y, radius, radius * radius * Random.Range (1.6f, 2.4f)
				));
			}

			
		}
		*/

		//GenerateCoins (500000);
	}

	void Update () {
		float mouseDir = Mathf.Atan2 (Input.mousePosition.x - Screen.width / 2, Input.mousePosition.y - Screen.height / 2);
		bool thrust;
		if (autoThrust) {
			thrust = true;
		} else {
			thrust = Input.GetMouseButton (0);
		}
		player.Update (planets, coins, thrust, mouseDir, Time.deltaTime * 45);

		foreach (Creature creature in creatures) {
			//creature.Update (planets);
		}
		render.Rend (player, creatures, planets, coins);
		planetDisplay.text = 
			"Planets Visited:\nTotal: " + player.visitedCount + " / " + planets.Count 
			+ "\nSupercluster: " + player.recentPlanet.sCluster.visitedCount + " / " + player.recentPlanet.sCluster.size
			+ "\nCluster: " + player.recentPlanet.cluster.visitedCount + " / " + player.recentPlanet.cluster.size;
		
		//coinDisplay.text = "Coins: " + player.coinCount;
		speedDisplay.text = "Speed: " + 100 * Mathf.Round (player.GetSpeed () / 100f) + "/s";
		dirDisplay.text = "Direction: " + 5 * Mathf.Round (player.GetDir () / 5f) + "º";

		readout.text = player.readoutText;
		readout.color = new Color (readout.color.r, readout.color.g, readout.color.b, player.readoutAlpha);
		player.readoutAlpha -= alphaDecayRate * Time.deltaTime;

		statusText.text = player.statusText;
	}
	void All (List<Planet> planets, List<Cluster> clusters, List<Cluster> sClusters, int count, float width, float height) {
		float clusterDist = 15000;
		float sClusterDist = 75000;

		sClusters = new List<Cluster> ();
		clusters = new List<Cluster> ();
		for (int i = 0; i < count; i++) {
			sClusters.Add (new Cluster ());
			float superClusterX = Random.Range (-width, width);
			float superClusterY = Random.Range (-height, height);
			int sum = 0;
			for (int j = 0; j < count; j++) {
				clusters.Add (new Cluster ());
				float clusterX = Random.Range (superClusterX - 100000, superClusterX + 100000);
				float clusterY = Random.Range (superClusterY - 100000, superClusterY + 100000);
				int k = 0;
				for (; k < count; k++) {
					if (!GeneratePlanet (planets, clusterX, clusterY, 20000,sClusters [sClusters.Count - 1], clusters [clusters.Count - 1], 800)) {
						break;
					}
				}
				sum += k;

				clusters [clusters.Count - 1].size = k;
				clusters [clusters.Count - 1].visitedCount = 0;
			}
			sClusters [sClusters.Count - 1].size = sum;
			sClusters [sClusters.Count - 1].visitedCount = 0;
		}
	/*
		List<Planet> outCluster = new List<Planet> ();
		foreach (Planet planet in planets) {
			outCluster.Add (planet);
		}

		List<Planet> inCluster = new List<Planet> ();
		Planet rPlanet = outCluster [Random.Range (0, outCluster.Count)];
		inCluster.Add (rPlanet);
		outCluster.Remove (rPlanet);

		for (;;) {
			bool found1 = false;
			foreach (Planet a in inCluster) {
				bool found2 = false;
				foreach (Planet b in outCluster) {
					if (Planet.GetDist (a, b) < clusterDist) {
						inCluster.Add (b);
						outCluster.Remove (b);
						found2 = true;
						break;
					}
				}
				if (found2) {
					found1 = true;
					break;
				}
			}
			if (!found1) {
				break;
			}
		}
		*/

	}
	/*
	void AddSuperClusters (List<Planet> planets, List<Cluster> clusters, List<Cluster> sClusters, int count, float width, float height) {
		sClusters = new List<Cluster> ();
		for (int i = 0; i < count; i++) {
			Cluster sCluster;
			float superClusterX = Random.Range (-width, width);
			float superClusterY = Random.Range (-height, height);
			AddClusters (planets, clusters, sCluster, 10, 270000, 270000, superClusterX, superClusterY);
		}
	}
	void AddClusters (List<Planet> planets, List<Cluster> clusters, Cluster sCluster, int count, float width, float height, float x, float y) {

		for (int i = 0; i < count; i++) {
			float clusterX = Random.Range (x - width, x + width);
			float clusterY = Random.Range (y - height, y + height);
			AddPlanets (planets, clusters, sClusters, 10, 30000, 30000, clusterX, clusterY);
		}
	}

	void AddPlanets (List<Planet> planets, int count, float width, float height, float x, float y) {
		for (;;) {
			if (GeneratePlanet (planets, x, y, (width + height) / 2, 800)) {
				count--;
				if (count <= 0) {
					break;
				}
			} else {
				break;
			}
		}
	}
*/
	/*
	List<Planet> GetPlanets (int count, float width, float height, float centreX, float centreY, float minSize) {
		List<Planet> planets = new List<Planet> ();

		for (int timeout = 0; timeout < 100000 && count > 0; timeout++) {
			float clusterX = Random.Range (centreX - width, centreX + width);
			float clusterY = Random.Range (centreY - height, centreY + height);
			float radius = Random.Range (10000, 20000);
			//Keeps adding to cluster until overlap
			if (GeneratePlanet (planets, clusterX, clusterY, radius, minSize * 2.5f)) {
				count--;
				for (;;) {
					if (GeneratePlanet (planets, clusterX, clusterY, radius, minSize)) {
						count--;
						if (count <= 0) {
							break;
						}
					} else {
						break;
					}
				}
			}
		}

		return planets;
	}
	*/

	bool GeneratePlanet (List<Planet> planets, float centreX, float centreY, float range, Cluster sCluster, Cluster cluster, float minSize = 600) {
		float x = Random.Range (centreX - range, centreX + range);
		float y = Random.Range (centreY - range, centreY + range);
		float r = minSize * Mathf.Pow (Random.Range (0f, 1f), -0.7f);

		foreach (Planet planet in planets) {
			float xDiff = x - planet.xPos;
			float yDiff = y - planet.yPos;

			float squareDist = xDiff * xDiff + yDiff * yDiff;
			float minDist = 1.5f * (r + planet.radius);
			if (squareDist <= minDist * minDist) {
				return false;
			}
		}

		planets.Add (new Planet (
			x, y, r, r * r * 2, cluster, sCluster, NameGenerator.GenerateName (5, 8)
		));
		return true;
	}

	void GenerateCoins (int count) {
		foreach (Planet planet in planets) {
			int amount = (int) (Mathf.Sqrt (planet.radius) * Random.Range (0.075f, 0.15f));
			float angle = Random.Range (0, Mathf.PI * 2);
			float inc = Mathf.PI * 2 / amount;
			float dist = planet.radius + 4;
			//Adds coins all around the planet
			for (int i = 0; i < amount; i++, angle += inc) {
				coins.Add (
					new Coin (
						planet.xPos + Mathf.Sin (angle) * dist,
						planet.yPos + Mathf.Cos (angle) * dist
					)
				);
				if (Random.Range (0, creatureChance) == 0) {
					creatures.Add (
						new Creature(
							planet.xPos + Mathf.Sin (angle) * dist,
							planet.yPos + Mathf.Cos (angle) * dist
						)
					);
				}
			}

			count -= amount;
			if (count <= 0) {
				//Stops if generated enough coins
				return;
			}
		}
	}
}
