//using Core.Collections;
//using System.Linq;
//using UnityEngine;

//namespace Core.Unity.UI
//{
//    [ExecuteAlways]
//    public class FlexList : MonoBehaviour
//    {
//        [SerializeField] private FlexDirection _direction;
//        [SerializeField] private FlexJustify _justify;
//        [SerializeField] private FlexAlign _align;
//        [SerializeField] private float _gap;
//        [SerializeField] private bool _childWidthExpand;
//        [SerializeField] private bool _childHeightExpand;

//        private RectTransform _rt;

//        private void Start() 
//        {
//            _rt = GetComponent<RectTransform>();
//        }

//        private void OnValidate()
//        {
//            if (Application.isPlaying) 
//                return;

//            Rebuild();
//        }

//        private void Rebuild()
//        {
//            _rt = GetComponent<RectTransform>();

//            //var pos = _rt.anchoredPosition;
//            //var size = _rt.sizeDelta;

//            var childrenAll = GetComponentsInChildren<RectTransform>().Except(_rt);
//            var childrenFlexItems = childrenAll.Select(i => i.Get<FlexListItem>()).NotNull();
//            var childrenFlexItemsActive = childrenFlexItems.Where(i => !i.IgnoreLayout);
//            var childrenFlexItemsActiveRTs = childrenFlexItems.Select(i => i.Get<RectTransform>());

//            var children = childrenAll.Except(childrenFlexItemsActiveRTs).ToArray();

//            var width = _rt.rect.width;
//            var height = _rt.rect.height;

//            if (_childWidthExpand)
//            {
//                var widthPerChild = width / children.Length;
//                var gap = _direction == FlexDirection.Horizontal ? _gap : 0;
//                children.ForEach((c, i) =>
//                {
//                    if (_direction != FlexDirection.Horizontal)
//                        c.SetWidth(width);
//                    else
//                    {
//                        c.StretchToMiddle(); // left
//                        c.SetWidth(widthPerChild - gap * (children.Length - 1));
//                        c.SetX(-i * (widthPerChild + gap));
//                        c.SetY(0);
//                    }
//                });
//            }

//            if (_childHeightExpand)
//            {
//                var gap = _direction == FlexDirection.Vertical ? _gap : 0;
//                var gapSum = gap * (children.Length - 1);

//                var heightPerChild = (height - gapSum) / children.Length;
//                children.ForEach((c, i) =>
//                {
//                    if (_direction != FlexDirection.Vertical)
//                        c.SetHeight(height);
//                    else
//                    {
//                        c.StretchToTop();
//                        c.SetHeight(heightPerChild);
//                        c.SetX(0);
//                        c.SetY(-i * (heightPerChild + gap));
//                    }
//                });

//                Debug.Log($"" + $@"
//                    Count: {children.Length},
//                    Width: {width},
//                    Height: {height},
//                    Child Height: {heightPerChild - gap * (children.Length - 1)}");
//            }

//            //_rt.anchoredPosition = pos;
//            //_rt.SetSize(size);

//            if (_direction == FlexDirection.Vertical && _childHeightExpand ||
//                _direction == FlexDirection.Horizontal && _childWidthExpand)
//                return;

//            //HorizontalOrVerticalLayoutGroup
//            //childrenFlexItemsActive
//        }

//#if UNITY_EDITOR
        
//        private int m_Capacity = 10;
//        private Vector2[] m_Sizes = new Vector2[10];

//        protected virtual void Update()
//        {
//            if (Application.isPlaying)
//                return;

//            int count = transform.childCount;

//            if (count > m_Capacity || count < m_Capacity)
//            {
//                if (count > m_Capacity * 2)
//                    m_Capacity = count;
//                else
//                    m_Capacity *= 2;

//                m_Sizes = new Vector2[m_Capacity];
//            }

//            // If children size change in editor, update layout (case 945680 - Child GameObjects in a Horizontal/Vertical Layout Group don't display their correct position in the Editor)
//            bool dirty = false;
//            for (int i = 0; i < count; i++)
//            {
//                var t = transform.GetChild(i) as RectTransform;
//                if (t != null && t.sizeDelta != m_Sizes[i])
//                {
//                    dirty = true;
//                    m_Sizes[i] = t.sizeDelta;
//                }
//            }

//            if (dirty)
//                Rebuild();
//        }
//#endif
//    }
//}

//public enum FlexDirection
//{
//    Vertical,
//    Horizontal
//}

//public enum FlexJustify
//{
//    Left,
//    Center,
//    Right,

//    SpaceBetween,
//    SpaceEvenly
//}

//public enum FlexAlign
//{
//    Left,
//    Center,
//    Right
//}