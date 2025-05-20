using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class Puzzle_Square : Puzzle
{
    [SerializeField] private Puzzle_Check[] puzzlecheck;    // 퍼즐의 정답을 체크하기위해 만든 배열
    [SerializeField] private LayerMask layer;               // 특정 레이어 체크를 위한 Layer
    [SerializeField] private LayerMask layer_P;             // 특정 레이어 체크를 위한 Layer
    [SerializeField] private GameObject clear;              // 정답 체크를하는 오브젝트를 담아두는 GameObject
    [SerializeField] private Material Light;                // 마테리얼의 속성을 변경 하기 위한 Material

    private GameObject target;                              // 터치한 오브젝트를 담아두는 GameObject
    private Vector3 SavePosition;                           // 터치 시점의 포지션을 저장하기위한 Vector3

    private void Start()
    {
        Light.SetColor("_EmissionColor", Color.red);
    }

    /// <summary>
    /// 퍼즐 클래스에서 상속된 퍼즐 Press Event
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
    /// 정답을 체크하는 메소드
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
    /// 퍼즐 클래스에서 상속된 퍼즐 Position Event
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
