using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {    }
    // Using https://www.youtube.com/watch?v=Ouu3D_VHx9o , Optimisations @7:08

    [SerializeField]
    private float G = 6.67e-11f; //Grav Constant
    [SerializeField]
    private float orbitVelocity;
    //Set Semi-major axis components to massive objects
    private float semiMajor;
    private float semiMajorOne;
    private float semiMajorTwo;
    private float CoMass;

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

        if (rb.GetComponent<Rigidbody>())
            //Gives tangential velocity to direction of attraction referenced to y axis (flat orbits) Soruce: https://answers.unity.com/questions/1333667/perpendicular-to-a-3d-direction-vector.html
            CoMass = rb.mass + rbToAttract.mass;
            orbitVelocity = Mathf.Sqrt(G * CoMass / distance);

            rb.GetComponent<Rigidbody>().velocity = Vector3.Cross(direction, Vector3.up).normalized * orbitVelocity;
    }

}