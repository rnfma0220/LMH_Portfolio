using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Puzzle_CabinetKeyPad : Puzzle
{
    [SerializeField] private Material[] Button_Material;    // 마테리얼의 속성을 변경 하기 위한 Material
    [SerializeField] private Material SuccessLight;         // 마테리얼의 속성을 변경 하기 위한 Material
    [SerializeField] private Material FailureLight;         // 마테리얼의 속성을 변경 하기 위한 Material
    [SerializeField] private LayerMask layer;               // 특정 레이어 체크를 위한 Layer

    private const string KeyNumber = "348157269";           // 정답 체크를 위해 만들어둔 const string
    private string NumberCheck = string.Empty;              // 버튼을 누른 값을 저장하여 정답을 체크하기 위한 용도의 string
    private bool[] ButtonStates;                            // 버튼의 상태를 저장하기위한 bool 배열

    private void Start()
    {
        ButtonStates = new bool[Button_Material.Length]; // 상태 초기화
    }

    /// <summary>
    /// 퍼즐 클래스에서 상속된 퍼즐 Press Event
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
        if (name.Equals("Enter"))
        {
            ClearCheck();
        }
        else
        {
            if (!int.TryParse(name, out int index)) return; // name을 int값으로 변경하면서 숫자가 아닐경우 반환
            index -= 1;
            if (ButtonStates[index]) return; // 이미 눌린 버튼이면 반환

            Button_Material[index].EnableKeyword("_EMISSION");
            ButtonStates[index] = true;
            NumberCheck += name;
            AudioManager.Instance.PlaySFX("SFX_Beeps");
        }
    }

    /// <summary>
    /// 정답을 체크하기 위한 메소드
    /// </summary>
    private void ClearCheck()
    {
        if (KeyNumber == NumberCheck)
        {
            // 정답 처리
            ResetPuzzle();
            SuccessLight.EnableKeyword("_EMISSION");
            PuzzleClear();
        }
        else
        {
            // 실패 처리
            ResetPuzzle();
            FailureLight.EnableKeyword("_EMISSION");
            Invoke("FailureLightOff", 1f);
        }
    }

    /// <summary>
    /// FailureLight의 EMISSION을 끄기위한 메소드
    /// </summary>
    private void FailureLightOff()
    {
        FailureLight.DisableKeyword("_EMISSION");
    }

    /// <summary>
    /// 버튼의 상태와 마테리얼을 초기화 하기위한 메소드
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
