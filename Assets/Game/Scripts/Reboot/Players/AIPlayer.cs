using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reboot
{
    public class AIPlayer : Player
    {
        [SerializeField] private bool m_IsBusy = false;

        private void Update()
        {
            if (m_IsMyTurn && !m_IsBusy && m_ActionPoints > 0)
            {
                m_IsBusy = true;
                SelectUnit(m_Units[Random.Range(0, m_Units.Count)]);

                SetOrder_BUTTON(Random.Range(1,1)); //Pick a random order
                TargetNode(m_MoveableTiles[Random.Range(0, m_MoveableTiles.Count)]); //Select random tile from moveable tiles
                ConfirmOrder();// Excecute order
            }
        }

        protected override void OnUnitFinishedAction()
        {
            m_IsBusy = false;
            base.OnUnitFinishedAction();
        }
    }
}