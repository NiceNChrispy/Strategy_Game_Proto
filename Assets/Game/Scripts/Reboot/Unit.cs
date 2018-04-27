using UnityEngine;

namespace Reboot
{
    public class Unit : MonoBehaviour, ISelectable
    {
        [SerializeField, Range(0, 10)] private int m_MaxMoveDistance;
        private Map m_Map;

        public bool IsSelectable
        {
            get; set;
        }

        private void OnEnable()
        {
            IsSelectable = true;
        }

        public void Select()
        {
            Debug.Log("Selected");
        }

        public void Deselect()
        {
            Debug.Log("Deselected");
        }

        public void Move(Vertex<Hex> hex)
        {

        }

        public void Attack()
        {

        }
    }
}