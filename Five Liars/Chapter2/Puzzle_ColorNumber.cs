using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Puzzle_ColorNumber : Puzzle 
{ 
    [SerializeField] private TMP_Text[] Numbertext;         // 버튼을 누르면 누른 버튼의 텍스트를 화면상에 표기하기 위한 TMP_Text 배열
    [SerializeField] private LayerMask layer;               // 특정 레이어 체크를 위한 Layer
    private const string PuzzleClearNumber = "4218";        // 퍼즐의 정답체크를 위해 정답을 담아둔 const string
    private string NumberCheck;                             // 퍼즐의 정답 체크를 위한 string
    private bool isChecking = false;                        // 정답 체크중인지를 확인하기 위한 bool

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
    /// 터치한 오브젝트의 name을 string과 Text에 넣는 메소드
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
    /// 정답을 체크하는 메소드
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
    /// 정답이 틀렸을때 화면에 표기된 text와 정답체크 값을 초기화하기 위한 메소드
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
    /// 퍼즐 클래스에서 상속된 퍼즐 Position Event
    /// </summary>
    /// <param name="context"></param>
    public override void OnPuzzlePosition(InputAction.CallbackContext context)
    {
        return;
    }
}
