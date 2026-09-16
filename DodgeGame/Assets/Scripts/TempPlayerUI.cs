using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TempPlayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerHealthText;

    public void RefreshHealthUI(int health)
    {
        _playerHealthText.text = health.ToString();
    }
}
