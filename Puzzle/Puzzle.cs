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
    /// 퍼즐을 터치할 수 있는지 확인합니다.
    /// 현재 뷰가 SubView이며, 대화 중이 아니고,
    /// 현재 카메라가 지정된 카메라 목록에 포함되어 있는지 검사합니다.
    /// </summary>
    /// <returns>터치 가능 여부</returns>
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
    /// 퍼즐을 누를 때 호출되는 추상 메서드입니다.
    /// 구체적인 퍼즐 동작은 상속받은 클래스에서 구현
    /// </summary>
    /// <param name="context">입력 컨텍스트</param>
    public abstract void OnPuzzlePress(InputAction.CallbackContext context);

    /// <summary>
    /// 퍼즐의 위치 조작 시 호출되는 추상 메서드입니다.
    /// 구체적인 퍼즐 동작은 상속받은 클래스에서 구현
    /// </summary>
    /// <param name="context">입력 컨텍스트</param>
    public abstract void OnPuzzlePosition(InputAction.CallbackContext context);

    /// <summary>
    /// 퍼즐이 클리어되었을 때 호출됩니다.
    /// 클리어 동작을 수행하고 퍼즐의 동작을 비활성화합니다.
    /// 이후 게임 데이터를 저장합니다.
    /// </summary>
    public void PuzzleClear()
    {
        clearAction.PuzzleInteract();
        enabled = false;
        SaveManager.Instance.SaveGame();
    }
}
