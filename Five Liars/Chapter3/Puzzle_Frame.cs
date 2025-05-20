using UnityEngine;
using UnityEngine.InputSystem;

public class Puzzle_Frame : Puzzle
{
    public Material[] Inlinematerials;                                  // 마테리얼의 속성을 변경 하기 위한 Material 배열
    public Material[] Outlinematerials;                                 // 마테리얼의 속성을 변경 하기 위한 Material 배열
    [SerializeField] private Puzzle_SunFlowerFrame sunFlowerFrames;     // 정답체크 하기위해 선언한 퍼즐 클래스
    [SerializeField] private LayerMask layer;                           // 특정 레이어 체크를 위한 Layer

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
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layer))
            {
                if (hit.transform.name.Equals("ClearButton"))
                {
                    AudioManager.Instance.PlaySFX("SFX_PushButton2");
                    ClearCheck();
                }
            }
        }
    }

    /// <summary>
    /// 정답 체크하는 체크하는 메소드
    /// </summary>
    private void ClearCheck()
    {
        if (!sunFlowerFrames.ClearCheck()) return;

        sunFlowerFrames.enabled = false;
        AudioManager.Instance.PlaySFX("SFX_UseKey");
        PuzzleClear();
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
