using UnityEngine;

public class DamageManager : MonoBehaviour
{
    public static DamageManager Instance; // Singleton para acceso global
    public bool doubleDamage = false; // Estado del daño x2

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantener entre escenas si es necesario
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void ActivateDoubleDamage()
    {
        doubleDamage = true;
        Debug.Log("¡Daño x2 activado permanentemente!");
    }
}
