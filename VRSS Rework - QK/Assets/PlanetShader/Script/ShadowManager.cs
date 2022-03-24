using UnityEngine;
using System.Collections;

public class ShadowManager : MonoBehaviour {

	public GameObject[] planet;

	void Start ()
	{
		planet = GameObject.FindGameObjectsWithTag("Celestial");
		int		i = -1;
		if (planet.Length > 10 || planet.Length <= 0) // Our shadowing system can only compute the shadow of 10 object for each object
			return;
		while (++i < planet.Length)  // looping trough our solar system
		{
			if (planet[i].GetComponent<Planet>())
			{
				int		x = -1;
				int		x2 = -1;
				planet[i].GetComponent<Planet>().setShadowNumber(planet.Length - 1); // first we need to set the number of object we want ( - 1 because we wont give own coordinates)
				while (++x < planet.Length) // giving each planet the coordinates of all the other planet
				{
					if (x != i) // we don't pass own shadow to the object
					{
						x2++;
						planet[i].GetComponent<Planet>().setShadow(planet[x].transform, x2);
					}
				}
			}
		}
	}
}
