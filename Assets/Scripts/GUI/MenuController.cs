using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameMenu gameMenu;
    [SerializeField] private EndLevelMenu endLevelMenu;

    private void Start()
    {
        ShowGameMenu();
    }

    public void ShowGameMenu()
    {
        gameMenu.gameObject.SetActive(true);
        endLevelMenu.gameObject.SetActive(false);
    }

    public void ShowEndLevelMenu()
    {
        endLevelMenu.gameObject.SetActive(true);
        gameMenu.gameObject.SetActive(false);
    }
}
