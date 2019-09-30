// Provides basic camera movement for Galaxy Generator demos.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galaxy
{
    public class GalaxyCameraController : MonoBehaviour
    {
        enum UpdateType { Update, Late, Fixed }

        [SerializeField] UpdateType updateType;
        [SerializeField] float thrust = .1f;
        [SerializeField] float maxVel = 1f;
        [SerializeField] float turnSpeed = 1f;
        [Space]
        [SerializeField] float drag = .5f;
        [SerializeField] float boost = 6f;
        [SerializeField] float superboost = 12f;

        [SerializeField] float friction = 0.99f;


        Vector3 velocity = Vector3.zero;
        List<Vector3> rotations = new List<Vector3>();
        // Update is called once per frame
        void Update()
        {
            if (updateType != UpdateType.Update) { return; }

            HandleCameraControls(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            if (updateType != UpdateType.Fixed) { return; }

            HandleCameraControls(1f);
        }

        private void LateUpdate()
        {
            if (updateType != UpdateType.Late) { return; }

            HandleCameraControls(Time.deltaTime);
    }

    void HandleCameraControls(float dTime)
    {

            float multiplier = 1;
            if (Input.GetKey(KeyCode.LeftAlt))
            {
                multiplier = superboost;
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                multiplier = boost;
            }
            

            if (Input.GetKey(KeyCode.Space))
            {
                multiplier = drag;
            }

            float vel = Input.GetAxis("Vertical") * thrust * multiplier * dTime;
            float strafe = Input.GetAxis("Horizontal") * thrust * multiplier * dTime;

            

            velocity += transform.forward * vel;
            velocity += transform.right * strafe;

            if(velocity.magnitude > maxVel)
            {
                velocity = velocity.normalized * maxVel;
            }

            transform.Translate(velocity, Space.World);

            velocity *= friction;



            if (Input.GetMouseButton(0))
            {
                float xTurn = Input.GetAxis("Mouse X") * turnSpeed * dTime;
                float yTurn = Input.GetAxis("Mouse Y") * -turnSpeed * dTime;

                
                rotations.Add(new Vector3(xTurn, yTurn, 0));

                
            }
            else
            {
                rotations.Add(Vector3.zero);
            }

            if(rotations.Count > 30)
            {
                rotations.RemoveAt(0);
            }


            if(rotations.Count > 0)
            {
                Vector3 average = Vector3.zero;
                foreach(Vector3 rot in rotations)
                {
                    average += rot;
                }
                average /= (float)rotations.Count;


                transform.Rotate(new Vector3(0, average.x, 0), Space.Self);
                transform.Rotate(new Vector3(average.y, 0, 0), Space.Self);
            }
            
        }
    }
}