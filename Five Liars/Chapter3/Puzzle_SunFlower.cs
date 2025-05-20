using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Puzzle_SunFlower : Puzzle
{
    [SerializeField] private Material[] materials;      // ���׸����� �Ӽ��� ���� �ϱ� ���� Material
    [SerializeField] private Renderer[] renderers;      // ������ üũ�ϱ����� ��Ƶδ� Renderer �迭
    [SerializeField] private LayerMask layer;           // Ư�� ���̾� üũ�� ���� Layer

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
    /// ������ üũ�ϴ� �޼ҵ�
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
    /// ���� Ŭ�������� ��ӵ� ���� Position Event
    /// </summary>
    /// <param name="context"></param>
    public override void OnPuzzlePosition(InputAction.CallbackContext context)
    {
        return;
    }
}
