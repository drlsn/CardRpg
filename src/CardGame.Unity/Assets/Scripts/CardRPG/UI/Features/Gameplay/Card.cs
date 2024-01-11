using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardRPG.UI.Gameplay
{
    public class Card : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nameText;

        [SerializeField] private TMP_Text _hpText;
        [SerializeField] private TMP_Text _attackText;

        [SerializeField] private Button _cardButton;

        [SerializeField] private Image _image;

        public delegate void OnCardSelectedDelegate(Entities.Gameplay.Card card, bool isEnemy);
        public event OnCardSelectedDelegate OnCardSelected;

        private Entities.Gameplay.Card _card;
        private bool _isEnemy;

        private static CardImages _cardImages;

        private void Awake()
        {
            _cardImages ??= FindObjectOfType<CardImages>();
        }

        public void Init(Entities.Gameplay.Card card, bool isEnemy)
        {
            _card = card;
            _isEnemy = isEnemy;

            _nameText.text = card.Name;
            _hpText.text = card.Statistics.HP.CalculatedValue.ToString() + " HP";
            _attackText.text = card.Statistics.Attack.CalculatedValue.ToString() + " AT";

            _image.sprite = _cardImages.Sprites.GetRandom();

            var rt = _image.rectTransform;

            var ratio = _image.sprite.texture.width / _image.sprite.texture.height;
            var hDiv = _image.rectTransform.sizeDelta.y / _image.sprite.texture.height;

            _image.rectTransform.sizeDelta = new(rt.sizeDelta.x * hDiv * 2, rt.sizeDelta.y);

            _cardButton.onClick.AddListener(() => OnCardSelected?.Invoke(_card, _isEnemy));
        }
    }
}

public static class RandomExtensions
{
    public static T GetRandom<T>(this IList<T> source, System.Random random = null)
    {
        random ??= new System.Random();
        int idx = random.Next(source.Count() - 1);
        return source[idx];
    }
}
