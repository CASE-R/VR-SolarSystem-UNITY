using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitMotion : MonoBehaviour //Using: https://www.youtube.com/watch?v=lKfqi52PqHk
{
    public Transform orbitingObject;
    public Ellipse orbitPath;

    [Range(0f, 1f)]
    public float orbitProgress = 0f;
    public float orbitPeriod = 3f;
    public bool orbitActive = true;

    // Start is called before the first frame update
    void Start()
    {
        if (orbitingObject == null)
        {
            orbitActive = false;
            return;
        }
        setOrbitingObjectPosition();
        //if (orbitActive) start orbit animation
        StartCoroutine(AnimateOrbit ());
    }

    void setOrbitingObjectPosition()
    {
        Vector2 orbitPos = orbitPath.Evaluate (orbitProgress);
        orbitingObject.localPosition = new Vector3(orbitPos.x, 0, orbitPos.y);
    }
    
    IEnumerator AnimateOrbit ()
    {
        if (orbitPeriod < 0.1f)
        {
            orbitPeriod = 0.1f;
        }
        float orbitSpeed = 1f / orbitPeriod;
        while (orbitActive)
        {
            orbitProgress += Time.deltaTime * orbitSpeed;
            orbitProgress %= 1f; //If we go beyond 1f, reset back to [0,1]
            setOrbitingObjectPosition ();
            yield return null;
        }
    }
}
