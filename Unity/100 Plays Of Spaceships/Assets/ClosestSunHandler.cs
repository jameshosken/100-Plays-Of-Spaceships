using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galaxy
{
    public class ClosestSunHandler : MonoBehaviour
    {
        [SerializeField] SpiralGalaxyGenerator galaxyGenerator;
        [SerializeField] GameObject sunLight;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Time.frameCount % 12 == 0)
            {
                CalculateNearestStar();
            }
        }

        private void CalculateNearestStar()
        {
            List<GameObject> systems = galaxyGenerator.GetSystems();
            Vector3 nearestPos = Vector3.zero;

            float min = float.MaxValue;

            for (int i = 0; i < systems.Count; i++)
            {
                Vector3 center = systems[i].GetComponent<GenerateSolarSystem>().GetCenter();

                float dist = Vector3.Distance(transform.position, center);
                if (dist < min)
                {
                    min = dist;
                    nearestPos = center;
                }
            }

            sunLight.transform.position = nearestPos;

        }
    }

}