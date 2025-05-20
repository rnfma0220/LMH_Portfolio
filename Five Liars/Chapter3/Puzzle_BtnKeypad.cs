using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Puzzle_BtnKeypad : Puzzle
{
    [SerializeField] private Material[] keyLight;           // 마테리얼의 속성을 설정하기위해 담아둔 Material 배열
    [SerializeField] private LayerMask layer;               // 특정 레이어 체크를 위한 Layer

    private const string KeyNumber = "7358";                // 퍼즐의 정답체크를 위해 정답을 담아둔 const string
    private string NumberCheck = string.Empty;              // 퍼즐의 정답 체크를 위한 string
    private bool[] ButtonStates;                            // 버튼의 상태를 저장하기위한 bool 배열
    private bool isCheck;                                   // 정답을 체크하는 상태를 확인하기위한 bool

    private void Start()
    {
        ButtonStates = new bool[keyLight.Length]; // 상태 초기화
    }

    /// <summary>
    /// 퍼즐 클래스에서 상속된 퍼즐 Press Event
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
    /// 퍼즐 클래스에서 상속된 퍼즐 Position Event
    /// </summary>
    /// <param name="context"></param>
    public override void OnPuzzlePosition(InputAction.CallbackContext context)
    {
        return;
    }

    /// <summary>
    /// 터치된 오브젝트의 name값을 확인하여 string값을 입력하고 정답체크 하는 메소드
    /// </summary>
    /// <param name="name"></param>
    private void KeyPadTouch(string name)
    {
        if (!int.TryParse(name, out int index)) return; // name을 int값으로 변경하면서 숫자가 아닐경우 반환처리
        index -= 1;
        if (ButtonStates[index]) return; // 이미 눌린 버튼이면 중단

        keyLight[index].EnableKeyword("_EMISSION");
        ButtonStates[index] = true;
        NumberCheck += name;
        AudioManager.Instance.PlaySFX("SFX_Beeps");

        if(NumberCheck.Length == 4) ClearCheck();
    }

    /// <summary>
    /// 정답 체크하기 위한 메소드
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
    /// 입력된 값과 마테리얼을 초기화 하기 위한 메소드
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
