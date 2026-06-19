using System;
using UnityEngine;

public class SecondOrderDynamics<T> {

    private readonly Func<T, T, T> add;
    private readonly Func<T, T, T> subtract;
    private readonly Func<T, float, T> multiply;
    private readonly Func<T, float, T> divide;

    private T targetPosition;
    private T currentPosition, currentVelocity;
    private float k1, k2, k3; //Constants for current velocity, current acceleration, and target velocity

    private bool hasTargetVelocity = false;

    public SecondOrderDynamics(
            float f, float z, float r, 
            T startPos, 
            Func<T, T, T> add, 
            Func<T, T, T> subtract, 
            Func<T, float, T> multiply,
            Func<T, float, T> divide) 
        {

        //Define operators for the generic class
        this.add = add;
        this.subtract = subtract;
        this.multiply = multiply;
        this.divide = divide;


        //Calculate constants
        k1 = z / (Mathf.PI * f);
        k2 = 1 / ((2 * Mathf.PI * f) * (2 * Mathf.PI * f));
        k3 = r * z / (2 * Mathf.PI * f);

        targetPosition = startPos;
        currentPosition = startPos;
        currentVelocity = default;
    }

    // Update is called once per frame
    public T UpdateMovement(float deltatime, T targetPos, T targetVelocity = default) {
        //Estimate targetVelocity if it does not exist
        if (!hasTargetVelocity) {
            targetVelocity = divide(subtract(targetPos, targetPosition), deltatime); //(targetPos - targetPosition) / deltatime
            targetPosition = targetPos;
        }
        //Clamp k2 to avoid problems with high frequency or frame drops
        float stabledK2 = 1.1f * (deltatime * deltatime / 4 + deltatime * k1 / 2);
        float stableK2 = stabledK2 > k2 ? stabledK2 : k2;

        //Calculate position based on velocity
        currentPosition = add(currentPosition, multiply(currentVelocity, deltatime)); //currentPosition + currentVelocity * deltatime

        //Calculate velocity based on acceleration
        T newTargetVelocity = multiply((T)targetVelocity, k3);
        T newCurrentVelocity = multiply(currentVelocity, k1);
        currentVelocity = add(currentVelocity, divide(multiply((subtract(subtract(add(targetPos, newTargetVelocity), currentPosition), newCurrentVelocity)), deltatime), stableK2));
        //currentVelocity + (targetPos + k3 * (T)targetVelocity - currentPosition - k1 * currentVelocity) * deltatime / k2

        return currentPosition;
    }

    
}
