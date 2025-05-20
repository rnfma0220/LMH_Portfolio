using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class Puzzle_Chess : Puzzle
{
    [SerializeField] private Puzzle_Check[] puzzlecheck;        // 자식 객체의 String를 체크하기 위한 배열
    [SerializeField] private GameObject[] ResetPoint_White;     // 체스말을 체스판 밖으로 뺐을때의 위치를 위한 GameObject 배열
    [SerializeField] private GameObject[] ResetPoint_Black;     // 체스말을 체스판 밖으로 뺐을때의 위치를 위한 GameObject 배열
    [SerializeField] private LayerMask layer;                   // 특정 레이어 체크를 위한 Layer
    [SerializeField] private LayerMask layer_P;                 // 특정 레이어 체크를 위한 Layer
    private GameObject target;                                  // 현재 들고있는 오브젝트를 담아두는 GameObject

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

        target.transform.position = new Vector3(worldPosition.x, target.transform.position.y, worldPosition.z);
    }

    /// <summary>
    /// 지정된 위치에 String값으로 이름과 일치하는 오브젝트가 배치되었는지 체크하는 메소드
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
