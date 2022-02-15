using System.Collections;
using System.Collections.Generic;
//using System.Math.Sqrt;
using UnityEngine;

public class SolarSystemAU : MonoBehaviour
{
    // Creates initial parameters
    [SerializeField] public int frameRate = 60;
    [Range(0.1f, 100f)]
    [SerializeField] public float initialTimeScale = 1;
    [Range(0.015f, 1f)]
    [SerializeField] public float initialFixedTimeStep = 0.02f;

    //Gives visual timers (NOT TO BE CHANGED IN EDITOR)
    [SerializeField] private float timeStart;
    [SerializeField] private float physTimeStart;
    /*[SerializeField] private float fixedDeltaUnscaledTime;
    [SerializeField] private float fixedScaledDeltaTime;
    [SerializeField] private float fixedScaledTime;
    */

    public float G = 2.959233859351670E-04f;
    public GameObject[] celestials;

    //Original calculated on initial startup, changing G mid
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

                    celestial1.GetComponent<Rigidbody>().velocity += celestial1.transform.right * Mathf.Sqrt(G * m2 / r);
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
        Time.fixedDeltaTime = initialFixedTimeStep;

        timeStart += Time.deltaTime;
        //physTimeStart += Time.fixedDeltaTime;
        /*
        Time.fixedUnscaledTime = fixedUnscaledDeltaTime;
        Time.fixedUnscaledTime = fixedScaledDeltaTime;
        [SerializeField] private float fixedScaledTime;
        */
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
        physTimeStart += Time.fixedDeltaTime;
    }

}
