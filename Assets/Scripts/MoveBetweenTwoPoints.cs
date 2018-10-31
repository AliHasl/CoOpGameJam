using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// © 2017 TheFlyingKeyboard and released under MIT License
// theflyingkeyboard.net
//Moves object between two points
public class MoveBetweenTwoPoints : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0.0f;
    [SerializeField] private GameObject pointA = null;
    [SerializeField] private GameObject pointB = null;
    [SerializeField] private bool reverseMove = false;
    [SerializeField] private Transform objectToUse;
    [SerializeField] private bool moveThisObject = false;
    private float startTime;
    private float journeyLength;
    private float distCovered;
    private float fracJourney;

    public void setValues(float moveSpeed, GameObject pointA, GameObject pointB)
    {
        this.moveSpeed = moveSpeed;
        this.pointA = pointA;
        this.pointB = pointB;
        

        startTime = Time.time;

        objectToUse = GetComponentInParent<Transform>();

        journeyLength = Vector3.Distance(pointA.transform.position, pointB.transform.position);
    }

    void Update()
    {
        if (moveSpeed != 0.0f)
        {
            distCovered = (Time.time - startTime) * moveSpeed;
            fracJourney = distCovered / journeyLength;

            objectToUse.position = Vector3.Slerp(pointA.transform.position, pointB.transform.position, fracJourney);
        }
    }
}