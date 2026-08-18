using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerInputManager inputManager;
    public PageManager pageManager;
    public WordLoader wordLoader;

    [SerializeField] private List<PlayerData> players = new List<PlayerData>();
    private int currentPlayerIndex = -1;
    public string CurrentWord;

    public bool IsCustom;


    public void StartNormalGame()
    {
        IsCustom = false;
        inputManager.isCustomMode = false;
        pageManager.ShowInputPanel();
    }

    public void StartCustomGame()
    {
        IsCustom = true;
        inputManager.isCustomMode = true;
        pageManager.ShowInputPanel();
    }

    public void StartGame()
    {
        if (!IsCustom)
        {
            List<string> names = inputManager.GetPlayerNames();
            if (names.Count < 2)
            {
                Debug.LogWarning("Нужно минимум 2 игрока!");
                return;
            }

            players.Clear();
            foreach (string name in names)
            {
                players.Add(new PlayerData { Name = name, IsSpy = false });
            }

            MakeSpy();
            string[] categoryWords = wordLoader.GetCurrentWords();
            if (categoryWords != null && categoryWords.Length > 0)
                CurrentWord = categoryWords[Random.Range(0, categoryWords.Length)];
            else
                CurrentWord = "Слово не найдено";
        }
        else
        {
            List<PlayerData> customPlayers = inputManager.GetPlayersWithSpyStatus();
            if (customPlayers.Count < 2)
            {
                Debug.LogWarning("Нужно минимум 2 игрока!");
                return;
            }

            players = customPlayers;

            string customWord = inputManager.customWordInput.text;
            if (string.IsNullOrEmpty(customWord))
            {
                Debug.LogWarning("Введите слово для игры!");
                return;
            }
            CurrentWord = customWord;

        }

        currentPlayerIndex = 0;
        pageManager.ShowRoleReveal(players[currentPlayerIndex].Name);
    }

    private void MakeSpy()
    {
        if (players.Count == 0) return;
        int spyIndex = Random.Range(0, players.Count);
        players[spyIndex].IsSpy = true;
        Debug.Log($"Шпион назначен: {players[spyIndex].Name}");
    }

    public void RevealCurrentPlayer()
    {
        if (currentPlayerIndex < 0 || currentPlayerIndex >= players.Count)
            return;

        PlayerData player = players[currentPlayerIndex];
        if (player.IsSpy)
            pageManager.ShowWordDisplay("Ты шпион!");
        else
            pageManager.ShowWordDisplay($"Твоё слово: {CurrentWord}");
    }

    public void NextPlayer()
    {
        currentPlayerIndex++;
        if (currentPlayerIndex < players.Count)
            pageManager.ShowRoleReveal(players[currentPlayerIndex].Name);
        else
        {
            pageManager.ShowInputPanel();
            inputManager.ResetCustomWord();
        }
    }
}