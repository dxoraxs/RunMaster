using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    [SerializeField] private Text coinText;

    private void OnEnable()
    {
        EventManager.OnCoinTextUpdate += UpdateText;
    }

    private void OnDisable()
    {
        EventManager.OnCoinTextUpdate -= UpdateText;
    }

    private void UpdateText(int value) => coinText.text = value.ToString();
}
