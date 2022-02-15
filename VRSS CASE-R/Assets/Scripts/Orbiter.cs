using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbiter : MonoBehaviour
{
    public int sphereCount = 500;
    public int maxRadius = 200;
    public GameObject[] spheres;
    public Material[] mats;
    public Material trailMat;

    private void Awake()
    {
        spheres = new GameObject[sphereCount];
    }
    // Start is called before the first frame update
    void Start()
    {
        //spheres = CreateSpheres(int count, int radius);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        foreach (GameObject s in spheres)
        {
            Vector3 difference = this.transform.position - s.transform.position;

            float dist = difference.magnitude;
            Vector3 gravityDirection = difference.normalized;
            float gravity = 6.7f;
        }
    }
}
