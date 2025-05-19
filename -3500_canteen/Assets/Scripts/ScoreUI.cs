using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// public class ScoreUI : MonoBehaviour
// {
//     [SerializeField]
//     private TextMeshProUGUI scoreText;

//     private void Start()
//     {
//         // ���������ɹ��¼�
//         DeliveryManager.Instance.OnRecipeSucess += OnRecipeSucess;
//         RefreshScore();
//     }

//     private void OnDestroy()
//     {
//         DeliveryManager.Instance.OnRecipeSucess -= OnRecipeSucess;
//     }

//     private void OnRecipeSucess(object sender, System.EventArgs e)
//     {
//         RefreshScore();
//     }

//     private void RefreshScore()
//     {
//         int value = DeliveryManager.Instance.GetTotalEarnedValue();
//         scoreText.text = $"�ܵ÷֣�{value}";
//     }
// }
public class ScoreUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => DeliveryManager.Instance != null);

        Debug.Log(
            $"ScoreUI ({GetInstanceID()}) ���� DeliveryManager ({DeliveryManager.Instance.GetInstanceID()}) �� OnRecipeSucess �¼�"
        );
        DeliveryManager.Instance.OnRecipeSucess += OnRecipeSucess;
        DeliveryManager.Instance.OnScoreUpdated += OnScoreUpdated;
        RefreshScore();
    }

    private void OnDestroy()
    {
        if (DeliveryManager.Instance != null)
        {
            Debug.Log("ȡ������ OnRecipeSucess �¼�");
            DeliveryManager.Instance.OnRecipeSucess -= OnRecipeSucess;
            DeliveryManager.Instance.OnScoreUpdated -= OnScoreUpdated;
        }
    }

    private void OnRecipeSucess(object sender, System.EventArgs e)
    {
        Debug.Log("OnRecipeSucess �¼�������ˢ�µ÷�");
        RefreshScore();
    }

    private void OnScoreUpdated(object sender, int newTotalValue)
    {
        Debug.Log("OnScoreUpdated �¼�������ˢ�µ÷�");
        RefreshScore();
        // scoreText.text = $"�ܵ÷֣�{newTotalValue}";
    }

    private void RefreshScore()
    {
        if (DeliveryManager.Instance == null)
        {
            Debug.LogError("DeliveryManager.Instance Ϊ null");
            return;
        }

        int value = DeliveryManager.Instance.GetTotalEarnedValue();
        Debug.Log($"��ǰ�ܵ÷֣�{value}");
        if (scoreText != null)
        {
            scoreText.text = $"�ܵ÷֣�{value}";
        }
        else
        {
            Debug.LogError("scoreText δ��ֵ");
        }
    }
}
