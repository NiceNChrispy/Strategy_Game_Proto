using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

namespace Reboot
{
    public class PlayerUI : MonoBehaviour
    {
        [Section("Main UI")]
        [SerializeField] private Canvas m_UICanvas;
        [SerializeField] private GameObject m_UIPanel;
        [SerializeField] private Player m_Player;


        [Section("Side UI")]
        [SerializeField] private GameObject characterUIPrefab;
        [SerializeField] private GameObject characterUI;
        [SerializeField] private GameObject characterUIHolder;
        [SerializeField] private bool chracterDrawOpen;
        [SerializeField] private Image arrow;
        [SerializeField] private List<CharacterInfo> characterList;

        private void OnEnable()
        {
            m_Player.OnSelectUnit += OnPlayerSelectedUnit;
            m_Player.OnDeselectUnit += OnPlayerDeselectedUnit;
            m_UIPanel.SetActive(false);
            PopulateCharacterDraw();
        }

        public void OnPlayerSelectedUnit(Unit unit)
        {
            m_UIPanel.SetActive(true);
        }

        public void OnPlayerDeselectedUnit(Unit unit)
        {
            m_UIPanel.SetActive(false);
        }

        public void EnableOrderButtons()
        {

        }

        public void LoadCharacterUI()
        {

        }

        #region CharacterDraw
        public void PopulateCharacterDraw()
        {
            for (int i = 0; i < m_Player.Units.Count; i++)
            {
                GameObject charInfo = Instantiate(characterUIPrefab, characterUIHolder.transform) as GameObject;
                CharacterInfo info = charInfo.GetComponent<CharacterInfo>();
                info.associatedUnit = m_Player.Units[i];
                characterList.Add(info);
            }
            UpdateCharacterInfo();
        }

        public void UpdateCharacterInfo()
        {
            for (int i = 0; i < characterList.Count; i++)
            {
                characterList[i].UpdateInfo();
            }
        }

        public void OpenCloseDraw(int drawNum)
        {
            switch (drawNum)
            {
                case 0:
                    chracterDrawOpen = !chracterDrawOpen;
                    if (chracterDrawOpen)
                    {
                        iTween.MoveTo(characterUI, iTween.Hash("x", 85, "time", .25f, "easeType", iTween.EaseType.easeOutCirc));
                        arrow.rectTransform.eulerAngles = new Vector3(0f, 0f, -90f);
                    }
                    else
                    {
                        iTween.MoveTo(characterUI, iTween.Hash("x", -90, "time", .25f, "easeType", iTween.EaseType.easeOutCirc));
                        arrow.rectTransform.eulerAngles = new Vector3(0f, 0f, 90f);
                    }
                    break;

                default:
                    break;
            }
        }
        #endregion
    }
}