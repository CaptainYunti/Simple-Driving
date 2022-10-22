using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{

    [SerializeField] float speed = 10f;
    [SerializeField] float acceleration = .1f;
    [SerializeField] float rotationSpeed = 200f;

    int steerValue;


    void Update()
    {
        SteeringProcess();
        SpeedProcess();
    }

    private void SteeringProcess()
    {
        transform.Rotate(0f, steerValue * rotationSpeed * Time.deltaTime, 0f);
    }

    private void SpeedProcess()
    {
        speed += acceleration * Time.deltaTime;
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public void Steer(int value)
    {
        steerValue = value;
    }
}
