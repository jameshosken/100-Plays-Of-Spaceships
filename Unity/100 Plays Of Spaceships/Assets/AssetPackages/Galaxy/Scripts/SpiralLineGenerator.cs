// Generates single spiral, used for individual arms within galaxy.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galaxy
{
    public class SpiralLineGenerator : MonoBehaviour
    {

        // Serialized in case you want to use this script on its own:
        [SerializeField] int numberOfPoints;                // How many points/segments in each spiral
        [SerializeField] float fidelity;                    // How far between each point / how long is each segment
        [SerializeField] Vector2 t_scale = Vector2.one;     // How big is the spiral relative to the gameObject
        [SerializeField] float t_multiplier = 1f;           // Affects the amount of curve to the spiral
        [SerializeField] float t_exponent = 1f;             // Affects the falloff; how sharply the spiral turns. Lower values create more compact edges
        [SerializeField] float t_rotation = 0;              // Useful for constructing multi-arm galaxies

        LineRenderer line;

        List<Vector3> spiral = new List<Vector3>();
        void Awake()
        {
            line = GetComponent<LineRenderer>();
        }

        public List<Vector3> UpdateSettings(int _numberOfPoints, float _fidelity, Vector2 _t_scale, float _t_multiplier, float _t_exponent, float _t_rotation)
        {

            numberOfPoints = _numberOfPoints;
            fidelity = _fidelity;
            t_scale = _t_scale;
            t_multiplier = _t_multiplier;
            t_exponent = _t_exponent;
            t_rotation = _t_rotation;

            SetSpiral();

            return spiral;

        }

        private void SetSpiral()
        {
            fidelity = Mathf.Clamp(fidelity, 0.001f, float.MaxValue);

            spiral.Clear();

            for (int i = 0; i < numberOfPoints; i++)
            {
                // Equation for a spiral
                float t = (float)i * fidelity;
                float x = Mathf.Pow(t, t_exponent) * Mathf.Cos(t_multiplier * t + t_rotation);
                float y = Mathf.Pow(t, t_exponent) * Mathf.Sin(t_multiplier * t + t_rotation);

                // This line places the point from local space into world space, so we can rotate and translate the spiral
                Vector3 point = transform.TransformPoint(new Vector3(x * t_scale.x, y * t_scale.y, 0));
                spiral.Add(point);

            }

            line.positionCount = spiral.Count;
            line.SetPositions(spiral.ToArray());
        }


    }
}