using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthHandler : MonoBehaviour
{

    public enum LevelsOfDetail { Shader, Active, Inactive, NULL };

    [SerializeField] Vector2 LOD;
    [Tooltip("Distance at which LOD maxes out")]
    [SerializeField] float LODCutoff = 3f;


    LevelsOfDetail currentLOD;
    Camera cam;

    GenerateEarthlikePlanet earthGenerator;
    int currentOctaves;
    int maxOctaves;


    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        earthGenerator = GetComponent<GenerateEarthlikePlanet>();


        earthGenerator.GeneratePlanet();
        earthGenerator.transform.parent = transform;
        maxOctaves = earthGenerator.maxOctaves;


        currentLOD = LevelsOfDetail.NULL;

        CalculateLOD();

    }

    // Update is called once per frame
    void Update()
    {
        CalculateLOD();
    }

    private void CalculateLOD()
    {


        float distanceToCam = Vector3.Distance(transform.position, cam.transform.position);

        if (distanceToCam < LOD.x)
        {
            HandleShader(distanceToCam);
        }
        else if (distanceToCam < LOD.y)
        {
            if (currentLOD != LevelsOfDetail.Active)
            {
                HandleActive();
            }
        }
        else
        {
            if (currentLOD != LevelsOfDetail.Inactive)
            {
                HandleInactive();
            }
        }
    }

    private void HandleShader(float distance)
    {

        if (currentLOD != LevelsOfDetail.Shader)
        {
            earthGenerator.ActivateShader();
        }

        maxOctaves = earthGenerator.maxOctaves;
        currentLOD = LevelsOfDetail.Shader;

        //distance = Mathf.Clamp(distance, LODCutoff, LOD.x);

        int projectedOctaves = (int)Remap(distance, 0, LOD.x, maxOctaves, 2); // Output is revesred

        if (projectedOctaves != currentOctaves)
        {
            print("Shader LOD Change: " + projectedOctaves.ToString());

            earthGenerator.SetLOD(projectedOctaves);
            currentOctaves = projectedOctaves;
        }



    }
    private void HandleActive()
    {
        currentLOD = LevelsOfDetail.Active;
        print("NOW ACTIVE");

        earthGenerator.ActivateSphere();

    }

    private void HandleInactive()
    {
        currentLOD = LevelsOfDetail.Inactive;

        earthGenerator.DeactivateAll();

    }

    float Remap(float s, float a1, float a2, float b1, float b2) => b1 + (s - a1) * (b2 - b1) / (a2 - a1);

    //public float Remap(float value, float inputx, float inputy, float outputx, float outputy)
    //{
    //    //https://forum.unity.com/threads/re-map-a-number-from-one-range-to-another.119437/
    //    return (value - inputx) / (inputy - inputx) * (outputy - outputx) + outputx;
    //}
}