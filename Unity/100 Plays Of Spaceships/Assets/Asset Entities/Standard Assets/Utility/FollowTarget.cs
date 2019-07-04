using System;
using UnityEngine;


namespace UnityStandardAssets.Utility
{

    public class FollowTarget : MonoBehaviour
    {

        [SerializeField] bool velocityCompensation;
        [SerializeField] float vCompAmount = 3f;
        public Transform target;
        public Vector3 offset = new Vector3(0f, 7.5f, 0f);


        private void LateUpdate()
        {

            if (velocityCompensation)
            {
                Rigidbody body = target.GetComponent<Rigidbody>();
                transform.position = target.position + offset + body.velocity*vCompAmount;
            }
            else
            {
                transform.position = target.position + offset;
            }
            
        }
    }
}
