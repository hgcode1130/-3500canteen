// // using System.Collections;
// // using System.Collections.Generic;
// // using TMPro;
// // using UnityEngine;

// // public class ScoreUI : MonoBehaviour
// // {
// //     [SerializeField]
// //     private TextMeshProUGUI scoreText;

// //     private IEnumerator Start()
// //     {
// //         yield return new WaitUntil(() => DeliveryManager.Instance != null);

// //         Debug.Log(
// //             $"ScoreUI ({GetInstanceID()}) ���� DeliveryManager ({DeliveryManager.Instance.GetInstanceID()}) �� OnRecipeSucess �¼�"
// //         );
// //         DeliveryManager.Instance.OnRecipeSucess += OnRecipeSucess;
// //         DeliveryManager.Instance.OnScoreUpdated += OnScoreUpdated;
// //         RefreshScore();
// //     }

// //     private void OnDestroy()
// //     {
// //         if (DeliveryManager.Instance != null)
// //         {
// //             Debug.Log("ȡ������ OnRecipeSucess �¼�");
// //             DeliveryManager.Instance.OnRecipeSucess -= OnRecipeSucess;
// //             DeliveryManager.Instance.OnScoreUpdated -= OnScoreUpdated;
// //         }
// //     }

// //     private void OnRecipeSucess(object sender, System.EventArgs e)
// //     {
// //         Debug.Log("OnRecipeSucess �¼�������ˢ�µ÷�");
// //         RefreshScore();
// //     }

// //     private void OnScoreUpdated(object sender, int newTotalValue)
// //     {
// //         Debug.Log("OnScoreUpdated �¼�������ˢ�µ÷�");
// //         RefreshScore();
// //         // scoreText.text = $"�ܵ÷֣�{newTotalValue}";
// //     }

// //     private void RefreshScore()
// //     {
// //         if (DeliveryManager.Instance == null)
// //         {
// //             Debug.LogError("DeliveryManager.Instance Ϊ null");
// //             return;
// //         }

// //         int value = DeliveryManager.Instance.GetTotalEarnedValue();
// //         Debug.Log($"��ǰ�ܵ÷֣�{value}");
// //         if (scoreText != null)
// //         {
// //             scoreText.text = $"�ܵ÷֣�{value}";
// //         }
// //         else
// //         {
// //             Debug.LogError("scoreText δ��ֵ");
// //         }
// //     }
// // }
// using System.Collections;
// using System.Collections.Generic;
// using TMPro;
// using Unity.Netcode;
// using UnityEngine;

// public class ScoreUI : MonoBehaviour
// {
//     [SerializeField]
//     private TextMeshProUGUI scoreText;

//     private IEnumerator Start()
//     {
//         Debug.Log("[ScoreUI] �ȴ� DeliveryManager ��ʼ��...");
//         yield return new WaitUntil(() => DeliveryManager.Instance != null);

//         Debug.Log("[ScoreUI] ���� OnScoreUpdated �¼�");
//         DeliveryManager.Instance.OnScoreUpdated += OnScoreUpdated;
//         RefreshScore();
//     }

//     private void OnDestroy()
//     {
//         if (DeliveryManager.Instance != null)
//         {
//             Debug.Log("[ScoreUI] ȡ������ OnScoreUpdated �¼�");
//             DeliveryManager.Instance.OnScoreUpdated -= OnScoreUpdated;
//         }
//     }

//     private void OnScoreUpdated(object sender, int newScore)
//     {
//         Debug.Log($"[ScoreUI] �յ��������£�{newScore}");
//         RefreshScore();
//     }

//     private void RefreshScore()
//     {
//         if (scoreText == null)
//         {
//             Debug.LogError("[ScoreUI] scoreText δ��ֵ");
//             return;
//         }

//         // ��ȡ������ҷ���
//         ulong localClientId = NetworkManager.Singleton.LocalClientId;
//         int value = DeliveryManager.Instance.GetPlayerScore(localClientId);
//         Debug.Log($"[ScoreUI] ˢ�·�����{value}");
//         scoreText.text = $"�ܵ÷֣�{value}";
//     }
// }
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

    private IEnumerator Start()
    {
        Debug.Log("[ScoreUI] �ȴ� DeliveryManager ��ʼ��...");
        yield return new WaitUntil(() => DeliveryManager.Instance != null);

        Debug.Log("[ScoreUI] ���� OnScoreUpdated �¼�");
        DeliveryManager.Instance.OnScoreUpdated += OnScoreUpdated;
        RefreshScore();
    }

    private void OnDestroy()
    {
        if (DeliveryManager.Instance != null)
        {
            Debug.Log("[ScoreUI] ȡ������ OnScoreUpdated �¼�");
            DeliveryManager.Instance.OnScoreUpdated -= OnScoreUpdated;
        }
    }

    private void OnScoreUpdated(object sender, int newScore)
    {
        Debug.Log($"[ScoreUI] �յ��������£�{newScore}");
        RefreshScore();
    }

    private void RefreshScore()
    {
        if (scoreText == null)
        {
            Debug.LogError("[ScoreUI] scoreText δ��ֵ");
            return;
        }

        // ��ȡ������ҷ���
        ulong localClientId = NetworkManager.Singleton.LocalClientId;
        int value = DeliveryManager.Instance.GetPlayerScore(localClientId);
        Debug.Log($"[ScoreUI] ˢ�·�����{value}");
        scoreText.text = $"�ܵ÷֣�{value}";
    }
}
