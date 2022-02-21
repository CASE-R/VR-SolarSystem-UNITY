using System.Collections;
using System.Collections.Generic;
//using System.Math.Sqrt;
using UnityEngine;

public class EllipticalTest2 : MonoBehaviour
{
    // Creates initial parameters
    [SerializeField] public int frameRate = 60;
    [Range(0f, 100f)]
    [SerializeField] public float initialTimeScale = 1;
    [Range(0.015f, 1f)]
    [SerializeField] public float initialFixedTimeStep = 0.02f;

    //Gives visual timers (NOT TO BE CHANGED IN EDITOR)
    [SerializeField] private float timeStart;
    [SerializeField] private float physTimeStart;

    public float G = 0.08892541f;
    public GameObject[] celestials; // [Sun, M, V, E, Moo, Mars, J, S, U, N] 
    public float[] periVelocity = { 34.06311235f, 20.36335006f, 17.49255161f, 0.625147704f, 15.30514423f, 7.92113719f, 5.857966867f, 4.118228776f, 3.161988437f};

    //Original calculated on initial startup, changing G mid
    void InitialVelocity()
    {
        for (int COi = 0; COi < 1; COi++) // Want only Sun-Celestial pairing, hence we pick index 0 from celestials[] and break once length exceeds 1
        {
            for (int COj = 1; COj < celestials.Length; COj++) // Coupling Celestial-semiMajor
            {
                if (COi != COj)
                {

                    //periVelocity[COj-1];  Aligning Body-semiMajor coupling to match correct Celestial, would use "COj" if you included semiMajor for Sun (which is 0)
                    //float m_COi = celestials[COi].GetComponent<Rigidbody>().mass;
                    //float m_COj = celestials[COj].GetComponent<Rigidbody>().mass;

                    //float m_CoM = m_COi + m_COj; //Assigns Centre of Mass of 2-Body to orbit around
                    //float r = Vector3.Distance(celestials[COi].transform.position, celestials[COj].transform.position); //Distance between 2-body
                    //celestials[COi].transform.LookAt(celestials[COj].transform); //Gives radial distance to "celestial1" or "COi"

                    celestials[COj].GetComponent<Rigidbody>().velocity += celestials[COj].transform.forward * periVelocity[COj - 1];


                    //print(celestials[COi] + " is paired to " + celestials[COj] + " with semiMajorAxis " + semiMajorAxis + " and orbitalVelocity " + celestials[COi].GetComponent<Rigidbody>().velocity.magnitude); // Prints pairing w/ orbit velocity for debug

                    //- (1 / (2*i)) 
                    // celestials.[COi].GetComponent<Rigidbody>().velocity += celestials.[COi].transform.right * Mathf.Sqrt(G * m2 / r);

                }
            }
        }
    }


    //Vector3 direction = celestial1.transform.position - celestial2.transform.position;

    //float semiMajor = celestial2.GetComponent<CelestialProperty>().semiMajorAxis;

    //celestial1.GetComponent<Rigidbody>().velocity = Vector3.Cross(direction, Vector3.forward).normalized * Mathf.Sqrt(G * mass2 * ((2 / r) - (1 / semiMajor)));

    //Debug.Log("Position Vector is " + direction + "with 'magnitude'" + r);

    //Debug.Log("Velocity of " + celestial1.name + "is" + celestial1.GetComponent<Rigidbody>().velocity);

    // Start is called before the first frame update
    void Start()
    {
        celestials = GameObject.FindGameObjectsWithTag("Celestial");
        CelestialProperty celProperty = GetComponent<CelestialProperty>();

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

                    celestial1.GetComponent<Rigidbody>().AddForce((celestial2.transform.position - celestial1.transform.position).normalized * (G * m1 * m2 / (r * r)));


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
