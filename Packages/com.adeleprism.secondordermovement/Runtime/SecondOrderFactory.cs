using UnityEngine;

public static class SecondOrderFactory
{
    //Define how each operator works on different types
    public static SecondOrderDynamics<float> Float(float f, float z, float r, float t0 = 0f)
    => new SecondOrderDynamics<float>(f, z, r, t0,
        (a, b) => a + b,
        (a, b) => a - b,
        (a, b) => a * b,
        (a, b) => a / b
        );

    public static SecondOrderDynamics<Vector2> Vector2(float f, float z, float r, Vector2 t0)
    => new SecondOrderDynamics<Vector2>(f, z, r, t0,
        (a, b) => a + b,
        (a, b) => a - b,
        (a, b) => a * b,
        (a, b) => a / b
        );

    public static SecondOrderDynamics<Vector3> Vector3(float f, float z, float r, Vector3 t0)
    => new SecondOrderDynamics<Vector3>(f, z, r, t0,
        (a, b) => a + b,
        (a, b) => a - b,
        (a, b) => a * b,
        (a, b) => a / b
        );

    public static SecondOrderDynamics<Color> Color(float f, float z, float r, Color t0)
    => new SecondOrderDynamics<Color>(f, z, r, t0,
        (a, b) => a + b,
        (a, b) => a - b,
        (a, b) => a * b,
        (a, b) => a / b
        );

    public static SecondOrderDynamics<Vector4> Vector4(float f, float z, float r, Vector4 t0)
    => new SecondOrderDynamics<Vector4>(f, z, r, t0,
        (a, b) => a + b,
        (a, b) => a - b,
        (a, b) => a * b,
        (a, b) => a / b
        );
}
