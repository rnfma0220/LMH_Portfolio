using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Puzzle_LightHint : Puzzle
{
    [SerializeField] private Material Lignt_L;                          // ���׸����� �Ӽ��� ���� �ϱ� ���� Material
    [SerializeField] private Material Lignt_R;                          // ���׸����� �Ӽ��� ���� �ϱ� ���� Material
    [SerializeField] private LayerMask layer;                           // Ư�� ���̾� üũ�� ���� Layer

    private bool IsHint;                                                // ���� ��Ʈ�� ��������� üũ�ϱ� ���� bool
    private GameObject HintButton;                                      // ��ġ�� ������Ʈ�� ��Ƶα� ���� GameObject
    private Vector3 UpPositions = new Vector3(0.137f, 0f, 0.016f);      // Ư�� ������Ʈ�� �����϶� ����ϱ� ���� �������� ��Ƶ� Vector3
    private Vector3 DownPositions = new Vector3(0.135f, 0f, 0.008f);    // Ư�� ������Ʈ�� �����϶� ����ϱ� ���� �������� ��Ƶ� Vector3

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
    /// ��Ʈ�� ������� ����ϴ� �ڷ�ƾ �޼ҵ�
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
