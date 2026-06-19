using UnityEngine;

/// <summary>
/// Uses Second Order Dynamics to follow a target value of type T, with tweakable values when serialized.
/// </summary>
/// <typeparam name="T"></typeparam>
[System.Serializable]
public class SecondOrderMovement<T>
{
    [SerializeField][Range(0f, 10f)] public float frequency = 1;
    [SerializeField][Range(0f, 2f)] public float zeta = 0.5f;
    [SerializeField][Range(-10f, 10f)] public float response = 1;

    private float f0, z0, r0; //Variables to track if the serializable values change, so that the setup can be done

    private SecondOrderDynamics<T> func;

    public T Update(float deltatime, T currentPos, T targetPos, T targetVelocity = default) {
        //If values have changed, do setup
        if (func == null || frequency != f0 || zeta != z0 || response != r0) {
            InitFunction(currentPos);
        } //Do calculation of position, velocity, etc

        return func.UpdateMovement(deltatime, targetPos, targetVelocity);

    }

    private void InitFunction(T initialValue) {
        //Set the checking variables to their corresponding variables again
        f0 = frequency;
        z0 = zeta;
        r0 = response;

        //Restart the script
        if (typeof(T) == typeof(float)) {
            func = (SecondOrderDynamics<T>)(object)SecondOrderFactory.Float(frequency, zeta, response, (float)(object)initialValue);
        } else if (typeof(T) == typeof(Vector2)) {
            func = (SecondOrderDynamics<T>)(object)SecondOrderFactory.Vector2(frequency, zeta, response, (Vector2)(object)initialValue);
        } else if (typeof(T) == typeof(Vector3)) {
            func = (SecondOrderDynamics<T>)(object)SecondOrderFactory.Vector3(frequency, zeta, response, (Vector3)(object)initialValue);
        } else if (typeof(T) == typeof(Vector4)) {
            func = (SecondOrderDynamics<T>)(object)SecondOrderFactory.Vector4(frequency, zeta, response, (Vector4)(object)initialValue);
        } else if (typeof(T) == typeof(Color)) {
            func = (SecondOrderDynamics<T>)(object)SecondOrderFactory.Color(frequency, zeta, response, (Color)(object)initialValue);
        } else {
            throw new System.Exception ($"DynamicMovement does not support type {typeof(T)}");
        }

            
    }

}
