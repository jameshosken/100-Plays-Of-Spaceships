//Generates a collection of random galaxies

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galaxy
{
    public class GalaxyGeneratorGenerator : MonoBehaviour
    {
        [SerializeField] GameObject galaxyTemplate;

        [SerializeField] int numberOfGalaxies = 10;

        [SerializeField] float maxRadius = 20f;

        // Start is called before the first frame update
        void Start()
        {
            for (int i = 0; i < numberOfGalaxies; i++)
            {

                Vector3 point = UnityEngine.Random.onUnitSphere * UnityEngine.Random.Range(0, maxRadius);
                Quaternion rotation = UnityEngine.Random.rotation;
                GameObject galaxy = Instantiate(galaxyTemplate, point, rotation);

                galaxy.GetComponent<SpiralGalaxyGenerator>().randomize = true;

            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}