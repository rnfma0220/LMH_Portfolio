using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using Cinemachine;
using System;

public class Puzzle_Clock : Puzzle
{
    [SerializeField] private LayerMask layer;                                               // 특정 레이어 체크를 위한 Layer
    private GameObject target;                                                              // 현재 선택한 오브젝트를 담아두기 위한 GameObjcet
    private Vector2 CenterPosition = new Vector2(Screen.width / 2, Screen.height / 2);      // 화면의 중심점을 담아두기 위한 Vector2
    private int Hour_angle;                                                                 // 움직인 시침의 각도를 저장하여 정답체크를 하기 위한 int
    private int Minute_angle;                                                               // 움직인 분침의 각도를 저장하여 정답체크를 하기 위한 int
    const int hourhand_Clear = 60;                                                       // 퍼즐의 정답체크를 위해 정답을 담아둔 readonly int
    const int minutehand_Clear = 180;                                                    // 퍼즐의 정답체크를 위해 정답을 담아둔 readonly int
    private float previousQuantizedAngle = 0f;                                              // 기존의 회전값을 담아두기 위한 float

    /// <summary>
    /// 퍼즐 클래스에서 상속된 퍼즐 Press Event
    /// </summary>
    /// <param name="context"></param>
    public override void OnPuzzlePress(InputAction.CallbackContext context)
    {
        if (!CheckTouchEnable()) return;

        if (context.performed)
        {
            if (target != null) return;

            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(touchPosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layer))
            {
                target = hit.transform.gameObject;
            }
        }
        if (context.canceled)
        {
            if (target == null) return;

            if (target.name == "Hour")
            {
                Hour_angle = (int)target.transform.localEulerAngles.y;
            }
            else
            {
                Minute_angle = (int)target.transform.localEulerAngles.y;
            }
            target = null;
            ClearCheck();
        }
    }

    /// <summary>
    /// 퍼즐 클래스에서 상속된 PuzzlePoition Event
    /// </summary>
    /// <param name="context"></param>
    public override void OnPuzzlePosition(InputAction.CallbackContext context)
    {
        if (target == null) return;

        Vector2 currentTouchPosition = context.ReadValue<Vector2>();

        float angleInRadians = CalculateRotationAngle(CenterPosition, currentTouchPosition);

        float quantizedAngle = Mathf.Round(angleInRadians / 30.0f) * 30.0f;

        // 회전 값이 이전과 다를 경우에만 회전
        if (!Mathf.Approximately(quantizedAngle, previousQuantizedAngle))
        {
            target.transform.localRotation = Quaternion.Euler(0f, -quantizedAngle, 0f);

            AudioManager.Instance.PlaySFX("SFX_MoveClockHand");

            previousQuantizedAngle = quantizedAngle;
        }
    }

    /// <summary>
    /// hourhand_angle과 minutehand_angle에 저장해둔 각도를 비교하여 정답을 체크하는 메소드
    /// </summary>
    private void ClearCheck() 
    {
        if (Hour_angle == hourhand_Clear && Minute_angle == minutehand_Clear)
        {
            AudioManager.Instance.PlaySFX("SFX_ChurchBell");
            PuzzleClear();
        }
    }

    /// <summary>
    /// 현재 터치 위치를 기준으로 회전 각도를 계산하는 메소드
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    private float CalculateRotationAngle(Vector2 start, Vector2 end)
    {
        return Mathf.Atan2(end.x - start.x, end.y - start.y) * Mathf.Rad2Deg;
    }
}
