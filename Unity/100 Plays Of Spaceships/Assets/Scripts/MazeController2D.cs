using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MazeController2D : MonoBehaviour
{

    enum Direction { UP, DOWN, LEFT, RIGHT};

    Direction myDirection = Direction.UP; //Start direction
    Collider collider;

    [SerializeField] float speed;
    bool isTurning = false;

    [SerializeField] Text winText;
    
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponentInChildren<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isTurning)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            TurnAndMove(Direction.UP);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            TurnAndMove(Direction.DOWN);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            TurnAndMove(Direction.LEFT);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            TurnAndMove(Direction.RIGHT);
        }
    }

    void TurnAndMove(Direction direction)
    {

        StartCoroutine("TurnToDirection", direction);

    }

    IEnumerator TurnToDirection(Direction direction)
    {
        isTurning = true;
        bool needsToTurn = true;
        if(direction == myDirection)
        {
            needsToTurn = false;
        }

        

        Vector3 lookDirection = Vector3.zero;

        switch (direction)
        {
            case Direction.UP:
                lookDirection = Vector3.up;
                break;
            case Direction.DOWN:
                lookDirection = Vector3.down;
                break;
            case Direction.LEFT:
                lookDirection = Vector3.left;
                break;
            case Direction.RIGHT:
                lookDirection = Vector3.right;
                break;
            default:
                break;
        }


        if (needsToTurn)
        {

            Quaternion endRot = Quaternion.LookRotation(lookDirection,Vector3.back);
            Quaternion startRot = transform.rotation;

            int cycles = 12;
            float timeToTurn = speed;

            if(myDirection == Direction.UP && direction == Direction.DOWN ||
                myDirection == Direction.DOWN && direction == Direction.UP ||
                myDirection == Direction.LEFT && direction == Direction.RIGHT ||
                myDirection == Direction.RIGHT && direction == Direction.LEFT)
            {
                timeToTurn = timeToTurn * 2f;
            }

            for (int i = 0; i < cycles; i++)
            {
                float c = (1 / (float)cycles) * (float)i;
                //float zRotation = Mathf.SmoothStep(startRotation.z, lookDirection.z, c);

                transform.rotation = Quaternion.Lerp(startRot, endRot, c);

                yield return new WaitForSeconds(timeToTurn * Time.deltaTime);
            }

            transform.rotation = endRot;
        }

        //Now pointing right way:

        Ray ray = new Ray(transform.position, transform.forward);

        RaycastHit hit;

        if(Physics.Raycast(ray,out hit, 2f))
        {
            //cant move
        }
        else
        {
            Vector3 endPos = transform.position + (lookDirection*2);
            Vector3 startPos = transform.position;

            int cycles = 12;
            for (int i = 0; i < cycles; i++)
            {
                float c = (1 / (float)cycles) * (float)i;
                //float zRotation = Mathf.SmoothStep(startRotation.z, lookDirection.z, c);
                Vector3 movePos = new Vector3(
                    Mathf.SmoothStep(startPos.x, endPos.x, c),
                    Mathf.SmoothStep(startPos.y, endPos.y, c),
                    Mathf.SmoothStep(startPos.z, endPos.z, c)
                    );

                transform.position = movePos;

                yield return new WaitForSeconds(speed * Time.deltaTime);
            }
            transform.position = endPos;
        }


        myDirection = direction;

        isTurning = false;
        yield return null;
    }

    private void OnTriggerStay(Collider other)
    {
       
        Win();
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Win();
    }

    private void Win()
    {
        winText.text = "WIN!";
    }
}
