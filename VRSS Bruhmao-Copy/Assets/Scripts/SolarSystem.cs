using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarSystem : MonoBehaviour
{
    [SerializeField] public int frameRate = 60;
    [SerializeField] public float initialTimeScale = 1;

    public float G = 2.959233859351670E04f;
    public GameObject[] celestials;

    //Originall calculated on initial startup, changing G mid
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
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = frameRate;

    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = initialTimeScale;
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
