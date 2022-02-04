using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarSystem : MonoBehaviour
{

    public float G = 10f;
    GameObject[] celestials;

    void InitialVelocity()
    {
        foreach (GameObject celestial1 in celestials)
        {
            foreach (GameObject celestial2 in celestials)
            {
                if (!celestial1.Equals(celestial2))
                {
                    float m2 = celestial2.GetComponent<Rigidbody>().mass;
                    float r = Vector3.Distance(celestial1.transform.position, celestial2.transform.position);
                    celestial1.transform.LookAt(celestial2.transform);

                    celestial1.GetComponent<Rigidbody>().velocity += celestial1.transform.right * Mathf.Sqrt((G * m2) / r);
                }

            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        celestials = GameObject.FindGameObjectsWithTag("Celestial");
        InitialVelocity();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Gravity()
    {
        foreach (GameObject celestial1 in celestials)
        {
            foreach (GameObject celestial2 in celestials)
            {
                if (!celestial1.Equals(celestial2))
                {
                    float m1 = celestial1.GetComponent<Rigidbody>().mass;
                    float m2 = celestial2.GetComponent<Rigidbody>().mass;

                    float r = Vector3.Distance(celestial1.transform.position, celestial2.transform.position);

                    celestial1.GetComponent<Rigidbody>().AddForce((celestial2.transform.position - celestial1.transform.position).normalized * (G*m1*m2/(r*r)));
                }

            }
        }
    }

    void FixedUpdate()
    {
        Gravity();
    }

}
