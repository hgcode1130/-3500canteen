// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class DeliveryManagerUI : MonoBehaviour
// {
//     [SerializeField]
//     private List<DeliveryManagerSingleUI> slotUIs;

//     private void Start()
//     {
//         DeliveryManager.Instance.OnRecipeSpawned += OnRecipeChanged;
//         DeliveryManager.Instance.OnRecipeComplete += OnRecipeChanged;
//         DeliveryManager.Instance.OnRecipeFailed += OnRecipeChanged;
//         UpdateVisual();
//     }

//     private void OnRecipeChanged(object sender, EventArgs e)
//     {
//         UpdateVisual();
//     }

//     private void UpdateVisual()
//     {
//         List<WaitingRecipe> waitingRecipes = DeliveryManager.Instance.GetWaitingRecipeList();

//         for (int i = 0; i < slotUIs.Count; i++)
//         {
//             if (i < waitingRecipes.Count && waitingRecipes[i] != null)
//             {
//                 slotUIs[i].SetWaitingRecipe(waitingRecipes[i]);
//             }
//             else
//             {
//                 slotUIs[i].ClearUI(); // û�ж����Ĳ�λ�������
//             }
//         }
//     }
// }
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField]
    private List<DeliveryManagerSingleUI> slotUIs;

    private void Start()
    {
        // ����ʱ�ȴ� DeliveryManager ������ʼ�����
        StartCoroutine(SetupWhenReady());
    }

    private IEnumerator SetupWhenReady()
    {
        // �ȴ�ֱ�� DeliveryManager.Instance ����ֵ
        yield return new WaitUntil(() => DeliveryManager.Instance != null);

        // �����¼�
        DeliveryManager.Instance.OnRecipeSpawned += HandleRecipeChanged;
        DeliveryManager.Instance.OnRecipeComplete += HandleRecipeChanged;
        DeliveryManager.Instance.OnRecipeFailed += HandleRecipeChanged;

        // �״�ˢ�� UI
        UpdateVisual();
    }

    private void OnDisable()
    {
        // ȡ�����ģ���ֹ�ڴ�й©
        if (DeliveryManager.Instance == null)
            return;

        DeliveryManager.Instance.OnRecipeSpawned -= HandleRecipeChanged;
        DeliveryManager.Instance.OnRecipeComplete -= HandleRecipeChanged;
        DeliveryManager.Instance.OnRecipeFailed -= HandleRecipeChanged;
    }

    private void HandleRecipeChanged(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        var dm = DeliveryManager.Instance;
        if (dm == null)
            return;

        var waitingRecipes = dm.GetWaitingRecipeList();
        if (waitingRecipes == null)
            return;

        for (int i = 0; i < slotUIs.Count; i++)
        {
            var ui = slotUIs[i];
            if (ui == null)
                continue;

            if (i < waitingRecipes.Count && waitingRecipes[i] != null)
            {
                ui.SetWaitingRecipe(waitingRecipes[i]);
            }
            else
            {
                ui.ClearUI();
            }
        }
    }
}