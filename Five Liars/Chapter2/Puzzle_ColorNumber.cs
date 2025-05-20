using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Puzzle_ColorNumber : Puzzle 
{ 
    [SerializeField] private TMP_Text[] Numbertext;         // ��ư�� ������ ���� ��ư�� �ؽ�Ʈ�� ȭ��� ǥ���ϱ� ���� TMP_Text �迭
    [SerializeField] private LayerMask layer;               // Ư�� ���̾� üũ�� ���� Layer
    private const string PuzzleClearNumber = "4218";        // ������ ����üũ�� ���� ������ ��Ƶ� const string
    private string NumberCheck;                             // ������ ���� üũ�� ���� string
    private bool isChecking = false;                        // ���� üũ�������� Ȯ���ϱ� ���� bool

    /// <summary>
    /// ���� Ŭ�������� ��ӵ� ���� Press Event
    /// </summary>
    /// <param name="context"></param>
    public override void OnPuzzlePress(InputAction.CallbackContext context)
    {
        if (!CheckTouchEnable()) return;

        if (context.canceled)
        {
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(touchPosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layer) && !isChecking)
            {
                Buttoninput(hit.transform.gameObject.name);
            }
        }
    }

    /// <summary>
    /// ��ġ�� ������Ʈ�� name�� string�� Text�� �ִ� �޼ҵ�
    /// </summary>
    /// <param name="name"></param>
    private void Buttoninput(string name)
    {
        NumberCheck += name;

        int inputStep = NumberCheck.Length - 1;
        Numbertext[inputStep].text = name;
        AudioManager.Instance.PlaySFX("SFX_DigitalButton");

        if (NumberCheck.Length == 4)
        {
            ClearCheck();
            isChecking = true;
        }
    }

    /// <summary>
    /// ������ üũ�ϴ� �޼ҵ�
    /// </summary>
    private void ClearCheck()
    {
        if (PuzzleClearNumber == NumberCheck)
        {
            PuzzleClear();
        }
        else
        {
            Invoke("Failure", 0.5f);
        }
    }

    /// <summary>
    /// ������ Ʋ������ ȭ�鿡 ǥ��� text�� ����üũ ���� �ʱ�ȭ�ϱ� ���� �޼ҵ�
    /// </summary>
    private void Failure()
    {
        for (int i = 0; i < NumberCheck.Length; i++)
        {
            Numbertext[i].text = string.Empty;
        }

        NumberCheck = string.Empty;
        isChecking = false;
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
