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
    protected void Awake()
    {
        // Validar que lifeText tenga un valor asignado
        if (lifeText == null)
        {
            Debug.LogError($"The TextMeshProUGUI reference in {gameObject.name} is not assigned. Please assign it in the inspector.", this);
            enabled = false; // Desactiva el script si no está configurado correctamente
        }
    }
    protected void Start()
    {
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
            lifeText.text = textname + m_actualLife; 
        }
    }
    public bool IsLifeLessThan(int value)
    {
        return m_actualLife < value;
    }
}
