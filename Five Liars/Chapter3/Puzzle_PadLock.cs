using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Puzzle_PadLock : Puzzle
{
    [SerializeField] private LayerMask layer;                                       // Ư�� ���̾� üũ�� ���� Layer
    private GameObject target;                                                      // ��ġ�� ������Ʈ�� ��Ƶδ� GameObject
    private int[] puzzlevalue = new int[5];                                         // ������ �Է��� ������ üũ�ϱ����� int �迭
    private int[] puzzleclear = { 31, 231, 191, 71, 231 };                          // ������ üũ�ϱ� ���� int �迭
    private int[] validAngles = { 31, 71, 111, 151, 191, 231, 271, 311, 351 };      // ���ߴ� Ư�� ������ ���س��� ���� int �迭
    private bool isDragging;                                                        // ���� �巡�� ������ üũ�ϱ� ���� bool
    private Vector2 previousTouchPosition;                                          // ��ġ ���� ��ġ�� �����Ͽ� �̵� �Ÿ� ��꿡 ����ϴ� Vector2
    private float currentAngle = 0f;                                                // ���� ȸ�� ������ �����ϴ� float
    private float rotateDuration = 0.4f;                                            // ȸ�� �ӵ��� �����ϴ� float

    private void Start()
    {
        // ������ �⺻���� 31�� �Է��� ���� for��
        for (int i = 0; i < puzzlevalue.Length; i++)
        {
            puzzlevalue[i] = 31;
        }
    }

    /// <summary>
    /// ���� Ŭ�������� ��ӵ� ���� Position Event
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
    /// ���� Ŭ�������� ��ӵ� ���� Press Event
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
    /// Ÿ���� ������Ʈ�� �Է��ص� �������� ���� ����� ������ �����ϰ� puzzlevalue�� �Է��ϴ� �޼ҵ�
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

