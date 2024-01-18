using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardRPG.UI
{
    internal class CardImages : MonoBehaviour
    {
        public List<SpriteGroup> Sprites;
        public List<SpriteGroup> ReverseSprites;

        public Sprite GetAvers(int group = 0) =>
            Sprites[group].Values.GetRandom();

        public Sprite GetRevers(int group = 0) =>
            ReverseSprites[group].Values.GetRandom();
    }

    [Serializable]
    public class SpriteGroup
    {
        public List<Sprite> Values;
    }
}
