using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class Puzzle_Book : Puzzle
{
    [SerializeField] private Puzzle_Check[] puzzlecheck;    // 자식 객체의 String를 체크하기 위한 배열
    [SerializeField] private GameObject[] BookPositions;    // 책들의 기본 위치를 잡기위한 게임 오브젝트 배열
    [SerializeField] private LayerMask layer;               // 특정 레이어 체크를 위한 Layer
    private GameObject target;                              // 클릭중인 오브젝트를 담아둘 GameObject
    private GameObject NearPositions;                       // target의 오브젝트에서 가까운 포지션을 체크하기위한 GameObject
    private bool ObjectMove;                                // 현재 오브젝트가 이동중일때 이중터치를 막기위한 Bool

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
    /// 퍼즐 클래스에서 상속된 PuzzlePoition Event
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
    /// 오브젝트를 이동시키기위해 가장 가까운 거리의 오브젝트를 체크하기위한 메소드
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
    /// BookPositions에 있는 슬룻오브젝트의 자식객체가 없는 오브젝트의 int값을 반환하는 메소드
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
    /// 현재 자식객체가 없는 오브젝트의 int값과 가까운 거리의 오브젝트의 int값을 비교하여 움직이는 방향을 정하는 메소드
    /// </summary>
    /// <param name="emptyIndex">자식객체가 없는 오브젝트의 int값</param>
    /// <param name="targetIndex">움직여야 할 대상 오브젝트의 int값</param>
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
    /// 지정된 startIndex부터 자식 객체가 없는 위치인 emptyIndex까지 오른쪽 방향으로 자식 객체를 한 칸씩 이동시키는 메소드
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
    /// 지정된 startIndex부터 자식 객체가 없는 위치인 emptyIndex까지 왼쪽 방향으로 자식 객체를 한 칸씩 이동시키는 메소드
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
    /// fromIndex의 위치에있는 자식 객체를 toIndex의 위치로 움기는 메소드
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
    /// obj의 오브젝트 객체를 position의 자식 객체로 설정하는 메소드
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
    /// 지정된 위치에 String값으로 이름과 일치하는 오브젝트가 배치되었는지 체크하는 메소드
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
