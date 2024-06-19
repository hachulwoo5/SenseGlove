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
        // 초기 y-rotation 값을 저장합니다.
        previousYRotation = transform. localEulerAngles. y;

        // Rigidbody 컴포넌트를 가져옵니다.
        rb = GetComponent<Rigidbody> ( );

        // 처음에는 Rigidbody의 position constraints를 모두 고정합니다.

        // 초기 위치를 설정합니다.
        Vector3 position = transform. localPosition;
        position. y = minYPosition;
        transform. localPosition = position;
    }

    void Update ( )
    {
        // 현재 y-rotation 값을 가져옵니다.
        float currentYRotation = transform. localEulerAngles. y;

        // 각도 변화량을 계산합니다.
        float deltaRotation = currentYRotation - previousYRotation;

        // 각도 변화량을 정규화하여 0 ~ 360도 범위에서 계산합니다.
        if ( deltaRotation > 180f )
            deltaRotation -= 360f;
        else if ( deltaRotation < -180f )
            deltaRotation += 360f;

        // 누적 회전 값을 업데이트합니다.
        accumulatedRotation += deltaRotation;

        // 누적 회전 값을 기준으로 위치를 조정합니다.
        AdjustPosition ( accumulatedRotation );

        // 현재 y-rotation 값을 이전 값으로 업데이트합니다.
        previousYRotation = currentYRotation;
    }

    void AdjustPosition ( float accumulatedRotation )
    {
        // 누적 회전 값을 비례적으로 y-position 값으로 변환합니다.
        float positionIncrement = ( accumulatedRotation / rotationThreshold ) * positionIncrementPerRotation;
        Vector3 position = transform. localPosition;
        position. y = Mathf. Clamp ( minYPosition + positionIncrement , minYPosition , maxYPosition );
        transform. localPosition = position;

        // y-position 값이 0.75f에 도달했을 때 Rigidbody position constraints를 해제합니다.
        if ( position. y >= maxYPosition )
        {
            rb. constraints = RigidbodyConstraints. FreezeRotation;
        }
    }
}
