using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Puzzle_SunFlower : Puzzle
{
    [SerializeField] private Material[] materials;      // 마테리얼의 속성을 변경 하기 위한 Material
    [SerializeField] private Renderer[] renderers;      // 정답을 체크하기위해 담아두는 Renderer 배열
    [SerializeField] private LayerMask layer;           // 특정 레이어 체크를 위한 Layer

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
                Renderer renderer = hit.transform.gameObject.GetComponent<Renderer>();

                Material currentMaterial = renderer.sharedMaterial;

                int index = Array.IndexOf(materials, currentMaterial);

                if (index != -1)
                {
                    int nextIndex = (index + 1) % materials.Length;

                    renderer.sharedMaterial = materials[nextIndex];
                }

                AudioManager.Instance.PlaySFX("SFX_PushButton1");

                ClearCheck();
            }
        }
    }

    /// <summary>
    /// 정답을 체크하는 메소드
    /// </summary>
    private void ClearCheck()
    {
        int[] targetOrder = { 3, 2, 0, 1 };

        for (int i = 0; i < renderers.Length; i++)
        {
            Material currentMaterial = renderers[i].sharedMaterial;

            if (currentMaterial != materials[targetOrder[i]])
            {
                return;
            }
        }

        AudioManager.Instance.PlaySFX("SFX_FallFrame");
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
