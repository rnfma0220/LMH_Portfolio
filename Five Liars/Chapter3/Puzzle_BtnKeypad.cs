using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Puzzle_BtnKeypad : Puzzle
{
    [SerializeField] private Material[] keyLight;           // ���׸����� �Ӽ��� �����ϱ����� ��Ƶ� Material �迭
    [SerializeField] private LayerMask layer;               // Ư�� ���̾� üũ�� ���� Layer

    private const string KeyNumber = "7358";                // ������ ����üũ�� ���� ������ ��Ƶ� const string
    private string NumberCheck = string.Empty;              // ������ ���� üũ�� ���� string
    private bool[] ButtonStates;                            // ��ư�� ���¸� �����ϱ����� bool �迭
    private bool isCheck;                                   // ������ üũ�ϴ� ���¸� Ȯ���ϱ����� bool

    private void Start()
    {
        ButtonStates = new bool[keyLight.Length]; // ���� �ʱ�ȭ
    }

    /// <summary>
    /// ���� Ŭ�������� ��ӵ� ���� Press Event
    /// </summary>
    /// <param name="context"></param>
    public override void OnPuzzlePress(InputAction.CallbackContext context)
    {
        if (!CheckTouchEnable()) return;

        if (context.canceled)
        {
            if (isCheck) return;

            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

            Ray ray = Camera.main.ScreenPointToRay(touchPosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layer))
            {
                KeyPadTouch(hit.transform.name);
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
    /// ��ġ�� ������Ʈ�� name���� Ȯ���Ͽ� string���� �Է��ϰ� ����üũ �ϴ� �޼ҵ�
    /// </summary>
    /// <param name="name"></param>
    private void KeyPadTouch(string name)
    {
        if (!int.TryParse(name, out int index)) return; // name�� int������ �����ϸ鼭 ���ڰ� �ƴҰ�� ��ȯó��
        index -= 1;
        if (ButtonStates[index]) return; // �̹� ���� ��ư�̸� �ߴ�

        keyLight[index].EnableKeyword("_EMISSION");
        ButtonStates[index] = true;
        NumberCheck += name;
        AudioManager.Instance.PlaySFX("SFX_Beeps");

        if(NumberCheck.Length == 4) ClearCheck();
    }

    /// <summary>
    /// ���� üũ�ϱ� ���� �޼ҵ�
    /// </summary>
    private void ClearCheck()
    {
        isCheck = true;

        if (KeyNumber.Equals(NumberCheck))
        {
            Invoke("ResetPuzzle", 1f);
            PuzzleClear();
        }
        else
        {
            AudioManager.Instance.PlaySFX("SFX_ErrorSound_2");
            Invoke("ResetPuzzle", 1f);
        }
    }

    /// <summary>
    /// �Էµ� ���� ���׸����� �ʱ�ȭ �ϱ� ���� �޼ҵ�
    /// </summary>
    private void ResetPuzzle()
    {
        for (int i = 0; i < keyLight.Length; i++)
        {
            keyLight[i].DisableKeyword("_EMISSION");
            ButtonStates[i] = false;
        }
        NumberCheck = string.Empty;
        isCheck = false;
    }
}
