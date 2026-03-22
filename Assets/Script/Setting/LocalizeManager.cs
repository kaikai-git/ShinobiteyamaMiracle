using UnityEngine;
using UnityEngine.Localization.Settings;
using System.Threading.Tasks;


namespace Setting
{
    //言語変更を管理するクラス
    public class LocalizeManager : MonoBehaviour
    {
        public enum LanguageType
        {
            English,
            Japanese,
        }
        static LanguageType currentLanguageType = LanguageType.Japanese;

      


        public static async Task ChangeLocale(LanguageType languageType)
        {
            string key = "";
            switch(languageType)
            {
                case LanguageType.English:
                    key = "en";
                    break;

                case LanguageType.Japanese:
                    key = "ja";
                    break;
            }
            // 指定したLocaleを取得
            var locale = LocalizationSettings.AvailableLocales.Locales.Find((x) => x.Identifier.Code == key);

            // Localeの設定
            LocalizationSettings.SelectedLocale = locale;

            // 初期化待ち
            await LocalizationSettings.InitializationOperation.Task;
        }
    }

}

