using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [Header("Paneles de Menú (Para juego)")]
    public GameObject defeatPanel;
    public GameObject victoryPanel;

    void Awake()
    {
        // Configurar un Singleton simple para acceder desde otros scripts
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        // Asegurarnos de que el juego no empiece pausado al cargar la escena
        Time.timeScale = 1f;

        // Ocultar los paneles al empezar la partida si estamos en la escena de juego
        if (defeatPanel != null) defeatPanel.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);
    }

    // --- FUNCIONES PARA LOS BOTONES ---

    public void IniciarJuego()
    {
        // Carga la siguiente escena en la lista (asegúrate de que el juego sea la escena 1)
        SceneManager.LoadScene(1);
    }

    public void Reintentar()
    {
        // Recarga la escena actual en la que se encuentra el jugador
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }

    public void VolverAlMenu()
    {
        // Carga la escena del menú principal (escena 0)
        SceneManager.LoadScene(0);
    }

    // --- DISPARADORES DE ESTADO (GAME OVER / VICTORIA) ---

    public void TriggerDefeat()
    {
        if (defeatPanel != null)
        {
            defeatPanel.SetActive(true);
            Time.timeScale = 0f; // Pausa el juego (frenará el FixedUpdate de los enemigos y jugador)
        }
    }

    public void TriggerVictory()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
            Time.timeScale = 0f; // Pausa el juego
        }
    }
}