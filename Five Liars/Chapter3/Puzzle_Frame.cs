using UnityEngine;
using UnityEngine.InputSystem;

public class Puzzle_Frame : Puzzle
{
    public Material[] Inlinematerials;                                  // ���׸����� �Ӽ��� ���� �ϱ� ���� Material �迭
    public Material[] Outlinematerials;                                 // ���׸����� �Ӽ��� ���� �ϱ� ���� Material �迭
    [SerializeField] private Puzzle_SunFlowerFrame sunFlowerFrames;     // ����üũ �ϱ����� ������ ���� Ŭ����
    [SerializeField] private LayerMask layer;                           // Ư�� ���̾� üũ�� ���� Layer

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
    /// ���� üũ�ϴ� üũ�ϴ� �޼ҵ�
    /// </summary>
    private void ClearCheck()
    {
        if (!sunFlowerFrames.ClearCheck()) return;

        sunFlowerFrames.enabled = false;
        AudioManager.Instance.PlaySFX("SFX_UseKey");
        PuzzleClear();
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
