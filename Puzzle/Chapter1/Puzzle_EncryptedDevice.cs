using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class Puzzle_EncryptedDevice : Puzzle
{
    [SerializeField] private Material ButtonLight_L;                                // ��ư�� ���׸��󿡼� EMISSION �� Ȱ��ȭ ��Ȱ��ȭ �ϱ����� ������ Material
    [SerializeField] private Material ButtonLight_R;                                // ��ư�� ���׸��󿡼� EMISSION �� Ȱ��ȭ ��Ȱ��ȭ �ϱ����� ������ Material
    [SerializeField] private LayerMask layer;                                       // Ư�� ���̾� üũ�� ���� Layer
    private string DeviceNumberCheck = string.Empty;                                // ��ư�� ���� ���� �����Ͽ� ������ üũ�ϱ� ���� �뵵�� string
    private GameObject PressButton;                                                 // ���� ���� ��ư�� �������� Ȯ���ϱ����� ��Ƶδ� GameObjcet
    const string DeviceNumber = "011001";                                        // ������ ����üũ�� ���� ������ ��Ƶ� readonly String
    private bool IsButton;                                                          // ���� ��ư�� Ŭ���������� üũ�ϴ� bool
    private bool isChecking = false;                                                // ���� ���� üũ������ üũ�ϴ� bool
    private Vector3 UpPositions = new Vector3(-0.036f, 0.067f, 0.0506f);            // ������Ʈ�� �����϶� ����ϱ� ���� �������� ��Ƶ� Vector3
    private Vector3 DownPositions = new Vector3(-0.0458f, 0.067f, 0.0506f);         // ������Ʈ�� �����϶� ����ϱ� ���� �������� ��Ƶ� Vector3

    /// <summary>
    /// ���� Ŭ�������� ��ӵ� ���� Press Event
    /// </summary>
    /// <param name="context"></param>
    public override void OnPuzzlePress(InputAction.CallbackContext context)
    {
        if (!CheckTouchEnable()) return;

        if (context.canceled)
        {
            if (PressButton != null && IsButton) return;
            if (isChecking) return;

            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(touchPosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layer))
            {
                PressButton = hit.transform.gameObject;
                StartCoroutine(PuzzleInput());
                AudioManager.Instance.PlaySFX("SFX_DigitalButton");
                IsButton = true;
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
    /// ��ġ�� ������Ʈ�� üũ�Ͽ� ���� �ִ� �޼ҵ�
    /// </summary>
    /// <returns></returns>
    private IEnumerator PuzzleInput()
    {
        if (PressButton.CompareTag("DeviceButtonL"))
        {
            PressButton.transform.DOLocalMove(DownPositions, 0.1f).SetEase(Ease.InOutQuad);
            yield return new WaitForSeconds(0.1f);
            ButtonLight_L.EnableKeyword("_EMISSION");
            DeviceNumberCheck += "0";
            yield return new WaitForSeconds(0.2f);
            PressButton.transform.DOLocalMove(UpPositions, 0.1f).SetEase(Ease.InOutQuad);
            ButtonLight_L.DisableKeyword("_EMISSION");
        }
        else
        {
            PressButton.transform.DOLocalMove(DownPositions, 0.1f).SetEase(Ease.InOutQuad);
            yield return new WaitForSeconds(0.1f);
            ButtonLight_R.EnableKeyword("_EMISSION");
            DeviceNumberCheck += "1";
            yield return new WaitForSeconds(0.2f);
            PressButton.transform.DOLocalMove(UpPositions, 0.1f).SetEase(Ease.InOutQuad);
            ButtonLight_R.DisableKeyword("_EMISSION");
        }

        ButtonAction();
    }

    /// <summary>
    /// ��ư �Է��� ����Ǿ����� �ʱ�ȭ ���ְ� 6�� �ԷµǾ����� üũ�ϴ� �޼ҵ�
    /// </summary>
    private void ButtonAction()
    {
        if (DeviceNumberCheck.Length == 6)
        {
            isChecking = true;

            ClearCheck();
        }
        IsButton = false;
        PressButton = null;
    }

    /// <summary>
    /// ������ �ƴҰ�� ������ ȿ���� �˷��ָ� �Էµ� ���� �ʱ�ȭ �����ִ� �޼ҵ�
    /// </summary>
    /// <returns></returns>
    private IEnumerator Puzzlefailure()
    {
        for (int i = 0; i < 3; i++)
        {
            ButtonLight_L.EnableKeyword("_EMISSION");
            ButtonLight_R.EnableKeyword("_EMISSION");
            yield return new WaitForSeconds(0.2f);
            ButtonLight_L.DisableKeyword("_EMISSION");
            ButtonLight_R.DisableKeyword("_EMISSION");
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(0.05f);
        DeviceNumberCheck = string.Empty;
        isChecking = false;
    }

    /// <summary>
    /// �Էµ� ������Ʈ�� �������� üũ�ϴ� �޼ҵ�
    /// </summary>
    private void ClearCheck()
    {
        if (DeviceNumber == DeviceNumberCheck)
        {
            PuzzleClear();
        }
        else
        {
            AudioManager.Instance.PlaySFX("SFX_ErrorSound");
            StartCoroutine(Puzzlefailure());
        }
    }
}
