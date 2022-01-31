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

    const float G = 66.74f; //Grav Constant

    public Rigidbody rb;

    //Below is the optimisiation changes
    public static List<Attractor> Attractors;

    void OnEnable ()
    {
        if (Attractors == null)
            Attractors = new List<Attractor>();
        Attractors.Add(this);
    }

    void OnDisable ()
    {
        Attractors.Remove(this);
    }

    private void FixedUpdate()
    {
        //Attractor[] attractors = FindObjectsOfType<Attractor>();
        // The above code line is removed when adding OnEnable/Disable 
        foreach (Attractor attractor in Attractors)
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

        if (distance == 0f)
            return; //Avoids error when a duplicate body occupies same space

        float forceMagnitude = G * (rb.mass * rbToAttract.mass) / Mathf.Pow(distance, 2); //Magnitude of Force
        Vector3 force = direction.normalized * forceMagnitude; //apply newton force in direction of object

        rbToAttract.AddForce(force);
    }
}
