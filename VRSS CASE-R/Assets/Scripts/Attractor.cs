using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 144;
    }

    /* Update is called once per frame
    void Update()
    {
        
    }
    */

    // Using https://www.youtube.com/watch?v=Ouu3D_VHx9o , Optimisations @7:08

    const float G = 6.674f; //Grav Constant

    public Rigidbody rb;

    private void FixedUpdate()
    {
        Attractor[] attractors = FindObjectsOfType<Attractor>();
        foreach (Attractor attractor in attractors)
        {
            if (attractor != this)
                Attract(attractor);
        }
    }
    void Attract (Attractor objToAttract)
    {
        Rigidbody rbToAttract = objToAttract.rb; //Stores mass into variable

        Vector3 direction = rb.position - rbToAttract.position; // Direction of attraction
        float distance = direction.magnitude; //Sets distance float (length of direction vector)

        float forceMagnitude = G * (rb.mass * rbToAttract.mass) / Mathf.Pow(distance, 2); //Magnitude of Force
        Vector3 force = direction.normalized * forceMagnitude; //apply newton force in direction of object

        rbToAttract.AddForce(force);
    }
}
