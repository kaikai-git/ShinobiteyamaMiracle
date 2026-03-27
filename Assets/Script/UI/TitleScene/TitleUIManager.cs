using Setting;
using TMPro;
using UnityEngine;

namespace UI
{
    public class TitleUIManager : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown languageDropDown;
        [SerializeField] GameObject loadGameList;
        [SerializeField] GameObject defaultUIList;

        [SerializeField] AudioClip selectSE;    //UIを選んだ時の音
        [SerializeField] AudioClip decideSE;    //UIを決定した時の音



        void Start()
        {
            loadGameList.SetActive(false);
            languageDropDown.onValueChanged.AddListener(OnSetLocalize);
        }

        void OnSetLocalize(int index)
        {
            string selectedText = languageDropDown.options[index].text;
            Debug.Log(index + " / " + selectedText);

            switch(index)
            {
                case 0:
                LocalizeManager.ChangeLocale(LocalizeManager.LanguageType.Japanese);
                break;

                case 1:
                LocalizeManager.ChangeLocale(LocalizeManager.LanguageType.English);
                break;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnNewGame()
        {
            
        }

        public void OnOpenContinueMenu()
        {
            Debug.Log($"Instance: {SoundManager.Instance}");
            Debug.Log($"selectSE: {selectSE}");

            SoundManager.Instance.PlaySE(SEType.CANCEL_UI);

            loadGameList.SetActive(true);
            defaultUIList.SetActive(false);
        }

        public void OnCloseContinueMenu()
        {
            loadGameList.SetActive(false);
            defaultUIList.SetActive(true);

        }


        public void OnExitGame()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
            #else
                Application.Quit();//ゲームプレイ終了
            #endif
        }
    }
}


