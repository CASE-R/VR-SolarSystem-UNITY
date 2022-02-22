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

    float[] perihelion = {30.749074185f, 71.845880293f, 98.326849289f, 95.061307948f, 138.136873488f, 495.056752096f, 907.468014278f, 1826.692870226f, 2988.709742109f};
    float[] aphelion = {46.670410032f, 72.822497627f, 101.672482252f, 105.317340535f, 166.620543055f, 545.704488028f, 1007.050227944f, 2006.303560208f, 3047.405045522f};

    public float massCOi;
    public float massCOj;
    /// <summary>
    /// Below are old lists used, kept for future reference but unimportant
    /// </summary>
    //public float[] periVelocity = { 34.06311235f, 20.36335006f, 17.49255161f, 0.625147704f, 15.30514423f, 7.92113719f, 5.857966867f, 4.118228776f, 3.161988437f};
    //float[] semiMajor = {38.709742109f, 72.334188960f, 99.999665771f, 101.782049101f, 152.378708272f, 520.380620062f, 957.259121111f, 1916.498215217f, 3018.057393815f}; // [4] = Moon wrt to Sun
    //{ 38.70974211f, 72.33385473f, 99.99799463f, 0.256955307f, 152.3790425f, 520.3806201f, 957.2594553f, 1916.498215f, 3018.05706f }; // [4]= Moon wrt to Earth

    void InitialVelocity()
    {
        for (int COi = 0; COi < 1; COi++) // Want only Sun-Celestial pairing, hence we pick index 0 from celestials[] and break once length exceeds 1
        {
            for (int COj = 1; COj < celestials.Length; COj++) // Coupling Celestial-semiMajor
            {
                if (COi != COj)
                {
                    massCOi = celestials[COi].GetComponent<Rigidbody>().mass;
                    massCOj = celestials[COj].GetComponent<Rigidbody>().mass;
                    Debug.Log("COiMass = " + massCOi + " COjMass = " + massCOj);
                    float semiMajor = (perihelion[COj - 1] + aphelion[COj - 1]) / 2; //Can be proven that 2a = r_p + r_A


                    float distance = Vector3.Distance(celestials[COi].transform.position, celestials[COj].transform.position); //Radial Distance between 2-body

                    celestials[COi].transform.LookAt(celestials[COj].transform);
                    Debug.Log("Distance is " + distance);

                    // Using original visViva
                    celestials[COj].GetComponent<Rigidbody>().velocity += celestials[COj].transform.forward * Mathf.Sqrt((G * (massCOi + massCOj)) * ((2 / distance) - (1 / (semiMajor)))); //Applies Vis Viva Orbital Velocity Equation, this is wrt. whatever "COi" is in the anti-clockwise direction

                    Debug.Log("Velocity of " + COj + " is " + celestials[COj].GetComponent<Rigidbody>().velocity);

                }
            }
        }
    }

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
