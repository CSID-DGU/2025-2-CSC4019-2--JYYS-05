using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuWindow : Window
{
    [Header("Window Info")]
    [SerializeField] private TextMeshProUGUI menuName;
    [SerializeField] private TextMeshProUGUI priceTMP;
    [SerializeField] private TextMeshProUGUI menuDescription;
    [SerializeField] private Image image;

    /// <summary>
    /// 윈도우 TMP 업데이트
    /// </summary>
    /// <param name="_title"></param>
    /// <param name="_description"></param>
    public void UpdateInfoToMenuWindow(string _menuName, int _price, string _description, Sprite _sprite)
    {
        menuName.text = _menuName;
        priceTMP.text = _price.ToString();
        menuDescription.text = _description;
        if (_sprite != null) image.sprite = _sprite;
    }
}