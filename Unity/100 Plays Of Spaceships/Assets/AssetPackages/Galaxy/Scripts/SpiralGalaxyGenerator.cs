// Stores settings for a single galaxy, and renders galaxy into scene.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galaxy
{
    public class SpiralGalaxyGenerator : MonoBehaviour
    {

        [Header("General Settings")]



        [SerializeField] GameObject spiralArmTemplate;
        [SerializeField] GameObject gasParticleTemplate;
        //[SerializeField] GameObject starParticleTemplate;
        [SerializeField] GameObject SolarSystemGenerator;
        [SerializeField] bool playOnAwake = false;
        [SerializeField] bool autoUpdate = false;
        public bool GenerateGalaxyTrigger = false;
        public bool randomize = false;

        [Space]
        [Header("Spiral Settings")] //See SpiralLineGenerator for an overview of these parameters.

        [Range(0, 16)]
        [SerializeField] int numberOfSpirals;
        [Tooltip("Number of sections in each spiral arm")]
        [Range(1, 100)]
        [SerializeField] int numberOfPoints = 50;
        [Tooltip("Space between each point on a spiral arm")]
        [Range(0.0001f, 2f)]
        [SerializeField] float fidelity = 0.1f;
        [Tooltip("XY scale of spiral")]
        [SerializeField] Vector2 t_scale = Vector2.one;
        [Range(0.0001f, 10f)]
        [SerializeField] float t_multiplier = 1f;
        [Range(0.0001f, 10f)]
        [SerializeField] float t_exponent = 1f;


        [Space]
        [Header("Gas Particle Settings")]

        [Range(1, 6000)]
        [SerializeField] int gasMaxParticles;
        [Tooltip("Min, Max")]
        [SerializeField] Vector2 gasPositionOffset;
        [Tooltip("Min, Max")]
        [SerializeField] Vector2 gasVelocity;
        [Tooltip("Min, Max")]
        [SerializeField] Vector2 gasSize;
        [Tooltip("Min, Max")]
        [SerializeField] Vector2 gasLife;
        [Tooltip("Galaxy colour gradient")]
        [SerializeField] Color32[] gasColours = { new Color32(0, 0, 0, 0), new Color32(255, 255, 255, 255) };
        [SerializeField] Texture2D gasParticleTexture;

        //[Space]
        //[Header("Star Particle Settings")]
        //[SerializeField] int starMaxParticles;
        //[Tooltip("Min, Max")]
        //[SerializeField] Vector2 starPositionOffset;
        //[Tooltip("Min, Max")]
        //[SerializeField] Vector2 starVelocity;
        //[Tooltip("Min, Max")]
        //[SerializeField] Vector2 starSize;
        //[Tooltip("Min, Max")]
        //[SerializeField] Vector2 starLife;
        //[SerializeField] Color32[] starColours = { new Color32(0, 0, 0, 0), new Color32(255, 255, 255, 255) };
        [Header("Solar Systems")]
        [SerializeField] int numberOfSystems;
        [SerializeField] float maxOffset;

        [Space]
        [Header("Visibility Settings")]
        [SerializeField] bool showGas = true;
        [SerializeField] bool showStars = true;
        [SerializeField] bool showSpirals = true;
        bool pSpirals = true;

        //Particle Systems
        ParticleSystem gasParticles;
        //ParticleSystem starParticles;

        //Spiral information
        List<SpiralLineGenerator> spirals = new List<SpiralLineGenerator>();
        List<Vector3> points = new List<Vector3>();

        List<GameObject> solarSystems = new List<GameObject>();
        bool awake = false;
        void Start()
        {
            awake = true;

            GameObject gasParticleObject = Instantiate(gasParticleTemplate, transform.position, transform.rotation);
            gasParticleObject.transform.parent = transform;
            gasParticles = gasParticleObject.GetComponent<ParticleSystem>();
            ParticleSystemRenderer gasRenderer = gasParticleObject.GetComponent<ParticleSystemRenderer>();

            gasMaxParticles = numberOfPoints * 100;

            gasRenderer.material.SetTexture("_MainTex", gasParticleTexture);

            //GameObject starParticleObject = Instantiate(starParticleTemplate, transform.position, transform.rotation);
            //starParticleObject.transform.parent = transform;
            //starParticles = starParticleObject.GetComponent<ParticleSystem>();



            if (randomize)
            {
                RandomizeSettings();
            }

            if (playOnAwake)
            {
                GenerateGalaxy();
            }
        }



        public void RandomizeSettings()
        {

            // These randomized values are hard coded and not necessarily meant for production use. 
            // This is merely a demonstration of how you might set up a random collection of galaxies

            numberOfSpirals = (int)UnityEngine.Random.Range(1, 9);
            numberOfPoints = (int)UnityEngine.Random.Range(20, 40); ;
            fidelity = UnityEngine.Random.Range(0.1f, 0.3f);
            t_scale = Vector2.one * UnityEngine.Random.Range(1, 4);
            t_multiplier = UnityEngine.Random.Range(1, 3);
            t_exponent = UnityEngine.Random.Range(.5f, 2);

            gasColours[0] = UnityEngine.Random.ColorHSV();
            gasColours[1] = UnityEngine.Random.ColorHSV();

            gasColours[0].a = 20;
            gasColours[1].a = 20;

            //starColours[0] = UnityEngine.Random.ColorHSV(0f,1f, 0f,.5f);
            //starColours[1] = UnityEngine.Random.ColorHSV(0f, 1f, 0f, .5f);

        }

        void ClearGalaxy()
        {
            for (int i = spirals.Count - 1; i >= 0; i--)
            {
                GameObject.Destroy(spirals[i].gameObject);
            }
            points.Clear();
            spirals.Clear();
        }

        void GenerateGalaxy()
        {
            ClearGalaxy();
            for (int i = 0; i < numberOfSpirals; i++)
            {
                float angle = 2 * Mathf.PI * i / numberOfSpirals;
                GameObject clone = Instantiate(spiralArmTemplate, transform.position, transform.rotation) as GameObject;

                SpiralLineGenerator spiral = clone.GetComponent<SpiralLineGenerator>();

                points.AddRange(spiral.UpdateSettings(numberOfPoints, fidelity, t_scale, t_multiplier, t_exponent, angle));
                spiral.transform.parent = transform;
                spirals.Add(spiral);

            }

            PrewarmGasParticles();
            //PrewarmStarParticles();
            GenerateSolarSystems();
        }

        private void GenerateSolarSystems()
        {

            for(int i = 0; i < numberOfSystems; i++)
            {
                int choice = UnityEngine.Random.Range(0, points.Count);

                GameObject system = Instantiate(SolarSystemGenerator);
                system.transform.parent = transform;
                system.transform.rotation = UnityEngine.Random.rotation;

                //Ensure not too close to other:

                float min = float.MaxValue;

                Vector3 position = points[choice] + UnityEngine.Random.onUnitSphere * UnityEngine.Random.Range(0f, maxOffset);

                system.transform.localPosition = position;
                solarSystems.Add(system);
            }
        }

        public List<GameObject> GetSystems()
        {
            return solarSystems;
        }

        // Update is called once per frame
        void Update()
        {
            if (GenerateGalaxyTrigger)
            {

                GenerateGalaxy();
                GenerateGalaxyTrigger = false;
            }

            //Make sure particles stay at a certain amount:
            if (points.Count > 0)
            {
                ThresholdEmitGasParticles();
                //ThresholdEmitStarParticles();
            }

            HandleVisibility();

        }

        private void HandleVisibility()
        {
            // Gas
            if (!showGas && gasParticles.gameObject.activeSelf)
            {
                gasParticles.gameObject.SetActive(false);
            }

            if (showGas && !gasParticles.gameObject.activeSelf)
            {
                gasParticles.gameObject.SetActive(true);
            }

            //// Stars
            //if (!showStars && starParticles.gameObject.activeSelf)
            //{
            //    starParticles.gameObject.SetActive(false);
            //}

            //if (showStars && !starParticles.gameObject.activeSelf)
            //{
            //    starParticles.gameObject.SetActive(true);
            //}

            // Spirals
            foreach (SpiralLineGenerator spiral in spirals)
            {
                if (spiral.gameObject.activeSelf != showSpirals)
                {
                    spiral.gameObject.SetActive(showSpirals);
                }
            }

        }

        /// <summary>
        /// STAR
        /// </summary>

        //private void PrewarmStarParticles()
        //{
        //    for (int i = 0; i < points.Count; i++)
        //    {

        //        if (starParticles.particleCount < starMaxParticles)
        //        {
        //            // This little section prevents particle clumping around points 
        //            // when the sprial points are far apart

        //            Vector3 point = points[i];
        //            if (i > 0)
        //            {
        //                point = Vector3.Lerp(point, points[i - 1], UnityEngine.Random.Range(0f, 1f));
        //            }
        //            EmitStarParticle(point);
        //        }
        //    }
        //}

        //void ThresholdEmitStarParticles()
        //{
        //    while (starParticles.particleCount < starMaxParticles)
        //    {
        //        EmitStarParticle(ChooseRandomPointFromList(points));
        //    }
        //}

        //private void EmitStarParticle(Vector3 point)
        //{

        //    ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
        //    emitParams.position = point + UnityEngine.Random.insideUnitSphere * UnityEngine.Random.Range(starPositionOffset.x, starPositionOffset.y);
        //    emitParams.velocity = UnityEngine.Random.insideUnitSphere * UnityEngine.Random.Range(starVelocity.x, starVelocity.y); // Random velocity from min to max
        //    emitParams.startSize = UnityEngine.Random.Range(starSize.x, starSize.y);
        //    emitParams.startLifetime = UnityEngine.Random.Range(starLife.x, starLife.y);
        //    emitParams.startColor = Color32.Lerp(starColours[0], starColours[1], UnityEngine.Random.Range(0f, 1f));
        //    starParticles.Emit(emitParams, 1);
        //}

        /// <summary>
        /// GAS
        /// </summary>

        private void PrewarmGasParticles()
        {
            for (int i = 0; i < points.Count; i++)
            {
                if (gasParticles.particleCount < gasMaxParticles)
                {
                    Vector3 point = points[i];
                    if (i > 0)
                    {
                        point = Vector3.Lerp(point, points[i - 1], UnityEngine.Random.Range(0f, 1f));
                    }
                    EmitGasParticle(point);
                }

            }
        }

        void ThresholdEmitGasParticles()
        {
            while (gasParticles.particleCount < gasMaxParticles)
            {
                EmitGasParticle(ChooseRandomPointFromList(points));
            }
        }

        private void EmitGasParticle(Vector3 point)
        {

            ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
            emitParams.position = point + UnityEngine.Random.insideUnitSphere * UnityEngine.Random.Range(gasPositionOffset.x, gasPositionOffset.y);
            emitParams.velocity = UnityEngine.Random.insideUnitSphere * UnityEngine.Random.Range(gasVelocity.x, gasVelocity.y); // Random velocity from min to max
            emitParams.startSize = UnityEngine.Random.Range(gasSize.x, gasSize.y);
            emitParams.startLifetime = UnityEngine.Random.Range(gasLife.x, gasLife.y);
            emitParams.startColor = Color32.Lerp(gasColours[0], gasColours[1], UnityEngine.Random.Range(0f, 1f));
            gasParticles.Emit(emitParams, 1);
        }

        private void OnValidate()
        {
            if (autoUpdate && awake)
            {
                GenerateGalaxy();
            }
        }

        Vector3 ChooseRandomPointFromList(List<Vector3> list)
        {
            int index = (int)UnityEngine.Random.Range(1, list.Count);

            // Prevent cluping around corners
            Vector3 point = points[index];
            point = Vector3.Lerp(point, points[index - 1], UnityEngine.Random.Range(0f, 1f));
            

            return point;
        }

    }
}