using TMPro;
using UnityEngine;

namespace CardRPG.UI.Gameplay
{
    public class Card : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nameText;

        [SerializeField] private TMP_Text _hpText;
        [SerializeField] private TMP_Text _attackText;

        public void Init(Entities.Gameplay.Card card)
        {
            _nameText.text = card.Name;
            _hpText.text = card.Statistics.HP.CalculatedValue.ToString() + " HP";
            _attackText.text = card.Statistics.Attack.CalculatedValue.ToString() + " AT";
        }
    }
}
