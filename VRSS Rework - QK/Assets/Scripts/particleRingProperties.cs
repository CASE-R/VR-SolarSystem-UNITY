using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleRingProperties : MonoBehaviour
{
    GameObject forceField;
    GameObject parentObj;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void OnValidate()
    {
        var childCount = gameObject.transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            forceField = gameObject.transform.GetChild(i).GetComponent<ParticleSystemForceField>().gameObject;
            //Debug.Log(forceField.name);
            if (forceField != null)
            {
                parentObj = forceField.GetComponentInParent<BodyProperties>().gameObject;
                float gravConstant = parentObj.GetComponentInParent<SimulationScript>().gravitationalConstant;



                //Debug.Log("Has ring");
                var fo = forceField.GetComponent<ParticleSystemForceField>();
                //fo.enabled = true;

                fo.gravity = 1;
                //fo.endRange = Celestial.transform.lossyScale.x * 50f;
                fo.directionX = fo.directionY = fo.directionZ = gravConstant;

                fo.rotationAttraction = 1;
                float circularSpeed = Mathf.Sqrt(gravConstant * parentObj.GetComponent<BodyProperties>().mass / parentObj.GetComponent<BodyProperties>().volumetricMeanRadius);

                var rotSpeed = forceField.GetComponentInChildren<ParticleSystemForceField>().rotationSpeed;
                rotSpeed.mode = ParticleSystemCurveMode.TwoConstants;
                //rotSpeed.constantMin = circularSpeed;
                //fo.rotationSpeed = circularSpeed * 2;

                //rotSpeed.constantMin = Mathf.Sqrt(gravitationalConstant * Celestial.GetComponent<BodyProperties>().mass / Celestial.GetComponent<BodyProperties>().volumetricMeanRadius);
                //rotSpeed.constantMax = 2 * Mathf.Sqrt(gravitationalConstant * Celestial.GetComponent<BodyProperties>().mass / Celestial.GetComponent<BodyProperties>().volumetricMeanRadius);

            }
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
