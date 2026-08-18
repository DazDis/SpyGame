using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class PlayerInputManager : MonoBehaviour
{
    public GameObject inputPrefab;          // префаб с InputField и Toggle
    public Transform container;             // родитель для полей
    public TMP_InputField customWordInput;  // поле для своего слова (кастомный режим)
    public GameObject CustomWord;  // поле для своего слова (кастомный режим)
    public GameObject CommonWord;  // поле для общего слова (обычный режим)

    public bool isCustomMode = false;       // устанавливается из GameManager

    private int playerCount = 0;

    void Start()
    {
        AddPlayerField();
    }
    private void OnEnable()
    {
        if (isCustomMode) {CustomWord.SetActive(true); CommonWord.SetActive(false); }
        else {CustomWord.SetActive(false); CommonWord.SetActive(true);}
        foreach (Transform child in container)
        {
            Toggle spyToggle = child.GetComponentInChildren<Toggle>(true);
            spyToggle.gameObject.SetActive(isCustomMode);
        }
    }
    void AddPlayerField()
    {
        playerCount++;
        GameObject newField = Instantiate(inputPrefab, container);

        TMP_InputField input = newField.GetComponentInChildren<TMP_InputField>();
        Toggle spyToggle = newField.GetComponentInChildren<Toggle>(true);

        if (spyToggle != null)
            spyToggle.gameObject.SetActive(isCustomMode);

        input.onEndEdit.AddListener(delegate {
            if (IsLastField(newField) && !string.IsNullOrEmpty(input.text))
                AddPlayerField();
        });
    }

    bool IsLastField(GameObject field)
    {
        if (container.childCount == 0) return false;
        Transform lastChild = container.GetChild(container.childCount - 1);
        return field.transform == lastChild;
    }

    public List<string> GetPlayerNames()
    {
        List<string> names = new List<string>();
        foreach (Transform child in container)
        {
            TMP_InputField input = child.GetComponentInChildren<TMP_InputField>();
            if (input != null && !string.IsNullOrEmpty(input.text))
                names.Add(input.text);
        }
        return names;
    }

    public List<PlayerData> GetPlayersWithSpyStatus()
    {
        List<PlayerData> result = new List<PlayerData>();
        foreach (Transform child in container)
        {
            TMP_InputField input = child.GetComponentInChildren<TMP_InputField>();
            if (input == null || string.IsNullOrEmpty(input.text))
                continue;

            Toggle spyToggle = child.GetComponentInChildren<Toggle>();
            bool isSpy = spyToggle != null && spyToggle.isOn;

            result.Add(new PlayerData { Name = input.text, IsSpy = isSpy });
        }
        return result;
    }
    public void ResetCustomWord()
    {
        customWordInput.text = "";
    }
}