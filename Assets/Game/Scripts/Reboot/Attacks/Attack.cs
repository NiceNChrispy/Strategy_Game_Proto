using System;
using UnityEngine;

namespace Reboot
{
    [Serializable]
    public class AttackData
    {
        public string Name;
        public int Range;
        public int Cost;
        public int Damage;
    }

    [CreateAssetMenu(fileName = "Attack", menuName = "Reboot/Attack")]
    public class AttackBehaviour : ScriptableObject
    {
        [SerializeField] private AttackData m_AttackData;
        [SerializeField] private AttackEffect m_AttackEffect;
        public event Action OnHit;

        public string Name { get { return Data.Name; } }
        public int Range   { get { return Data.Range; } }
        public int Cost    { get { return Data.Cost; } }
        public int Damage  { get { return Data.Damage; } }

        public AttackData Data
        {
            get
            {
                return m_AttackData;
            }
        }
        public AttackEffect Effect
        {
            get
            {
                return m_AttackEffect;
            }
        }
    }
}