using UnityEngine;
using UnityEngine.UI;

public class SelectButtonOnEnable : MonoBehaviour
{
    [SerializeField] Button buttonToSelect;
    void OnEnable() => buttonToSelect.Select();
    public void SetButtonToSelect(Button button) => buttonToSelect = button;
}