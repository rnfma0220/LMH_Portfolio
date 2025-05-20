using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Puzzle_Mannequin : Puzzle
{
    [SerializeField] private Material[] Button_Material;                            // 오브젝트의 마테리얼의 속성을 변경 하기위해 위한 Material
    [SerializeField] private GameObject[] ButtonObject;                             // 오브젝트의 포지션을 수정하기위해 담아둔 GameObject 배열
    [SerializeField] private LayerMask layer;                                       // 특정 레이어 체크를 위한 Layer
    private const string MannequinNumber = "01234";                                 // 퍼즐의 정답체크를 위해 정답을 담아둔 const string
    private string NumberCheck = string.Empty;                                      // 퍼즐의 정답 체크를 위한 string
    private Vector3 Basicstate = new Vector3(0.0005f, -0.0004f, -0.2034f);          // 특정 오브젝트를 움직일때 사용하기 위해 포지션을 담아둔 Vector3
    private Vector3 Pressedstate = new Vector3(0f, 0f, -0.215f);                    // 특정 오브젝트를 움직일때 사용하기 위해 포지션을 담아둔 Vector3
    private bool IsButton;                                                          // 버튼을 터치중인지 확인하는 bool
    private bool[] ButtonStates;                                                    // 버튼의 상태를 저장하기위한 bool 배열

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
    /// 터치된 오브젝트를 체크하여 값을 넣는 메소드
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private IEnumerator Mannequin(string name)
    {
        if (!int.TryParse(name, out int index)) yield break; // name을 int값으로 변경하면서 숫자가 아닐경우 반환처리

        if (ButtonStates[index]) yield break; // 이미 눌린 버튼이면 중단

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
    /// 정답을 체크하는 메소드
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
    /// 퍼즐의 상태를 초기화하는 메소드
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
    /// 퍼즐 클래스에서 상속된 퍼즐 Position Event
    /// </summary>
    /// <param name="context"></param>
    public override void OnPuzzlePosition(InputAction.CallbackContext context)
    {
        return;
    }
}
