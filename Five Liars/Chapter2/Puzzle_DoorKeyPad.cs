using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Puzzle_DoorKeyPad : Puzzle
{
    [SerializeField] private TMP_Text[] Numbertext;     // 버튼을 누르면 누른 버튼의 텍스트를 화면상에 표기하기 위한 TMP_Text 배열
    [SerializeField] private Material[] Light;          // 마테리얼의 속성을 설정하기위해 담아둔 Material 배열
    [SerializeField] private LayerMask layer;           // 특정 레이어 체크를 위한 Layer
    private const string PuzzleClearNumber = "5389";    // 퍼즐의 정답체크를 위해 정답을 담아둔 const string
    private string NumberCheck = string.Empty;          // 퍼즐의 정답 체크를 위한 string
    private bool isChecking = false;                    // 정답을 체크중인지 확인하는 bool

    /// <summary>
    /// 퍼즐 클래스에서 상속된 퍼즐 Press Event
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
    /// 터치된 오브젝트의 name값을 확인하여 string값을 입력하고 정답체크 하는 메소드
    /// </summary>
    /// <param name="name"></param>
    private void Buttoninput(string name)
    {
        string numberPart = name.Replace("Num_", "");

        if (!int.TryParse(numberPart, out int result)) return; // numberPart를 int값으로 변경하면서 숫자가 아닐경우 반환

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
    /// 정답을 체크하는 메소드
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
    /// 퍼즐이 정답일경우 실행되는 메소드
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
    /// 퍼즐이 실패일경우 실행되는 메소드
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
    /// 퍼즐 클래스에서 상속된 퍼즐 Position Event
    /// </summary>
    /// <param name="context"></param>
    public override void OnPuzzlePosition(InputAction.CallbackContext context)
    {
        return;
    }
}
