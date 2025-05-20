using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Puzzle_LightHint : Puzzle
{
    [SerializeField] private Material Lignt_L;                          // 마테리얼의 속성을 변경 하기 위한 Material
    [SerializeField] private Material Lignt_R;                          // 마테리얼의 속성을 변경 하기 위한 Material
    [SerializeField] private LayerMask layer;                           // 특정 레이어 체크를 위한 Layer

    private bool IsHint;                                                // 현재 힌트가 재생중인지 체크하기 위한 bool
    private GameObject HintButton;                                      // 터치된 오브젝트를 담아두기 위한 GameObject
    private Vector3 UpPositions = new Vector3(0.137f, 0f, 0.016f);      // 특정 오브젝트를 움직일때 사용하기 위해 포지션을 담아둔 Vector3
    private Vector3 DownPositions = new Vector3(0.135f, 0f, 0.008f);    // 특정 오브젝트를 움직일때 사용하기 위해 포지션을 담아둔 Vector3

    /// <summary>
    /// 퍼즐 클래스에서 상속된 퍼즐 Press Event
    /// </summary>
    /// <param name="context"></param>
    public override void OnPuzzlePress(InputAction.CallbackContext context)
    {
        if (!CheckTouchEnable()) return;

        if (context.performed)
        {
            if (HintButton != null) return;

            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(touchPosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layer))
            {
                HintButton = hit.transform.gameObject;
            }
        }

        if (context.canceled)
        {
            if (HintButton != null && !IsHint)
            {
                IsHint = true;
                StartCoroutine(HintStart());
            }
        }
    }

    /// <summary>
    /// 퍼즐 클래스에서 상속된 퍼즐 Position Event
    /// </summary>
    /// <param name="context"></param>
    public override void OnPuzzlePosition(InputAction.CallbackContext context)
    {
        return;
    }

    /// <summary>
    /// 힌트를 순서대로 재생하는 코루틴 메소드
    /// </summary>
    /// <returns></returns>
    private IEnumerator HintStart()
    {
        HintButton.transform.DOLocalMove(DownPositions, 0.5f).SetEase(Ease.InOutQuad);
        AudioManager.Instance.PlaySFX("SFX_LightSwitchOn");
        yield return new WaitForSeconds(1f);
        Lignt_L.EnableKeyword("_EMISSION");
        yield return new WaitForSeconds(0.75f);
        Lignt_L.DisableKeyword("_EMISSION");
        yield return new WaitForSeconds(0.3f);
        Lignt_R.EnableKeyword("_EMISSION");
        yield return new WaitForSeconds(0.75f);
        Lignt_R.DisableKeyword("_EMISSION");
        yield return new WaitForSeconds(0.3f);
        Lignt_R.EnableKeyword("_EMISSION");
        yield return new WaitForSeconds(0.75f);
        Lignt_R.DisableKeyword("_EMISSION");
        yield return new WaitForSeconds(0.3f);
        Lignt_L.EnableKeyword("_EMISSION");
        yield return new WaitForSeconds(0.75f);
        Lignt_L.DisableKeyword("_EMISSION");
        yield return new WaitForSeconds(0.3f);
        Lignt_L.EnableKeyword("_EMISSION");
        yield return new WaitForSeconds(0.75f);
        Lignt_L.DisableKeyword("_EMISSION");
        yield return new WaitForSeconds(0.3f);
        Lignt_R.EnableKeyword("_EMISSION");
        yield return new WaitForSeconds(0.75f);
        Lignt_R.DisableKeyword("_EMISSION");
        HintButton.transform.DOLocalMove(UpPositions, 0.5f).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(0.5f);
        IsHint = false;
        HintButton = null;
    }

}
