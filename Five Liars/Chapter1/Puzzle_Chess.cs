using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class Puzzle_Chess : Puzzle
{
    [SerializeField] private Puzzle_Check[] puzzlecheck;        // �ڽ� ��ü�� String�� üũ�ϱ� ���� �迭
    [SerializeField] private GameObject[] ResetPoint_White;     // ü������ ü���� ������ �������� ��ġ�� ���� GameObject �迭
    [SerializeField] private GameObject[] ResetPoint_Black;     // ü������ ü���� ������ �������� ��ġ�� ���� GameObject �迭
    [SerializeField] private LayerMask layer;                   // Ư�� ���̾� üũ�� ���� Layer
    [SerializeField] private LayerMask layer_P;                 // Ư�� ���̾� üũ�� ���� Layer
    private GameObject target;                                  // ���� ����ִ� ������Ʈ�� ��Ƶδ� GameObject

    /// <summary>
    /// ���� Ŭ�������� ��ӵ� ���� Press Event
    /// </summary>
    /// <param name="context"></param>
    public override void OnPuzzlePress(InputAction.CallbackContext context)
    {
        if (!CheckTouchEnable()) return;

        if (context.performed)
        {
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

            Ray ray = Camera.main.ScreenPointToRay(touchPosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layer_P))
            {
                if (hit.transform.gameObject.CompareTag("PuzzleObject"))
                {
                    target = hit.transform.gameObject;
                }
            }
        }
        if (context.canceled)
        {
            if (target == null) return;

            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

            Ray ray = Camera.main.ScreenPointToRay(touchPosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layer))
            {
                if (hit.transform.CompareTag("Notinstallation"))
                {
                    if (hit.transform.name.Equals("WhiteZone"))
                    {
                        foreach (GameObject resetPoint in ResetPoint_White)
                        {
                            if (resetPoint.transform.childCount == 0)
                            {
                                target.transform.parent = resetPoint.transform;
                                target.transform.localPosition = new Vector3(0f, 0f, 0.003f);
                            }
                            else
                            {
                                target.transform.localPosition = new Vector3(0f, 0f, 0.003f);
                            }
                        }
                    }
                    else
                    {
                        foreach (GameObject resetPoint in ResetPoint_Black)
                        {
                            if (resetPoint.transform.childCount == 0)
                            {
                                target.transform.parent = resetPoint.transform;
                                target.transform.localPosition = new Vector3(0f, 0f, 0.003f);
                            }
                            else
                            {
                                target.transform.localPosition = new Vector3(0f, 0f, 0.003f);
                            }
                        }
                    }
                }
                else
                {
                    if (hit.transform.childCount > 0)
                    {
                        target.transform.localPosition = new Vector3(0f, 0f, 0.003f);
                    }
                    else
                    {
                        target.transform.parent = hit.transform;
                        target.transform.localPosition = new Vector3(0f, 0f, 0.003f);
                    }
                }
            }
            else
            {
                target.transform.localPosition = new Vector3(0f, 0f, 0.003f);
            }

            if (target != null) target = null;

            AudioManager.Instance.PlaySFX("SFX_MoveChess");
            ClearCheck();
        }
    }

    /// <summary>
    /// ���� Ŭ�������� ��ӵ� ���� Position Event
    /// </summary>
    /// <param name="context"></param>
    public override void OnPuzzlePosition(InputAction.CallbackContext context)
    {
        if (target == null) return;

        Vector3 touchPosition = context.ReadValue<Vector2>();

        float z = Camera.main.WorldToScreenPoint(target.transform.position).z;

        Vector3 screenPosition = new Vector3(touchPosition.x, touchPosition.y, z);

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        target.transform.position = new Vector3(worldPosition.x, target.transform.position.y, worldPosition.z);
    }

    /// <summary>
    /// ������ ��ġ�� String������ �̸��� ��ġ�ϴ� ������Ʈ�� ��ġ�Ǿ����� üũ�ϴ� �޼ҵ�
    /// </summary>
    private void ClearCheck()
    {
        foreach (Puzzle_Check chessboards in puzzlecheck)
        {
            if (!chessboards.IsPuzzleNameCheck())
            {
                return;
            }
        }
        PuzzleClear();
    }
}
