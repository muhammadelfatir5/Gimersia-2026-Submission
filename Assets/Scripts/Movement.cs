using Unity.Mathematics;
using UnityEditor.Callbacks;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("P1 Input")]
    public string P1HorizontalAxis = "P1_Horizontal";
    public string P1VerticalAxis = "P1_Vertical";
    public Transform P1Transform;

    [Header("P2 Input")]
    public string P2HorizontalAxis = "P2_Horizontal";
    public string P2VerticalAxis = "P2_Vertical";
    public Transform P2Transform;

    public float moveSpeed = 5f;
    public float turnSpeed = 120f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    void FixedUpdate()
    {
        Vector3 p1Input = new Vector3(Input.GetAxis(P1HorizontalAxis), 0f, Input.GetAxis(P1VerticalAxis));
        Vector3 p2Input = new Vector3(Input.GetAxis(P2HorizontalAxis), 0f, Input.GetAxis(P2VerticalAxis));

        if (p1Input.sqrMagnitude > 1f) p1Input.Normalize();
        if (p2Input.sqrMagnitude > 1f) p2Input.Normalize();

        Vector3 combinedMove = (p1Input + p2Input) * 0.5f;

        Vector3 p1WorldOffset = (P1Transform.position - rb.position);
        Vector3 p2WorldOffset = (P2Transform.position - rb.position);
        p1WorldOffset.y = 0f;
        p2WorldOffset.y = 0f;

        float torque1 = Vector3.Cross(p1WorldOffset.normalized, p1Input).y;
        float torque2 = Vector3.Cross(p2WorldOffset.normalized, p2Input).y;
        float combinedTurn = (torque1 + torque2) * 0.5f;

        Vector3 targetPos = rb.position + (combinedMove * moveSpeed * Time.fixedDeltaTime);
        Quaternion targetRot = rb.rotation * Quaternion.Euler(0f, combinedTurn * turnSpeed * Time.fixedDeltaTime, 0f);

        rb.MovePosition(targetPos);
        rb.MoveRotation(targetRot);
    }
}
