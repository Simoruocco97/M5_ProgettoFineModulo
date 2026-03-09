using UnityEngine;
using UnityEngine.UI;

public class UI_Lifebar : MonoBehaviour
{
    [Header("Lifebar")]
    [SerializeField] private Image lifebar;
    public void UpdateLifebar(int currentHp, int maxHp)
    {
        lifebar.fillAmount = (float)currentHp / maxHp;
    }
}