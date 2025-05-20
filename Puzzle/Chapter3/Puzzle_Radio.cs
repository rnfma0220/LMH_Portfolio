using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Puzzle_Rbio : Puzzle
{
    [SerializeField] private Material Lignt;                      // ���׸����� �Ӽ��� ���� �ϱ� ���� Material
    [SerializeField] private LayerMask layer;                     // Ư�� ���̾� üũ�� ���� Layer

    [SerializeField] private float shortLight = 0.3f;             // ����Ʈ�� ������ ª���ð��� �����ð�
    [SerializeField] private float longLight = 1f;                // ����Ʈ�� ������ ��ð��� �����ð�
    [SerializeField] private float time = 0.1f;                   // ���� ����Ʈ�� ���������� �ణ�� �����ð�
    [SerializeField] private float WaitTime = 1.5f;               // �ڷ�ƾ���� ����� ���� ����Ʈ�� �����ð�

    private bool IsHint;                                          // ���� ��Ʈ�� ��������� üũ�ϴ� bool
    private GameObject HintButton;                                // ��ġ�� ������Ʈ�� ����ִ� GameObject

    /// <summary>
    /// ���� Ŭ�������� ��ӵ� ���� Press Event
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
    /// ���� Ŭ�������� ��ӵ� ���� Position Event
    /// </summary>
    /// <param name="context"></param>
    public override void OnPuzzlePosition(InputAction.CallbackContext context)
    {
        return;
    }

    /// <summary>
    /// ������ �ð���ŭ ������Ʈ�� Light�� �������ϰ� ���� �޼ҵ�
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
