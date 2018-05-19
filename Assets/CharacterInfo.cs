
using UnityEngine;

using UnityEngine.UI;

namespace Reboot
{
    public class CharacterInfo : MonoBehaviour
    {
        public Unit associatedUnit;

        public Text unitName;
        public Image unitImage;
        public Text unitHealth;

        public void UpdateInfo()
        {
            unitName.text = associatedUnit.unitName;
            unitImage = associatedUnit.characterImage;
            unitHealth.text = string.Format("{0}/{1}", associatedUnit.Health ,associatedUnit.MaxHealth);
        }
    }
}
