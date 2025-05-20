using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class Puzzle_Distributionbox : Puzzle
{
    /// <summary>
    /// 스위치가 On인지 Off인지 체크하기위한 enum
    /// </summary>
    public enum OnOff
    {
        Off,
        On
    }

    [SerializeField] private Puzzle_Check[] Switches;       // 오브젝트의 enum을 이용하여 정답을 체크하기위한 배열
    [SerializeField] private Material[] Light;              // 마테리얼의 속성을 변경 하기 위한 Material
    [SerializeField] private GameObject Checkanswer;        // 정답을 체크하는 오브젝트를 담아두는 GameObject
    [SerializeField] private LayerMask layer;               // 특정 레이어 체크를 위한 Layer
    private bool PuzzleCheck;                               // 퍼즐의 정답이 체크중인지 확인하는 bool
    private int[] ClearNumber = new int[] { 0, 1, 0, 1, 0, 1, 0, 1, 1, 0, 0, 1, 1, 0, 0 }; // 퍼즐의 정답 체크를 위한 int 배열

    /// <summary>
    /// 퍼즐 클래스에서 상속된 퍼즐 Press Event
    /// </summary>
    /// <param name="context"></param>
    public override void OnPuzzlePress(InputAction.CallbackContext context)
    {
        if (!CheckTouchEnable()) return;

        if (PuzzleCheck) return;

        if (context.canceled)
        {
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

            Ray ray = Camera.main.ScreenPointToRay(touchPosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layer))
            {
                if(hit.transform.gameObject == Checkanswer)
                {
                    PuzzleCheck = true;
                    AudioManager.Instance.PlaySFX("SFX_PushLever");
                    ClearCheck();
                }
                else
                {
                    OnoffChange(hit.transform.gameObject);
                }
            }
        }
    }

    /// <summary>
    /// 오브젝트 터치시 On과 Off를 변경하는 메소드
    /// </summary>
    /// <param name="obj"></param>
    private void OnoffChange(GameObject obj)
    {
        Puzzle_Check puzzleCheck = obj.GetComponent<Puzzle_Check>();
        if (puzzleCheck == null) return;

        AudioManager.Instance.PlaySFX("SFX_PushSwitch");

        if (puzzleCheck.onoffswitch == OnOff.On)
        {
            obj.transform.DOLocalMoveX(0.0702f, 0f);
            puzzleCheck.onoffswitch = OnOff.Off;
        }
        else
        {
            obj.transform.DOLocalMoveX(-0.0700f, 0f);
            puzzleCheck.onoffswitch = OnOff.On;
        }
    }

    /// <summary>
    /// 정답을 체크하기 위한 메소드
    /// </summary>
    private void ClearCheck()
    {
        for (int i = 0; i < Switches.Length; i++)
        {

            if (Switches[i].onoffswitch != (OnOff)ClearNumber[i])
            {
                StartCoroutine(Failure());
                AudioManager.Instance.PlaySFX("SFX_ErrorSound_2");
                return;
            }
        }
        Checkanswer.transform.DOLocalRotate(new Vector3(10.279f, 270.116f, -363.121f), 0.25f).SetEase(Ease.Linear);
        Light[0].EnableKeyword("_EMISSION");
        AudioManager.Instance.PlaySFX("SFX_CorrectSound");
        PuzzleClear();
    }

    /// <summary>
    /// 정답 실패 시 마테리얼의 속성을 변경하는 메소드
    /// </summary>
    /// <returns></returns>
    private IEnumerator Failure()
    {
        Checkanswer.transform.DOLocalRotate(new Vector3(10.279f, 270.116f, -363.121f), 0.25f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(0.25f);
        Light[1].EnableKeyword("_EMISSION");
        Checkanswer.transform.DOLocalRotate(new Vector3(-81.046f, 284.103f, -375.635f), 0.25f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(0.25f);
        PuzzleCheck = false;
        yield return new WaitForSeconds(0.25f);
        Light[1].DisableKeyword("_EMISSION");
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
