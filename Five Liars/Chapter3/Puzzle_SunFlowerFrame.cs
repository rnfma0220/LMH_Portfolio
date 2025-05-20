using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Puzzle_SunFlowerFrame : Puzzle
{
    private Puzzle_Frame frame;                                 
    private Material[] inlinematerials;                         // ���׸����� �Ӽ��� ���� �ϱ� ���� Material
    private Material[] outlinematerials;                        // ���׸����� �Ӽ��� ���� �ϱ� ���� Material
    [SerializeField] private Renderer[] inlinerenderers;        // ������ üũ�ϱ����� ��Ƶδ� Renderer �迭
    [SerializeField] private Renderer[] outlinerenderers;       // ������ üũ�ϱ����� ��Ƶδ� Renderer �迭
    [SerializeField] private LayerMask layer;                   // Ư�� ���̾� üũ�� ���� Layer

    private void Awake()
    {
        TryGetComponent(out frame);
        inlinematerials = frame.Inlinematerials;
        outlinematerials = frame.Outlinematerials;
    }

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
                if(hit.transform.name.Equals("Button"))
                {
                    Renderer renderer = hit.transform.gameObject.GetComponent<Renderer>();
                    Material currentMaterial = renderer.sharedMaterial;
                    int index = Array.IndexOf(outlinematerials, currentMaterial);
                    if (index != -1)
                    {
                        int nextIndex = (index + 1) % outlinematerials.Length;
                        renderer.sharedMaterial = outlinematerials[nextIndex];
                    }
                }
                else
                {
                    if(hit.transform.TryGetComponent(out Renderer renderer))
                    {
                        Material currentMaterial = renderer.sharedMaterial;
                        int index = Array.IndexOf(inlinematerials, currentMaterial);
                        if (index != -1)
                        {
                            int nextIndex = (index + 1) % inlinematerials.Length;
                            renderer.sharedMaterial = inlinematerials[nextIndex];
                        }
                    }
                }

                AudioManager.Instance.PlaySFX("SFX_PushButton1");
            }
        }
    }

    /// <summary>
    /// ������ ���� üũ�ϴ� �޼ҵ�
    /// </summary>
    /// <returns></returns>
    public bool ClearCheck()
    {
        int[] intarget = { 1, 0, 1, 0, 1, 0, 1, 0 };
        int[] outtarget = { 0, 0 };

        for (int i = 0; i < inlinerenderers.Length; i++)
        {
            Material currentMaterial = inlinerenderers[i].sharedMaterial;

            if (currentMaterial != inlinematerials[intarget[i]])
            {
                AudioManager.Instance.PlaySFX("SFX_ErrorSound_2");
                return false;
            }
        }

        for (int i = 0; i < outlinerenderers.Length; i++)
        {
            Material currentMaterial = outlinerenderers[i].sharedMaterial;

            if (currentMaterial != outlinematerials[outtarget[i]])
            {
                AudioManager.Instance.PlaySFX("SFX_ErrorSound_2");
                return false;
            }
        }

        return true;
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
