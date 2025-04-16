using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class YOTOScrollView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform content;        // 内容容器
    [SerializeField] private RectTransform viewport;       // 视口
    [SerializeField] private float itemHeight = 100f;      // 每个项的高度
    [SerializeField] private int spacing = 5;              // 项之间的间距
    
    private List<YOTOScrollViewItem> itemPool;            // 对象池
    private List<YOTOScrollViewDataBase> dataList;                        // 数据列表
    private float contentHeight;                          // 内容总高度
    private int poolSize;                                 // 对象池大小
    private int startIndex;                               // 当前显示的起始索引
    private int endIndex;                                 // 当前显示的结束索引
    private HashSet<int> visibleIndices;                 // 当前可见的索引集合

    private Vector2 lastDragPosition;
    private bool isDragging;
    private Vector2 lastContentPosition;

    private void Awake()
    {
        itemPool = new List<YOTOScrollViewItem>();
        dataList = new List<YOTOScrollViewDataBase>();
        visibleIndices = new HashSet<int>();

        if (!TryGetComponent<Image>(out var image))
        {
            image = gameObject.AddComponent<Image>();
            image.color = new Color(0, 0, 0, 0);
        }
        image.raycastTarget = true;
        
        if (content == null || viewport == null)
        {
            Debug.LogError("Content or Viewport is not assigned!");
            return;
        }

    
        // 设置Content的锚点和轴心点
        content.anchorMin = new Vector2(0, 1);
        content.anchorMax = new Vector2(1, 1);
        content.pivot = new Vector2(0.5f, 1);
        content.anchoredPosition = Vector2.zero;

        // 设置Viewport的锚点
        viewport.anchorMin = Vector2.zero;
        viewport.anchorMax = Vector2.one;
    }

    private void Start()
    {
        // 强制更新布局
        Canvas.ForceUpdateCanvases();
    }

    public void Initialize(GameObject itemPrefab, int poolSize)
    {
        this.poolSize = poolSize;
        
        foreach (var item in itemPool)
        {
            if (item != null)
            {
                Destroy(item.gameObject);
            }
        }
        itemPool.Clear();

        for (int i = 0; i < poolSize; i++)
        {
            var itemGo = Instantiate(itemPrefab, content);
            var item = itemGo.GetComponent<YOTOScrollViewItem>();
            if (item == null)
            {
                Debug.LogError("Item prefab must have YOTOScrollViewItem component!");
                return;
            }
            item.gameObject.SetActive(false);
            itemPool.Add(item);
        }
    }

    public void SetData<T>(List<T> data) where T : YOTOScrollViewDataBase
    {
        if (viewport == null || content == null)
        {
            Debug.LogError("Viewport or Content is null!");
            return;
        }

        Debug.Log($"SetData: 数据数量={data?.Count}, 对象池大小={itemPool?.Count}");

        // 检查对象池是否初始化
        if (itemPool == null || itemPool.Count == 0)
        {
            Debug.LogError("对象池未初始化，请先调用Initialize方法！");
            return;
        }

        // 先隐藏所有项
        foreach (var item in itemPool)
        {
            if (item != null && item.gameObject.activeSelf)
            {
                item.OnHidItem(item.CurrentData);
                item.gameObject.SetActive(false);
            }
        }
        //
        visibleIndices.Clear();
        
        if (data == null || data.Count == 0)
        {
            Debug.Log("SetData: 数据为空，清理列表");
            dataList.Clear();
            contentHeight = 0;
            content.sizeDelta = new Vector2(content.sizeDelta.x, 0);
            return;
        }

        dataList = data.ConvertAll(item => (YOTOScrollViewDataBase)item);
        
        // 计算总高度：所有项的高度 + (项数-1)个间距
         contentHeight = (data.Count * itemHeight) + ((data.Count - 1) * spacing);
         content.sizeDelta = new Vector2(content.sizeDelta.x, contentHeight);
        // content.anchoredPosition = Vector2.zero;
        // lastContentPosition = Vector2.zero;
        ClampContentPosition();
        RefreshItems();
      
    }

    private YOTOScrollViewItem GetFreeItem()
    {
        foreach (var item in itemPool)
        {
            if (!item.gameObject.activeSelf)
            {
                return item;
            }
        }
        Debug.LogWarning($"对象池大小不足！当前池大小：{itemPool.Count}");
        return null;
    }

    private void RefreshItems()
    {
        if (dataList == null || dataList.Count == 0) return;

        float scrollPosition = content.anchoredPosition.y;
        float viewportHeight = viewport.rect.height;
        float itemTotalHeight = itemHeight + spacing;

        // 计算当前应该显示的索引范围
        startIndex = Mathf.Max(0, Mathf.FloorToInt(scrollPosition / itemTotalHeight));
        int visibleCount = Mathf.CeilToInt(viewportHeight / itemTotalHeight) + 2;
        endIndex = Mathf.Min(dataList.Count - 1, startIndex + visibleCount);
        // Debug.Log("索引范围"+startIndex+":"+endIndex);
        // 处理当前显示的项
        Dictionary<int, YOTOScrollViewItem> currentItems = new Dictionary<int, YOTOScrollViewItem>();
        foreach (var item in itemPool)
        {
            if (item.gameObject.activeSelf)
            {
                int index = item.DataIndex;
                if (index >= startIndex && index <= endIndex)
                {
                    currentItems[index] = item;
                    continue;
                }
                item.OnHidItem(item.CurrentData);
                item.gameObject.SetActive(false);
                visibleIndices.Remove(index);
            }
        }

        // 更新或显示需要显示的项
        for (int i = startIndex; i <= endIndex; i++)
        {
            float itemYPos = -(i * (itemHeight + spacing))-itemTotalHeight/2;

            if (currentItems.TryGetValue(i, out var existingItem))
            {
                existingItem.transform.localPosition = new Vector3(0, itemYPos, 0);
                // Debug.Log("设置位置"+existingItem.transform.localPosition);
                continue;
            }

            YOTOScrollViewItem newItem = GetFreeItem();
            if (newItem == null) continue;

            newItem.transform.localPosition = new Vector3(0, itemYPos, 0);
            // Debug.Log("设置位置2:"+newItem.transform.localPosition);
            newItem.DataIndex = i;
            newItem.gameObject.SetActive(true);
            newItem.OnRenderItem(dataList[i]);
            visibleIndices.Add(i);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        lastDragPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        Vector2 delta = eventData.position - lastDragPosition;
        content.anchoredPosition += new Vector2(0, delta.y);
        
        ClampContentPosition();
        RefreshItems();
        lastDragPosition = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
    }

    private void ClampContentPosition()
    {
        float y = content.anchoredPosition.y;
        float maxY = Mathf.Max(0, contentHeight - viewport.rect.height);
        y = Mathf.Clamp(y, 0, maxY);
        content.anchoredPosition = new Vector2(content.anchoredPosition.x, y);
    }

    private void Update()
    {
        if (content.anchoredPosition != lastContentPosition)
        {
            lastContentPosition = content.anchoredPosition;
            RefreshItems();
        }
    }
}
