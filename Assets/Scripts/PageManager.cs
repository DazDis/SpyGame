using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PageManager : MonoBehaviour
{
    public GameManager gameManager;
    [Header("Панели")]
    public GameObject mainMenuPanel;
    public GameObject inputPanel;
    public GameObject roleRevealPanel;   // страница 3
    public GameObject wordDisplayPanel;  // страница 4

    [Header("Элементы UI")]
    public TMP_Text roleNameText;            // на странице 3 – имя игрока
    public TMP_Text wordDisplayText;         // на странице 4 – слово или "шпион"

    private void Start()
    {
        ShowMainMenu();
    }
    public void OnNormalGameButton()
    {
        gameManager.StartNormalGame();
    }

    public void OnCustomGameButton()
    {
        gameManager.StartCustomGame();
    }
    public void ShowMainMenu()
    {
        SetAllPanels(false);
        mainMenuPanel.SetActive(true);
    }

    public void ShowInputPanel()
    {
        SetAllPanels(false);
        inputPanel.SetActive(true);
    }

    public void ShowRoleReveal(string playerName)
    {
        SetAllPanels(false);
        roleRevealPanel.SetActive(true);
        if (roleNameText != null)
            roleNameText.text = playerName;
    }

    public void ShowWordDisplay(string message)
    {
        SetAllPanels(false);
        wordDisplayPanel.SetActive(true);
        if (wordDisplayText != null)
            wordDisplayText.text = message;
    }

    private void SetAllPanels(bool active)
    {
        mainMenuPanel.SetActive(active);
        inputPanel.SetActive(active);
        roleRevealPanel.SetActive(active);
        wordDisplayPanel.SetActive(active);
    }

}