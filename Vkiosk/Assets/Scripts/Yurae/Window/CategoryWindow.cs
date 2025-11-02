using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CategoryWindow : Window
{
    [Header("Window Info")]
    [SerializeField] private TextMeshProUGUI categoryTitle;
    [SerializeField] private Image image;

    /// <summary>
    /// 윈도우 TMP 업데이트
    /// </summary>
    /// <param name="_title"></param>
    public void UpdateInfoToCategoryWindow(string _title, Sprite _sprite)
    {
        categoryTitle.text = _title;
        if(_sprite != null) image.sprite = _sprite;
    }
}
