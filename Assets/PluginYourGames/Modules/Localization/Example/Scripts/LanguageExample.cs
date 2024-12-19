using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace YG.Example
{
    public class LanguageExample : MonoBehaviour
    {
        public string ru, en;

        private TextMeshProUGUI textComponent;

        private void Awake()
        {
            textComponent = GetComponent<TextMeshProUGUI>();
            SwitchLanguage(YG2.lang);
        }

        private void OnEnable()
        {
            YG2.onSwitchLang += SwitchLanguage;

            // Проверяем текущий язык при включении объекта
            string currentLanguage = YG2.lang;
            SwitchLanguage(currentLanguage);
        }
        private void OnDisable()
        {
            YG2.onSwitchLang -= SwitchLanguage;
        }

        public void SwitchLanguage(string lang)
        {
            switch (lang)
            {
                case "ru":
                    textComponent.text = ru;
                    break;
                case "tr":
                    //textComponent.text = tr;
                    break;
                default:
                    textComponent.text = en;
                    break;
            }
        }
    }
}