using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Puzzle_Mannequin : Puzzle
{
    [SerializeField] private Material[] Button_Material;                            // ������Ʈ�� ���׸����� �Ӽ��� ���� �ϱ����� ���� Material
    [SerializeField] private GameObject[] ButtonObject;                             // ������Ʈ�� �������� �����ϱ����� ��Ƶ� GameObject �迭
    [SerializeField] private LayerMask layer;                                       // Ư�� ���̾� üũ�� ���� Layer
    private const string MannequinNumber = "01234";                                 // ������ ����üũ�� ���� ������ ��Ƶ� const string
    private string NumberCheck = string.Empty;                                      // ������ ���� üũ�� ���� string
    private Vector3 Basicstate = new Vector3(0.0005f, -0.0004f, -0.2034f);          // Ư�� ������Ʈ�� �����϶� ����ϱ� ���� �������� ��Ƶ� Vector3
    private Vector3 Pressedstate = new Vector3(0f, 0f, -0.215f);                    // Ư�� ������Ʈ�� �����϶� ����ϱ� ���� �������� ��Ƶ� Vector3
    private bool IsButton;                                                          // ��ư�� ��ġ������ Ȯ���ϴ� bool
    private bool[] ButtonStates;                                                    // ��ư�� ���¸� �����ϱ����� bool �迭

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

        if (context.canceled)
        {
            if (IsButton) return;

            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

            Ray ray = Camera.main.ScreenPointToRay(touchPosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layer))
            {
                StartCoroutine(Mannequin(hit.transform.name));
            }
        }
    }

    /// <summary>
    /// ��ġ�� ������Ʈ�� üũ�Ͽ� ���� �ִ� �޼ҵ�
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private IEnumerator Mannequin(string name)
    {
        if (!int.TryParse(name, out int index)) yield break; // name�� int������ �����ϸ鼭 ���ڰ� �ƴҰ�� ��ȯó��

        if (ButtonStates[index]) yield break; // �̹� ���� ��ư�̸� �ߴ�

        IsButton = true;

        ButtonObject[index].transform.DOLocalMove(Pressedstate, 0.25f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(0.25f);
        Button_Material[index].EnableKeyword("_EMISSION");
        NumberCheck += name;
        ButtonStates[index] = true;
        IsButton = false;
        AudioManager.Instance.PlaySFX("SFX_PushButton1");

        if (NumberCheck.Length == Button_Material.Length)
        {
            yield return new WaitForSeconds(0.25f);
            ClearCheck();
        }
    }

    /// <summary>
    /// ������ üũ�ϴ� �޼ҵ�
    /// </summary>
    private void ClearCheck()
    {
        if (MannequinNumber == NumberCheck)
        {
            AudioManager.Instance.PlaySFX("SFX_MoveHand");
            PuzzleClear();
        }
        else
        {
            ResetPuzzle();
        }
    }

    /// <summary>
    /// ������ ���¸� �ʱ�ȭ�ϴ� �޼ҵ�
    /// </summary>
    private void ResetPuzzle()
    {
        for (int i = 0; i < ButtonObject.Length; i++)
        {
            ButtonObject[i].transform.DOLocalMove(Basicstate, 0.5f).SetEase(Ease.Linear);
            Button_Material[i].DisableKeyword("_EMISSION");
            ButtonStates[i] = false;
        }
        NumberCheck = string.Empty;
    }

    /// <summary>
    /// ���� Ŭ�������� ��ӵ� ���� Position Event
    /// </summary>
    /// <param name="context"></param>
    public override void OnPuzzlePosition(InputAction.CallbackContext context)
    {
        return;
    }
}
