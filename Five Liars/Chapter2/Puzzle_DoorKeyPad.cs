using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Puzzle_DoorKeyPad : Puzzle
{
    [SerializeField] private TMP_Text[] Numbertext;     // ��ư�� ������ ���� ��ư�� �ؽ�Ʈ�� ȭ��� ǥ���ϱ� ���� TMP_Text �迭
    [SerializeField] private Material[] Light;          // ���׸����� �Ӽ��� �����ϱ����� ��Ƶ� Material �迭
    [SerializeField] private LayerMask layer;           // Ư�� ���̾� üũ�� ���� Layer
    private const string PuzzleClearNumber = "5389";    // ������ ����üũ�� ���� ������ ��Ƶ� const string
    private string NumberCheck = string.Empty;          // ������ ���� üũ�� ���� string
    private bool isChecking = false;                    // ������ üũ������ Ȯ���ϴ� bool

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
    /// ��ġ�� ������Ʈ�� name���� Ȯ���Ͽ� string���� �Է��ϰ� ����üũ �ϴ� �޼ҵ�
    /// </summary>
    /// <param name="name"></param>
    private void Buttoninput(string name)
    {
        string numberPart = name.Replace("Num_", "");

        if (!int.TryParse(numberPart, out int result)) return; // numberPart�� int������ �����ϸ鼭 ���ڰ� �ƴҰ�� ��ȯ

        NumberCheck += result;

        int inputStep = NumberCheck.Length - 1;
        Numbertext[inputStep].text = result.ToString();
        AudioManager.Instance.PlaySFX("SFX_Beeps");


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
            enabled = false;
            Light[1].EnableKeyword("_EMISSION");
            Invoke("Success", 0.5f);
        }
        else
        {
            Invoke("Failure", 0.5f);
            Light[0].EnableKeyword("_EMISSION");
        }
    }

    /// <summary>
    /// ������ �����ϰ�� ����Ǵ� �޼ҵ�
    /// </summary>
    private void Success()
    {
        Light[1].DisableKeyword("_EMISSION");
        for (int i = 0; i < NumberCheck.Length; i++)
        {
            Numbertext[i].text = string.Empty;
        }
        NumberCheck = string.Empty;
        isChecking = false;
        AudioManager.Instance.PlaySFX("SFX_UseKey");
        PuzzleClear();
    }

    /// <summary>
    /// ������ �����ϰ�� ����Ǵ� �޼ҵ�
    /// </summary>
    private void Failure()
    {
        for (int i = 0; i < NumberCheck.Length; i++)
        {
            Numbertext[i].text = string.Empty;
        }

        NumberCheck = string.Empty;
        AudioManager.Instance.PlaySFX("SFX_ErrorSound_1");
        isChecking = false;
        Light[0].DisableKeyword("_EMISSION");
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
