using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndLevelMenu : MonoBehaviour
{
    [SerializeField] private Text coin;

    private void OnEnable()
    {
        coin.text = "Your rewards: " + GameManager.Instance.GetCountReward + "\nYour coins: " + GameManager.Instance.GetCountCoin;
    }

    public void OnClickNextLevelButton()
    {
        SceneManager.LoadScene(0);
    }
}