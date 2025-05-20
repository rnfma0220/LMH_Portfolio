using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Puzzle_Rbio : Puzzle
{
    [SerializeField] private Material Lignt;                      // 마테리얼의 속성을 변경 하기 위한 Material
    [SerializeField] private LayerMask layer;                     // 특정 레이어 체크를 위한 Layer

    [SerializeField] private float shortLight = 0.3f;             // 라이트가 빛나는 짧은시간의 지연시간
    [SerializeField] private float longLight = 1f;                // 라이트가 빛나는 긴시간의 지연시간
    [SerializeField] private float time = 0.1f;                   // 다음 라이트가 나오기전의 약간의 지연시간
    [SerializeField] private float WaitTime = 1.5f;               // 코루틴에서 사용할 다음 라이트의 지연시간

    private bool IsHint;                                          // 현재 힌트가 재생중인지 체크하는 bool
    private GameObject HintButton;                                // 터치된 오브젝트를 담아주는 GameObject

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
    /// 지정한 시간만큼 오브젝트의 Light를 빛나게하고 끄는 메소드
    /// </summary>
    /// <returns></returns>
    private IEnumerator HintStart()
    {
        AudioManager.Instance.PlaySFX("SFX_Morse");
        yield return new WaitForSeconds(0.5f);
        Lignt.EnableKeyword("_EMISSION");
        yield return new WaitForSeconds(longLight);
        Lignt.DisableKeyword("_EMISSION");
        yield return new WaitForSeconds(time);
        Lignt.EnableKeyword("_EMISSION");
        yield return new WaitForSeconds(shortLight);
        Lignt.DisableKeyword("_EMISSION");
        yield return new WaitForSeconds(time);
        Lignt.EnableKeyword("_EMISSION");
        yield return new WaitForSeconds(longLight);
        Lignt.DisableKeyword("_EMISSION");
        yield return new WaitForSeconds(WaitTime); //--------------------------------------------
        Lignt.EnableKeyword("_EMISSION");
        yield return new WaitForSeconds(shortLight);
        Lignt.DisableKeyword("_EMISSION");
        yield return new WaitForSeconds(time);
        Lignt.EnableKeyword("_EMISSION");
        yield return new WaitForSeconds(longLight);
        Lignt.DisableKeyword("_EMISSION");
        yield return new WaitForSeconds(WaitTime); //--------------------------------------------
        Lignt.EnableKeyword("_EMISSION");
        yield return new WaitForSeconds(shortLight);
        Lignt.DisableKeyword("_EMISSION");
        yield return new WaitForSeconds(time);
        Lignt.EnableKeyword("_EMISSION");
        yield return new WaitForSeconds(longLight);
        Lignt.DisableKeyword("_EMISSION");
        yield return new WaitForSeconds(time);
        Lignt.EnableKeyword("_EMISSION");
        yield return new WaitForSeconds(shortLight);
        Lignt.DisableKeyword("_EMISSION");
        yield return new WaitForSeconds(WaitTime); //--------------------------------------------
        Lignt.EnableKeyword("_EMISSION");
        yield return new WaitForSeconds(longLight);
        Lignt.DisableKeyword("_EMISSION");
        yield return new WaitForSeconds(time);
        Lignt.EnableKeyword("_EMISSION");
        yield return new WaitForSeconds(longLight);
        Lignt.DisableKeyword("_EMISSION");
        yield return new WaitForSeconds(WaitTime); //--------------------------------------------
        Lignt.EnableKeyword("_EMISSION");
        yield return new WaitForSeconds(shortLight);
        Lignt.DisableKeyword("_EMISSION");
        yield return new WaitForSeconds(time);
        Lignt.EnableKeyword("_EMISSION");
        yield return new WaitForSeconds(longLight);
        Lignt.DisableKeyword("_EMISSION");
        IsHint = false;
        HintButton = null;
    }
}
