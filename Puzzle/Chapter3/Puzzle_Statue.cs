using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class Puzzle_Statue : Puzzle
{
    [SerializeField] private Material[] Light;          // 마테리얼의 속성을 변경 하기 위한 Material
    [SerializeField] private LayerMask layer;           // 특정 레이어 체크를 위한 Layer
    private const string PuzzleNumber = "8244";         // 퍼즐의 정답체크를 위해 정답을 담아둔 const string
    private string NumberCheck = string.Empty;          // 퍼즐의 정답 체크를 위한 string
    private bool isChecking = false;                    // 정답을 체크하는 중인지 확인하는 bool
    private GameObject hitObject;                       // 터치한 오브젝트를 담아두는 GameObject

    /// <summary>
    /// 퍼즐 클래스에서 상속된 퍼즐 Press Event
    /// </summary>
    /// <param name="context"></param>
    public override void OnPuzzlePress(InputAction.CallbackContext context)
    {
        if (!CheckTouchEnable()) return;

        if (context.canceled)
        {
            if (hitObject != null) return;

            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

            Ray ray = Camera.main.ScreenPointToRay(touchPosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layer) && !isChecking)
            {
                hitObject = hit.transform.gameObject;
                Buttoninput();
            }
        }
    }

    /// <summary>
    /// 터치된 오브젝트의 값을 넣고 입력된 갯수를 확인하여 정답체크를 호출하는 메소드
    /// </summary>
    private void Buttoninput()
    {
        NumberCheck += hitObject.name;
        hitObject.transform.DOLocalMoveZ(0.394f, 0.15f).SetEase(Ease.Linear).OnComplete(ButtonUp); ;
        int inputStep = NumberCheck.Length - 1;
        Light[inputStep].EnableKeyword("_EMISSION");
        AudioManager.Instance.PlaySFX("SFX_PushButton1");


        if (NumberCheck.Length == 4)
        {
            if (ClearCheck())
            {
                PuzzleClear();
            }
            else
            {
                StartCoroutine(Puzzlefailure());
            }

            isChecking = true;
        }
    }

    /// <summary>
    /// 터치된 오브젝트의 포지션을 다시 원상복구 시키는 메서드
    /// </summary>
    private void ButtonUp()
    {
        hitObject.transform.DOLocalMoveZ(0.412f, 0.15f).SetEase(Ease.Linear);
        hitObject = null;
    }

    /// <summary>
    /// 정답이 틀렸을때 틀린사운드와 시각적 효과를 주는 메서드
    /// </summary>
    /// <returns></returns>
    private IEnumerator Puzzlefailure()
    {
        AudioManager.Instance.PlaySFX("SFX_ErrorSound_2");

        for (int i = 0; i < 3; i++)
        {
            foreach (var material in Light)
            {
                material.DisableKeyword("_EMISSION");
            }

            yield return new WaitForSeconds(0.2f);

            foreach (var material in Light)
            {
                material.EnableKeyword("_EMISSION");
            }

            yield return new WaitForSeconds(0.2f);
        }

        foreach (var material in Light)
        {
            material.DisableKeyword("_EMISSION");
        }

        isChecking = false;
        NumberCheck = string.Empty;
    }

    /// <summary>
    /// 정답을 체크하는 메소드
    /// </summary>
    /// <returns></returns>
    private bool ClearCheck()
    {
        if (PuzzleNumber.Equals(NumberCheck))
        {
            return true;
        }

        return false;
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
