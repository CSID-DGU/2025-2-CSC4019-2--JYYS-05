// Unity
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class MenuGrid : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameTMP;
    [SerializeField] private TextMeshProUGUI priceTMP;
    [SerializeField] private TextMeshProUGUI countTMP;

    [SerializeField] private Button addButton;
    [SerializeField] private Button removeButton;
    
    public void Init(string _name)
    {
        nameTMP.text = _name;
    }

    public void SetCount(string _count, string _price)
    {
        countTMP.text = _count;
        priceTMP.text = _price;
    }

    public void SetAddButton(UnityAction _action)
    {
        addButton.onClick.AddListener(_action);
    }

    public void SetRemoveButton(UnityAction _action)
    {
        removeButton.onClick.AddListener(_action);
    }

    public void DestroyGrid()
    {
        Destroy(this.gameObject);
    }
}
