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
    [SerializeField] private LayerMask layer;                                               // Ư�� ���̾� üũ�� ���� Layer
    private GameObject target;                                                              // ���� ������ ������Ʈ�� ��Ƶα� ���� GameObjcet
    private Vector2 CenterPosition = new Vector2(Screen.width / 2, Screen.height / 2);      // ȭ���� �߽����� ��Ƶα� ���� Vector2
    private int Hour_angle;                                                                 // ������ ��ħ�� ������ �����Ͽ� ����üũ�� �ϱ� ���� int
    private int Minute_angle;                                                               // ������ ��ħ�� ������ �����Ͽ� ����üũ�� �ϱ� ���� int
    const int hourhand_Clear = 60;                                                       // ������ ����üũ�� ���� ������ ��Ƶ� readonly int
    const int minutehand_Clear = 180;                                                    // ������ ����üũ�� ���� ������ ��Ƶ� readonly int
    private float previousQuantizedAngle = 0f;                                              // ������ ȸ������ ��Ƶα� ���� float

    /// <summary>
    /// ���� Ŭ�������� ��ӵ� ���� Press Event
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
    /// ���� Ŭ�������� ��ӵ� PuzzlePoition Event
    /// </summary>
    /// <param name="context"></param>
    public override void OnPuzzlePosition(InputAction.CallbackContext context)
    {
        if (target == null) return;

        Vector2 currentTouchPosition = context.ReadValue<Vector2>();

        float angleInRadians = CalculateRotationAngle(CenterPosition, currentTouchPosition);

        float quantizedAngle = Mathf.Round(angleInRadians / 30.0f) * 30.0f;

        // ȸ�� ���� ������ �ٸ� ��쿡�� ȸ��
        if (!Mathf.Approximately(quantizedAngle, previousQuantizedAngle))
        {
            target.transform.localRotation = Quaternion.Euler(0f, -quantizedAngle, 0f);

            AudioManager.Instance.PlaySFX("SFX_MoveClockHand");

            previousQuantizedAngle = quantizedAngle;
        }
    }

    /// <summary>
    /// hourhand_angle�� minutehand_angle�� �����ص� ������ ���Ͽ� ������ üũ�ϴ� �޼ҵ�
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
    /// ���� ��ġ ��ġ�� �������� ȸ�� ������ ����ϴ� �޼ҵ�
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    private float CalculateRotationAngle(Vector2 start, Vector2 end)
    {
        return Mathf.Atan2(end.x - start.x, end.y - start.y) * Mathf.Rad2Deg;
    }
}
