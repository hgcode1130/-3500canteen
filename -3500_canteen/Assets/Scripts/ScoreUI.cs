using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

    private void Start()
    {
        // ���������ɹ��¼�
        DeliveryManager.Instance.OnRecipeSucess += OnRecipeSucess;
        RefreshScore();
    }

    private void OnDestroy()
    {
        DeliveryManager.Instance.OnRecipeSucess -= OnRecipeSucess;
    }

    private void OnRecipeSucess(object sender, System.EventArgs e)
    {
        RefreshScore();
    }

    private void RefreshScore()
    {
        int value = DeliveryManager.Instance.GetTotalEarnedValue();
        scoreText.text = $"�ܵ÷֣�{value}";
    }
}
