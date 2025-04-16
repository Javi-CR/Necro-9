using UnityEngine;

public class PortalAlwaysOn : MonoBehaviour
{
    [SerializeField] private Portal_Controller portalSimpleScripts;

    private void Start()
    {
        // Asegurarse de que el portal esté siempre encendido
        if (portalSimpleScripts != null)
        {
            portalSimpleScripts.TogglePortal(true);
        }
    }
}
