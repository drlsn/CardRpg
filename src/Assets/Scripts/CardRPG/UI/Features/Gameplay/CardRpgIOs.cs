using Core.Unity;
using System;

namespace CardRPG.UI.Gameplay
{
    public static class CardRpgIOs
    {
        [Serializable] public class BoardIO : InstantiationObject<Board>, IInstantiationObject<Board> { }
        [Serializable] public class CardIOList : InstantiationObjectList<Card>, IInstantiationObjectList<Card> { }
    }
}
