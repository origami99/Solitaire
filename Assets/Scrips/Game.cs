using UnityEngine;

public class Game : MonoBehaviour
{
    private void Update()
    {
        Cursor.lockState = CursorLockMode.Confined; // keep confined in the game window
    }
}
