using System. Collections;
using System. Collections. Generic;
using UnityEngine;

public class BottleCap : MonoBehaviour
{
    private float previousYRotation;
    private float accumulatedRotation;
    private const float positionIncrementPerRotation = 0.6f;
    private const float rotationThreshold = 360f;
    private const float minYPosition = -1.1129f;
    private const float maxYPosition = 0.75f;
    private Rigidbody rb;

    void Start ( )
    {
        // �ʱ� y-rotation ���� �����մϴ�.
        previousYRotation = transform. localEulerAngles. y;

        // Rigidbody ������Ʈ�� �����ɴϴ�.
        rb = GetComponent<Rigidbody> ( );

        // ó������ Rigidbody�� position constraints�� ��� �����մϴ�.

        // �ʱ� ��ġ�� �����մϴ�.
        Vector3 position = transform. localPosition;
        position. y = minYPosition;
        transform. localPosition = position;
    }

    void Update ( )
    {
        // ���� y-rotation ���� �����ɴϴ�.
        float currentYRotation = transform. localEulerAngles. y;

        // ���� ��ȭ���� ����մϴ�.
        float deltaRotation = currentYRotation - previousYRotation;

        // ���� ��ȭ���� ����ȭ�Ͽ� 0 ~ 360�� �������� ����մϴ�.
        if ( deltaRotation > 180f )
            deltaRotation -= 360f;
        else if ( deltaRotation < -180f )
            deltaRotation += 360f;

        // ���� ȸ�� ���� ������Ʈ�մϴ�.
        accumulatedRotation += deltaRotation;

        // ���� ȸ�� ���� �������� ��ġ�� �����մϴ�.
        AdjustPosition ( accumulatedRotation );

        // ���� y-rotation ���� ���� ������ ������Ʈ�մϴ�.
        previousYRotation = currentYRotation;
    }

    void AdjustPosition ( float accumulatedRotation )
    {
        // ���� ȸ�� ���� ��������� y-position ������ ��ȯ�մϴ�.
        float positionIncrement = ( accumulatedRotation / rotationThreshold ) * positionIncrementPerRotation;
        Vector3 position = transform. localPosition;
        position. y = Mathf. Clamp ( minYPosition + positionIncrement , minYPosition , maxYPosition );
        transform. localPosition = position;

        // y-position ���� 0.75f�� �������� �� Rigidbody position constraints�� �����մϴ�.
        if ( position. y >= maxYPosition )
        {
            rb. constraints = RigidbodyConstraints. FreezeRotation;
        }
    }
}
