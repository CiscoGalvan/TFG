using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Life : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float m_initialLife = 5;

    private float m_currentLife;
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
        m_currentLife = m_initialLife;
        UpdateLifeText();
    }
    public void DecreaseLife(float num)
    {
        m_currentLife -= num;
		Debug.Log("Life = " + m_currentLife);
		UpdateLifeText();
    }
	public void InstantKill()
	{
		m_currentLife = 0;
		Debug.Log("Life = " + m_currentLife);
		UpdateLifeText();
	}
	public void IncreaseLife(float num)
    {
        m_currentLife += num;
        UpdateLifeText();
    }
    public void SetLife(float num)
    {
        m_currentLife = num;
        UpdateLifeText();
    }
    public void SetInitialLife()
    {
        m_currentLife = m_initialLife;
        UpdateLifeText();
    }
    private void UpdateLifeText()
    {
        if (lifeText != null)
        {
            lifeText.text = textname + m_currentLife; 
        }
    }
    public bool IsLifeLessThan(int value)
    {
        return m_currentLife < value;
    }
}
