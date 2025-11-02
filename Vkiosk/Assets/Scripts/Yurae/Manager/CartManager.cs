// System
using System.Collections.Generic;

// Unity
using UnityEngine;
using TMPro;

[System.Serializable]
public class MenuInfo
{
    public string id;
    public string name;
    public int price;
    public int totalPrice;
    public MenuGrid menuGrid;

    public MenuInfo(string _id, string _name, int _price, MenuGrid _menuGrid)
    {
        id = _id;
        name = _name;
        price = _price;
        menuGrid = _menuGrid;
    }
}

[DisallowMultipleComponent]
public class CartManager : MonoBehaviour
{
    [SerializeField] private GameObject menuGrid;
    [SerializeField] private RectTransform cartFrameTransform;
    [SerializeField] private RectTransform payFrameTransform;
    [SerializeField] private TextMeshProUGUI countTMP;

    [SerializeField] private TextMeshProUGUI totalPriceTMP;

    private int selectCount;
 
    private Dictionary<string ,(MenuInfo info, int count)> cartDict = new();

    public void Init()
    {
        cartDict.Clear();

        selectCount = 0;
        UpdateCountTMP();

        RectTransform[] frameChild = null;

        if (cartFrameTransform.childCount != 0) frameChild = cartFrameTransform.GetComponentsInChildren<RectTransform>();
        else frameChild = payFrameTransform.GetComponentsInChildren<RectTransform>();

        foreach (RectTransform rect in frameChild)
        {
            if (rect == cartFrameTransform || rect == payFrameTransform) continue;

            Destroy(rect.gameObject);
        }
    }

    /// <summary>
    /// 장바구니에 추가하기 위한 메소드
    /// </summary>
    /// <param name="_id"></param>
    /// <param name="_name"></param>
    /// <param name="_price"></param>
    public void AddCart(string _id, string _name, int _price, Sprite _sprite)
    {
        // 이미 있을 경우 count만 ++
        if (cartDict.ContainsKey(_id))
        {
            cartDict[_id] = (cartDict[_id].info, cartDict[_id].count + 1);

            cartDict[_id].info.totalPrice = cartDict[_id].count * cartDict[_id].info.price;
            cartDict[_id].info.menuGrid.SetCount(cartDict[_id].count.ToString(), cartDict[_id].info.totalPrice.ToString("N0"));

            UpdateTotalPriceTMP();
        }
        // 아닐 경우 새로 추가
        else
        {
            MenuGrid grid = Instantiate(menuGrid, cartFrameTransform).GetComponent<MenuGrid>();

            cartDict[_id] = (new MenuInfo(_id, _name, _price, grid), 1);

            // 처음 추가할 시 한번만 실행하는 초기화 함수들
            cartDict[_id].info.menuGrid.Init(_name);

            cartDict[_id].info.totalPrice = cartDict[_id].count * cartDict[_id].info.price;
            cartDict[_id].info.menuGrid.SetCount(cartDict[_id].count.ToString(), cartDict[_id].info.totalPrice.ToString("N0"));

            cartDict[_id].info.menuGrid.SetAddButton(()=>AddCart(_id, _name, _price, _sprite));
            cartDict[_id].info.menuGrid.SetRemoveButton(() => RemoveCart(_id));

            selectCount++;
            UpdateCountTMP();
            UpdateTotalPriceTMP();
        }
    }

    /// <summary>
    /// 장바구니에서 제거하기 위한 메소드
    /// </summary>
    /// <param name="_id"></param>
    public void RemoveCart(string _id)
    {
        // 이미 포함되어 있는 경우만 제거 가능
        if (cartDict.ContainsKey(_id))
        {
            var (info, count) = cartDict[_id];
            if (count <= 1) // 1일 경우 삭제
            {
                cartDict[_id].info.menuGrid.DestroyGrid();
                cartDict.Remove(_id);

                selectCount--;
                UpdateCountTMP();
                UpdateTotalPriceTMP();
            }
            else
            {
                cartDict[_id] = (info, count - 1);

                cartDict[_id].info.totalPrice = cartDict[_id].count * cartDict[_id].info.price;
                cartDict[_id].info.menuGrid.SetCount(cartDict[_id].count.ToString(), cartDict[_id].info.totalPrice.ToString("N0"));
                UpdateTotalPriceTMP();
            }
        }
    }

    private void UpdateCountTMP()
    {
        countTMP.text = "선택한 메뉴\n" + selectCount + "개";
    }

    private void UpdateTotalPriceTMP()
    {
        int totalPrice = 0;
        foreach (KeyValuePair<string, (MenuInfo, int)> kvp in cartDict)
        {
            totalPrice += cartDict[kvp.Key].info.totalPrice; 
        }

        totalPriceTMP.text = totalPrice.ToString("N0") + "원";
    }
}
