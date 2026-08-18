using TMPro;
using UnityEngine;
using UnityEngine.UI; // или TMPro, если используешь TextMeshPro

public class WordLoader : MonoBehaviour
{
    [System.Serializable]
    public class Category
    {
        public string name;
        public string[] words;
    }

    [System.Serializable]
    public class WordData
    {
        public Category[] categories;
    }

    [Header("UI")]
    public TMP_Text categoryNameText;    // перетащи сюда Text с названием категории
    public Button nextButton;        // кнопка "вправо"
    public Button prevButton;        // кнопка "влево"

    private WordData wordData;
    private string[] currentCategoryWords;
    private int currentCategoryIndex = 0;

    void Awake()
    {
        // Загружаем JSON
        TextAsset json = Resources.Load<TextAsset>("words");
        if (json != null)
        {
            wordData = JsonUtility.FromJson<WordData>(json.text);
            if (wordData != null && wordData.categories.Length > 0)
            {
                currentCategoryIndex = 0;
                UpdateCurrentCategory();  // показываем первую категорию
            }
            else
                Debug.LogError("В JSON нет категорий");
        }
        else
            Debug.LogError("Файл words.json не найден в папке Resources");
    }

    void Start()
    {
        // Подписываем кнопки
        if (nextButton != null)
            nextButton.onClick.AddListener(NextCategory);
        if (prevButton != null)
            prevButton.onClick.AddListener(PreviousCategory);
    }

    // --- Методы для стрелок ---
    public void NextCategory()
    {
        if (wordData == null || wordData.categories.Length == 0) return;
        currentCategoryIndex = (currentCategoryIndex + 1) % wordData.categories.Length;
        UpdateCurrentCategory();
    }

    public void PreviousCategory()
    {
        if (wordData == null || wordData.categories.Length == 0) return;
        currentCategoryIndex--;
        if (currentCategoryIndex < 0)
            currentCategoryIndex = wordData.categories.Length - 1;
        UpdateCurrentCategory();
    }

    // --- Обновление текущей категории ---
    private void UpdateCurrentCategory()
    {
        if (wordData == null || wordData.categories.Length == 0) return;

        Category current = wordData.categories[currentCategoryIndex];
        currentCategoryWords = current.words;

        if (categoryNameText != null)
            categoryNameText.text = current.name;
    }

    // --- Геттеры для других скриптов ---
    public string[] GetCurrentWords() => currentCategoryWords;
    public string GetCurrentCategoryName() => wordData?.categories[currentCategoryIndex]?.name ?? "";
}