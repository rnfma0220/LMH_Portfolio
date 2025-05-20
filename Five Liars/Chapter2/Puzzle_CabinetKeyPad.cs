using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Puzzle_CabinetKeyPad : Puzzle
{
    [SerializeField] private Material[] Button_Material;    // ���׸����� �Ӽ��� ���� �ϱ� ���� Material
    [SerializeField] private Material SuccessLight;         // ���׸����� �Ӽ��� ���� �ϱ� ���� Material
    [SerializeField] private Material FailureLight;         // ���׸����� �Ӽ��� ���� �ϱ� ���� Material
    [SerializeField] private LayerMask layer;               // Ư�� ���̾� üũ�� ���� Layer

    private const string KeyNumber = "348157269";           // ���� üũ�� ���� ������ const string
    private string NumberCheck = string.Empty;              // ��ư�� ���� ���� �����Ͽ� ������ üũ�ϱ� ���� �뵵�� string
    private bool[] ButtonStates;                            // ��ư�� ���¸� �����ϱ����� bool �迭

    private void Start()
    {
        ButtonStates = new bool[Button_Material.Length]; // ���� �ʱ�ȭ
    }

    /// <summary>
    /// ���� Ŭ�������� ��ӵ� ���� Press Event
    /// </summary>
    /// <param name="context"></param>
    public override void OnPuzzlePress(InputAction.CallbackContext context)
    {
        if (!CheckTouchEnable()) return;

        if(context.canceled)
        {
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
        if (name.Equals("Enter"))
        {
            ClearCheck();
        }
        else
        {
            if (!int.TryParse(name, out int index)) return; // name�� int������ �����ϸ鼭 ���ڰ� �ƴҰ�� ��ȯ
            index -= 1;
            if (ButtonStates[index]) return; // �̹� ���� ��ư�̸� ��ȯ

            Button_Material[index].EnableKeyword("_EMISSION");
            ButtonStates[index] = true;
            NumberCheck += name;
            AudioManager.Instance.PlaySFX("SFX_Beeps");
        }
    }

    /// <summary>
    /// ������ üũ�ϱ� ���� �޼ҵ�
    /// </summary>
    private void ClearCheck()
    {
        if (KeyNumber == NumberCheck)
        {
            // ���� ó��
            ResetPuzzle();
            SuccessLight.EnableKeyword("_EMISSION");
            PuzzleClear();
        }
        else
        {
            // ���� ó��
            ResetPuzzle();
            FailureLight.EnableKeyword("_EMISSION");
            Invoke("FailureLightOff", 1f);
        }
    }

    /// <summary>
    /// FailureLight�� EMISSION�� �������� �޼ҵ�
    /// </summary>
    private void FailureLightOff()
    {
        FailureLight.DisableKeyword("_EMISSION");
    }

    /// <summary>
    /// ��ư�� ���¿� ���׸����� �ʱ�ȭ �ϱ����� �޼ҵ�
    /// </summary>
    private void ResetPuzzle()
    {
        for (int i = 0; i < Button_Material.Length; i++)
        {
            Button_Material[i].DisableKeyword("_EMISSION");
            ButtonStates[i] = false;
        }
        NumberCheck = string.Empty;
    }
}
