using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasHandler : MonoBehaviour
{

    public enum LevelsOfDetail { Shader, Active, Inactive, NULL };

    [SerializeField] Vector2 LOD;
    [Tooltip("Distance at which LOD maxes out")]
    [SerializeField] float LODCutoff = 3f;


    LevelsOfDetail currentLOD;
    Camera cam;

    GenerateGasGiant gasGiantGenerator;
    int currentOctaves;
    int maxOctaves;


    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        gasGiantGenerator = GetComponent<GenerateGasGiant>();
        gasGiantGenerator.transform.parent = transform;


        gasGiantGenerator.GeneratePlanet();

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


        float distanceToCam = Vector3.Distance(gasGiantGenerator.transform.position, cam.transform.position);

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
            gasGiantGenerator.ActivateShader();
        }




    }
    private void HandleActive()
    {
        currentLOD = LevelsOfDetail.Active;
        print("NOW ACTIVE");

        gasGiantGenerator.ActivateSphere();

    }

    private void HandleInactive()
    {
        currentLOD = LevelsOfDetail.Inactive;
        gasGiantGenerator.DeactivateAll();

    }

    float Remap(float s, float a1, float a2, float b1, float b2) => b1 + (s - a1) * (b2 - b1) / (a2 - a1);

    //public float Remap(float value, float inputx, float inputy, float outputx, float outputy)
    //{
    //    //https://forum.unity.com/threads/re-map-a-number-from-one-range-to-another.119437/
    //    return (value - inputx) / (inputy - inputx) * (outputy - outputx) + outputx;
    //}
}