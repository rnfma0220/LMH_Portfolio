using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class Puzzle_EncryptedDevice : Puzzle
{
    [SerializeField] private Material ButtonLight_L;                                // 버튼의 마테리얼에서 EMISSION 을 활성화 비활성화 하기위해 선언한 Material
    [SerializeField] private Material ButtonLight_R;                                // 버튼의 마테리얼에서 EMISSION 을 활성화 비활성화 하기위해 선언한 Material
    [SerializeField] private LayerMask layer;                                       // 특정 레이어 체크를 위한 Layer
    private string DeviceNumberCheck = string.Empty;                                // 버튼을 누른 값을 저장하여 정답을 체크하기 위한 용도의 string
    private GameObject PressButton;                                                 // 현재 누른 버튼이 무엇인지 확인하기위해 담아두는 GameObjcet
    const string DeviceNumber = "011001";                                        // 퍼즐의 정답체크를 위해 정답을 담아둔 readonly String
    private bool IsButton;                                                          // 현재 버튼이 클릭중인지를 체크하는 bool
    private bool isChecking = false;                                                // 현재 정답 체크중인지 체크하는 bool
    private Vector3 UpPositions = new Vector3(-0.036f, 0.067f, 0.0506f);            // 오브젝트를 움직일때 사용하기 위해 포지션을 담아둔 Vector3
    private Vector3 DownPositions = new Vector3(-0.0458f, 0.067f, 0.0506f);         // 오브젝트를 움직일때 사용하기 위해 포지션을 담아둔 Vector3

    /// <summary>
    /// 퍼즐 클래스에서 상속된 퍼즐 Press Event
    /// </summary>
    /// <param name="context"></param>
    public override void OnPuzzlePress(InputAction.CallbackContext context)
    {
        if (!CheckTouchEnable()) return;

        if (context.canceled)
        {
            if (PressButton != null && IsButton) return;
            if (isChecking) return;

            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(touchPosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layer))
            {
                PressButton = hit.transform.gameObject;
                StartCoroutine(PuzzleInput());
                AudioManager.Instance.PlaySFX("SFX_DigitalButton");
                IsButton = true;
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
    /// 터치된 오브젝트를 체크하여 값을 넣는 메소드
    /// </summary>
    /// <returns></returns>
    private IEnumerator PuzzleInput()
    {
        if (PressButton.CompareTag("DeviceButtonL"))
        {
            PressButton.transform.DOLocalMove(DownPositions, 0.1f).SetEase(Ease.InOutQuad);
            yield return new WaitForSeconds(0.1f);
            ButtonLight_L.EnableKeyword("_EMISSION");
            DeviceNumberCheck += "0";
            yield return new WaitForSeconds(0.2f);
            PressButton.transform.DOLocalMove(UpPositions, 0.1f).SetEase(Ease.InOutQuad);
            ButtonLight_L.DisableKeyword("_EMISSION");
        }
        else
        {
            PressButton.transform.DOLocalMove(DownPositions, 0.1f).SetEase(Ease.InOutQuad);
            yield return new WaitForSeconds(0.1f);
            ButtonLight_R.EnableKeyword("_EMISSION");
            DeviceNumberCheck += "1";
            yield return new WaitForSeconds(0.2f);
            PressButton.transform.DOLocalMove(UpPositions, 0.1f).SetEase(Ease.InOutQuad);
            ButtonLight_R.DisableKeyword("_EMISSION");
        }

        ButtonAction();
    }

    /// <summary>
    /// 버튼 입력이 종료되었을때 초기화 해주고 6개 입력되었는지 체크하는 메소드
    /// </summary>
    private void ButtonAction()
    {
        if (DeviceNumberCheck.Length == 6)
        {
            isChecking = true;

            ClearCheck();
        }
        IsButton = false;
        PressButton = null;
    }

    /// <summary>
    /// 정답이 아닐경우 실패의 효과를 알려주며 입력된 값을 초기화 시켜주는 메소드
    /// </summary>
    /// <returns></returns>
    private IEnumerator Puzzlefailure()
    {
        for (int i = 0; i < 3; i++)
        {
            ButtonLight_L.EnableKeyword("_EMISSION");
            ButtonLight_R.EnableKeyword("_EMISSION");
            yield return new WaitForSeconds(0.2f);
            ButtonLight_L.DisableKeyword("_EMISSION");
            ButtonLight_R.DisableKeyword("_EMISSION");
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(0.05f);
        DeviceNumberCheck = string.Empty;
        isChecking = false;
    }

    /// <summary>
    /// 입력된 오브젝트가 정답인지 체크하는 메소드
    /// </summary>
    private void ClearCheck()
    {
        if (DeviceNumber == DeviceNumberCheck)
        {
            PuzzleClear();
        }
        else
        {
            AudioManager.Instance.PlaySFX("SFX_ErrorSound");
            StartCoroutine(Puzzlefailure());
        }
    }
}
