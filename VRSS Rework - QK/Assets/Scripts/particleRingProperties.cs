using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleRingProperties : MonoBehaviour
{
    GameObject parentObj;
    
    // Start is called before the first frame update
    void Start()
    {
        parentObj = gameObject.GetComponentInParent<BodyProperties>().gameObject;
    }
    public void OnValidate()
    {
        if (parentObj.GetComponent<SimulationScript>() == null)
        {
            parentObj = parentObj.GetComponentInParent<BodyProperties>().gameObject;
        }

        Debug.Log(parentObj.name);
        //GameObject[] celestials = parentObj.GetComponent<SimulationScript>().celestials;


        //foreach (GameObject Celestial in celestials)
        //{
        //    if (Celestial.GetComponentInChildren<ParticleSystemForceField>() != null)
        //    {
        //        //Debug.Log("Has ring");
        //        var fo = Celestial.GetComponentInChildren<ParticleSystemForceField>();
        //        //fo.enabled = true;

        //        fo.gravity = 1;
        //        //fo.endRange = Celestial.transform.lossyScale.x * 50f;
        //        fo.directionX = fo.directionY = fo.directionZ = gravitationalConstant;

        //        fo.rotationAttraction = 1;
        //        fo.rotationSpeed = Mathf.Sqrt(gravitationalConstant * Celestial.GetComponent<BodyProperties>().mass / Celestial.GetComponent<BodyProperties>().volumetricMeanRadius);
        //    }
        //}

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
