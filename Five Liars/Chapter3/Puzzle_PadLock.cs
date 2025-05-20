using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Puzzle_PadLock : Puzzle
{
    [SerializeField] private LayerMask layer;                                       // 특정 레이어 체크를 위한 Layer
    private GameObject target;                                                      // 터치한 오브젝트를 담아두는 GameObject
    private int[] puzzlevalue = new int[5];                                         // 각도를 입력해 정답을 체크하기위한 int 배열
    private int[] puzzleclear = { 31, 231, 191, 71, 231 };                          // 정답을 체크하기 위한 int 배열
    private int[] validAngles = { 31, 71, 111, 151, 191, 231, 271, 311, 351 };      // 멈추는 특정 각도를 정해놓기 위한 int 배열
    private bool isDragging;                                                        // 현재 드래그 중인지 체크하기 위한 bool
    private Vector2 previousTouchPosition;                                          // 터치 이전 위치를 저장하여 이동 거리 계산에 사용하는 Vector2
    private float currentAngle = 0f;                                                // 현재 회전 각도를 저장하는 float
    private float rotateDuration = 0.4f;                                            // 회전 속도를 결정하는 float

    private void Start()
    {
        // 퍼즐의 기본값인 31도 입력을 위한 for문
        for (int i = 0; i < puzzlevalue.Length; i++)
        {
            puzzlevalue[i] = 31;
        }
    }

    /// <summary>
    /// 퍼즐 클래스에서 상속된 퍼즐 Position Event
    /// </summary>
    /// <param name="context"></param>
    public override void OnPuzzlePosition(InputAction.CallbackContext context)
    {
        if (!CheckTouchEnable()) return;
 
        if (target == null) return;

        Vector2 currentTouchPosition = context.ReadValue<Vector2>();

        if (!isDragging)
        {
            isDragging = true;
            previousTouchPosition = currentTouchPosition;
            currentAngle = target.transform.localEulerAngles.z;
            return;
        }

        Vector2 delta = currentTouchPosition - previousTouchPosition;

        currentAngle += delta.y * rotateDuration;

        target.transform.localRotation = Quaternion.Euler(0f, -180f, currentAngle);

        previousTouchPosition = currentTouchPosition;
    }

    /// <summary>
    /// 퍼즐 클래스에서 상속된 퍼즐 Press Event
    /// </summary>
    /// <param name="context"></param>
    public override void OnPuzzlePress(InputAction.CallbackContext context)
    {
        if (!CheckTouchEnable()) return;

        if (context.started)
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

            RotateDial();
            target = null;
            isDragging = false;

            for (int i = 0; i < puzzlevalue.Length; i++)
            {
                if (puzzlevalue[i] != puzzleclear[i])
                {
                    return;
                }
            }
            
            PuzzleClear();
        }
    }

    /// <summary>
    /// 타겟의 오브젝트를 입력해둔 각도에서 가장 가까운 각도로 변경하고 puzzlevalue에 입력하는 메소드
    /// </summary>
    private void RotateDial()
    {
        int currentZAngle = Mathf.RoundToInt(target.transform.localEulerAngles.z);

        int closestAngle = validAngles.OrderBy(angle => Mathf.Abs(currentZAngle - angle)).First();

        currentAngle = closestAngle;

        target.transform.localRotation = Quaternion.Euler(0f, -180f, currentAngle);

        switch (target.name)
        {
            case "0":
                puzzlevalue[0] = Mathf.RoundToInt(target.transform.localEulerAngles.z);
                break;
            case "1":
                puzzlevalue[1] = Mathf.RoundToInt(target.transform.localEulerAngles.z);
                break;
            case "2":
                puzzlevalue[2] = Mathf.RoundToInt(target.transform.localEulerAngles.z);
                break;
            case "3":
                puzzlevalue[3] = Mathf.RoundToInt(target.transform.localEulerAngles.z);
                break;
            case "4":
                puzzlevalue[4] = Mathf.RoundToInt(target.transform.localEulerAngles.z);
                break;
        }
    }
}

