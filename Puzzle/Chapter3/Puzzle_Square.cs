using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class Puzzle_Square : Puzzle
{
    [SerializeField] private Puzzle_Check[] puzzlecheck;    // ������ ������ üũ�ϱ����� ���� �迭
    [SerializeField] private LayerMask layer;               // Ư�� ���̾� üũ�� ���� Layer
    [SerializeField] private LayerMask layer_P;             // Ư�� ���̾� üũ�� ���� Layer
    [SerializeField] private GameObject clear;              // ���� üũ���ϴ� ������Ʈ�� ��Ƶδ� GameObject
    [SerializeField] private Material Light;                // ���׸����� �Ӽ��� ���� �ϱ� ���� Material

    private GameObject target;                              // ��ġ�� ������Ʈ�� ��Ƶδ� GameObject
    private Vector3 SavePosition;                           // ��ġ ������ �������� �����ϱ����� Vector3

    private void Start()
    {
        Light.SetColor("_EmissionColor", Color.red);
    }

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
                if(hit.transform.gameObject == clear)
                {
                    clear.transform.DOLocalMoveX(-0.189f, 0.3f).OnComplete(ClearCheck);
                    return;
                }

                if (hit.transform.CompareTag("PuzzleObject"))
                {
                    SavePosition = hit.transform.position;
                    target = hit.transform.gameObject;
                    target.transform.SetParent(transform);
                }
            }
        }

        if(context.canceled)
        {
            if (target == null) return;

            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

            Ray ray = Camera.main.ScreenPointToRay(touchPosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layer))
            {
                if (hit.transform.childCount == 0)
                {
                    target.transform.parent = hit.transform;
                    target.transform.localPosition = Vector3.zero;
                }
            }
            else
            {
                target.transform.position = SavePosition;
                SavePosition = Vector3.zero;
            }
            AudioManager.Instance.PlaySFX("SFX_PutinRhombus");
            if (target != null) target = null;

        }
    }

    /// <summary>
    /// ������ üũ�ϴ� �޼ҵ�
    /// </summary>
    private void ClearCheck()
    {
        AudioManager.Instance.PlaySFX("SFX_PushLever");

        foreach (Puzzle_Check square in puzzlecheck)
        {
            if (!square.IsPuzzleNameCheck())
            {
                clear.transform.DOLocalMoveX(0.167f, 0.3f);
                return;
            }
        }

        Light.SetColor("_EmissionColor", Color.green);
        PuzzleClear();
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
        
        target.transform.position = worldPosition;
    }
}
