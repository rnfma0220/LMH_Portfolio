using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Puzzle : MonoBehaviour
{
    [SerializeField] List<CinemachineVirtualCamera> assignedCameras;
    [SerializeField] InteractionController clearAction;
    private PlayerInput player;

    private void Awake()
    {
        player = FindObjectOfType<PlayerInput>();
    }

    private void OnEnable()
    {
        if(player == null) player = FindObjectOfType<PlayerInput>();

        player.actionEvents[1].AddListener(OnPuzzlePress);
        player.actionEvents[2].AddListener(OnPuzzlePosition);
    }
    private void OnDisable()
    {
        player.actionEvents[1].RemoveListener(OnPuzzlePress);
        player.actionEvents[2].RemoveListener(OnPuzzlePosition);
    }

    /// <summary>
    /// ������ ��ġ�� �� �ִ��� Ȯ���մϴ�.
    /// ���� �䰡 SubView�̸�, ��ȭ ���� �ƴϰ�,
    /// ���� ī�޶� ������ ī�޶� ��Ͽ� ���ԵǾ� �ִ��� �˻��մϴ�.
    /// </summary>
    /// <returns>��ġ ���� ����</returns>
    public bool CheckTouchEnable()
    {
        if (ViewManager.Instance.currentView == View.SubView && 
            !ViewManager.Instance.isDialog &&
            assignedCameras.Contains(ViewManager.Instance.currentCamera))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// ������ ���� �� ȣ��Ǵ� �߻� �޼����Դϴ�.
    /// ��ü���� ���� ������ ��ӹ��� Ŭ�������� ����
    /// </summary>
    /// <param name="context">�Է� ���ؽ�Ʈ</param>
    public abstract void OnPuzzlePress(InputAction.CallbackContext context);

    /// <summary>
    /// ������ ��ġ ���� �� ȣ��Ǵ� �߻� �޼����Դϴ�.
    /// ��ü���� ���� ������ ��ӹ��� Ŭ�������� ����
    /// </summary>
    /// <param name="context">�Է� ���ؽ�Ʈ</param>
    public abstract void OnPuzzlePosition(InputAction.CallbackContext context);

    /// <summary>
    /// ������ Ŭ����Ǿ��� �� ȣ��˴ϴ�.
    /// Ŭ���� ������ �����ϰ� ������ ������ ��Ȱ��ȭ�մϴ�.
    /// ���� ���� �����͸� �����մϴ�.
    /// </summary>
    public void PuzzleClear()
    {
        clearAction.PuzzleInteract();
        enabled = false;
        SaveManager.Instance.SaveGame();
    }
}
