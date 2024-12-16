using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Life : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float m_initialLife =5;
    [SerializeField]
    private float m_actualLife = 5;
    [SerializeField]
    private TextMeshProUGUI lifeText;
    [SerializeField]
    private string textname;
    protected void Start()
    {
        Debug.Log("gsfg");
        m_actualLife = m_initialLife;
        UpdateLifeText();
    }
    public void DecreaseLife(float num)
    {
        m_actualLife -= num;
        UpdateLifeText();
    }
    public void IncreaseLife(float num)
    {
        m_actualLife += num;
        UpdateLifeText();
    }
    public void SetLife(float num)
    {
        m_actualLife = num;
        UpdateLifeText();
    }
    public void SetInitialLife()
    {
        m_actualLife = m_initialLife;
        UpdateLifeText();
    }
    private void UpdateLifeText()
    {
        if (lifeText != null)
        {
            lifeText.text = textname + m_actualLife; // Muestra la vida con un decimal
        }
    }
}
