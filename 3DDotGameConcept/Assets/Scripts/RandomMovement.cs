using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    public float movementSpeed = 1.0f;
    public float rotationSpeed = 100f;
    public bool isWandering = false;
    public bool isRotatingLeft = false;
    public bool isRotatingRight = false;
    public bool isWalking = false;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!isWandering)
        {
            StartCoroutine(Wander());
        }

        if (isRotatingRight)
        {
            transform.Rotate(transform.up * Time.deltaTime * rotationSpeed);
        }
        if (isRotatingLeft)
        {
            transform.Rotate(transform.up * Time.deltaTime * -rotationSpeed);
        }

        if (isWalking)
        {
            rb.AddForce(transform.forward * movementSpeed);
        }

    }

    IEnumerator Wander()
    {
        int rotationTime = Random.Range(1, 3);
        int rotateWait = Random.Range(1, 3);
        int rotateDirection = Random.Range(1, 3);
        int walkWait = Random.Range(1, 3);
        int walkTime = Random.Range(1, 3);

        isWandering = true;

        yield return new WaitForSeconds(walkWait);

        isWalking = true;

        yield return new WaitForSeconds(walkTime);

        isWalking = false;

        yield return new WaitForSeconds(rotateWait);

        if(rotateDirection == 1)
        {
            isRotatingLeft = true;

            yield return new WaitForSeconds(rotationTime);

            isRotatingLeft = false;
        }
        if (rotateDirection == 2)
        {
            isRotatingRight = true;

            yield return new WaitForSeconds(rotationTime);

            isRotatingRight = false;
        }

        isWandering = false;
    }

}
