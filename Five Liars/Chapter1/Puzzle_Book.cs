using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class Puzzle_Book : Puzzle
{
    [SerializeField] private Puzzle_Check[] puzzlecheck;    // �ڽ� ��ü�� String�� üũ�ϱ� ���� �迭
    [SerializeField] private GameObject[] BookPositions;    // å���� �⺻ ��ġ�� ������� ���� ������Ʈ �迭
    [SerializeField] private LayerMask layer;               // Ư�� ���̾� üũ�� ���� Layer
    private GameObject target;                              // Ŭ������ ������Ʈ�� ��Ƶ� GameObject
    private GameObject NearPositions;                       // target�� ������Ʈ���� ����� �������� üũ�ϱ����� GameObject
    private bool ObjectMove;                                // ���� ������Ʈ�� �̵����϶� ������ġ�� �������� Bool

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
                if (!ObjectMove)
                {
                    target = hit.transform.gameObject;
                    target.transform.SetParent(transform);
                    ObjectMove = true;

                    float targetObjectY = target.transform.position.y;

                    Vector3 SetPosition = hit.point + (Camera.main.transform.forward * 0.1f);

                    SetPosition.y = targetObjectY;

                    target.transform.position = SetPosition;
                }
            }
        }
        
        if(context.canceled)
        {
            if (target == null) return;

            SetObjectToPosition(target, NearPositions);
            ClearCheck();
            target = null;
        }
    }

    /// <summary>
    /// ���� Ŭ�������� ��ӵ� PuzzlePoition Event
    /// </summary>
    /// <param name="context"></param>
    public override void OnPuzzlePosition(InputAction.CallbackContext context)
    {
        if (target == null) return;

        Vector3 touchPosition = context.ReadValue<Vector2>();

        float z = Camera.main.WorldToScreenPoint(target.transform.position).z;

        Vector3 screenPosition = new Vector3(touchPosition.x, touchPosition.y, z);

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        float clampedZ = Mathf.Clamp(worldPosition.z, 190f, 193.2f);

        target.transform.position = new Vector3(worldPosition.x, target.transform.position.y, clampedZ);

        MoveToClosestPosition(target);
    }

    /// <summary>
    /// ������Ʈ�� �̵���Ű������ ���� ����� �Ÿ��� ������Ʈ�� üũ�ϱ����� �޼ҵ�
    /// </summary>
    /// <param name="obj"></param>
    private void MoveToClosestPosition(GameObject obj)
    {
        if (BookPositions.Length == 0) return;

        float shortestDistance = float.MaxValue;

        foreach (GameObject position in BookPositions)
        {
            float distance = Vector3.Distance(obj.transform.position, position.transform.position);

            if (distance < shortestDistance)
            {
                NearPositions = position;
                shortestDistance = distance;
            }
        }

        int emptyIndex = FindEmptyPosition();
        int targetIndex = Array.IndexOf(BookPositions, NearPositions);

        if (emptyIndex == -1) return;

        ShiftPositions(emptyIndex, targetIndex);
    }

    /// <summary>
    /// BookPositions�� �ִ� ���������Ʈ�� �ڽİ�ü�� ���� ������Ʈ�� int���� ��ȯ�ϴ� �޼ҵ�
    /// </summary>
    /// <returns></returns>
    private int FindEmptyPosition()
    {
        for (int i = 0; i < BookPositions.Length; i++)
        {
            if (BookPositions[i].transform.childCount == 0) return i;
        }
        return -1;
    }

    /// <summary>
    /// ���� �ڽİ�ü�� ���� ������Ʈ�� int���� ����� �Ÿ��� ������Ʈ�� int���� ���Ͽ� �����̴� ������ ���ϴ� �޼ҵ�
    /// </summary>
    /// <param name="emptyIndex">�ڽİ�ü�� ���� ������Ʈ�� int��</param>
    /// <param name="targetIndex">�������� �� ��� ������Ʈ�� int��</param>
    private void ShiftPositions(int emptyIndex, int targetIndex)
    {
        if (emptyIndex == targetIndex) return;

        if (emptyIndex <= targetIndex)
        {
            ShiftLeft(targetIndex, emptyIndex);
        }
        else
        {
            ShiftRight(targetIndex, emptyIndex);
        }

        AudioManager.Instance.PlaySFX("SFX_Puzzle_Book_Put_Out");
    }

    /// <summary>
    /// ������ startIndex���� �ڽ� ��ü�� ���� ��ġ�� emptyIndex���� ������ �������� �ڽ� ��ü�� �� ĭ�� �̵���Ű�� �޼ҵ�
    /// </summary>
    /// <param name="startIndex"></param>
    /// <param name="emptyIndex"></param>
    private void ShiftRight(int startIndex, int emptyIndex)
    {
        for (int i = emptyIndex - 1; i >= startIndex; i--)
        {
            MoveChildToPosition(i, i + 1);
        }
    }

    /// <summary>
    /// ������ startIndex���� �ڽ� ��ü�� ���� ��ġ�� emptyIndex���� ���� �������� �ڽ� ��ü�� �� ĭ�� �̵���Ű�� �޼ҵ�
    /// </summary>
    /// <param name="startIndex"></param>
    /// <param name="emptyIndex"></param>
    private void ShiftLeft(int startIndex, int emptyIndex)
    {
        for (int i = emptyIndex + 1; i <= startIndex; i++)
        {
            MoveChildToPosition(i, i - 1);
        }
    }

    /// <summary>
    /// fromIndex�� ��ġ���ִ� �ڽ� ��ü�� toIndex�� ��ġ�� ���� �޼ҵ�
    /// </summary>
    /// <param name="fromIndex"></param>
    /// <param name="toIndex"></param>
    private void MoveChildToPosition(int fromIndex, int toIndex)
    {
        if (BookPositions[fromIndex].transform.childCount > 0)
        {
            GameObject child = BookPositions[fromIndex].transform.GetChild(0).gameObject;
            SetObjectToPosition(child, BookPositions[toIndex]);
        }
    }

    /// <summary>
    /// obj�� ������Ʈ ��ü�� position�� �ڽ� ��ü�� �����ϴ� �޼ҵ�
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="position"></param>
    private void SetObjectToPosition(GameObject obj, GameObject position)
    {
        if (position.transform.childCount == 0)
        {
            obj.transform.SetParent(position.transform);
            obj.transform.DOLocalMove(Vector3.zero, 0.25f).SetEase(Ease.InOutQuad).OnComplete(() => { ObjectMove = false; });
        }
        else
        {
            obj.transform.SetParent(BookPositions[FindEmptyPosition()].transform);
            obj.transform.DOLocalMove(Vector3.zero, 0.25f).SetEase(Ease.InOutQuad).OnComplete(() => { ObjectMove = false; });
        }
    }

    /// <summary>
    /// ������ ��ġ�� String������ �̸��� ��ġ�ϴ� ������Ʈ�� ��ġ�Ǿ����� üũ�ϴ� �޼ҵ�
    /// </summary>
    private void ClearCheck()
    {
        foreach (Puzzle_Check BookList in puzzlecheck)
        {
            if (!BookList.IsPuzzleNameCheck())
            {
                return;
            }
        }
        AudioManager.Instance.PlaySFX("SFX_Complete_Book_Puzzle");
        PuzzleClear();
    }
}
