using UnityEngine;
using System.Collections;

namespace Umbral
{
    /// <summary>
    /// Gestiona las mecánicas de luz del jugador: una luz sostenida y un pulso de claridad.
    /// Consume el recurso "Fe" del PlayerState y controla los componentes de Luz asociados.
    /// </summary>
    [RequireComponent(typeof(PlayerState))]
    public class InnerLight : MonoBehaviour
    {
        [Header("Referencias de Componentes")]
        [Tooltip("Luz suave y constante que consume Fe a lo largo del tiempo. Arrastrar el objeto de Luz aquí.")]
        [SerializeField] private Light _sustainedLight;
        [Tooltip("Destello de luz intenso y de corta duración. Arrastrar el objeto de Luz aquí.")]
        [SerializeField] private Light _pulseLight;

        [Header("Configuración de Luz Sostenida")]
        [Tooltip("Coste de Fe por segundo al mantener la luz sostenida activa.")]
        [SerializeField] private float _sustainedFaithCostPerSecond = 1.5f;
        private bool _isSustainedLightActive = false;

        [Header("Configuración de Pulso de Claridad")]
        [Tooltip("Coste de Fe para activar un único pulso de claridad.")]
        [SerializeField] private float _pulseFaithCost = 20f;
        [Tooltip("Duración del destello del pulso en segundos.")]
        [SerializeField] private float _pulseDuration = 1.0f;
        [Tooltip("Tiempo de espera en segundos antes de poder usar otro pulso.")]
        [SerializeField] private float _pulseCooldown = 3.0f;

        // Variables privadas
        private PlayerState _playerState;
        private float _pulseCooldownTimer = 0f;

        void Awake()
        {
            _playerState = GetComponent<PlayerState>();

            // Validaciones para asegurar que todo está configurado correctamente en el editor.
            if (_sustainedLight == null) Debug.LogError("Referencia a _sustainedLight no asignada en el Inspector.");
            if (_pulseLight == null) Debug.LogError("Referencia a _pulseLight no asignada en el Inspector.");

            // Asegurarse de que las luces están apagadas al inicio.
            if(_sustainedLight) _sustainedLight.enabled = false;
            if(_pulseLight) _pulseLight.enabled = false;
        }

        void Update()
        {
            // Actualizar el temporizador de cooldown del pulso.
            if (_pulseCooldownTimer > 0)
            {
                _pulseCooldownTimer -= Time.deltaTime;
            }

            HandleInput();
            UpdateSustainedLightState();
        }

        private void HandleInput()
        {
            // Input para alternar la luz sostenida (ej: tecla F).
            if (Input.GetKeyDown(KeyCode.F))
            {
                ToggleSustainedLight();
            }

            // Input para el pulso de claridad (ej: botón izquierdo del ratón).
            if (Input.GetMouseButtonDown(0))
            {
                TryActivatePulse();
            }
        }

        private void ToggleSustainedLight()
        {
            // No se puede encender si no hay Fe.
            if (!_isSustainedLightActive && _playerState.Faith <= 0)
            {
                // Opcional: Añadir feedback al jugador (sonido, UI).
                Debug.Log("No hay suficiente Fe para encender la luz.");
                return;
            }

            _isSustainedLightActive = !_isSustainedLightActive;
            _sustainedLight.enabled = _isSustainedLightActive;
        }

        private void UpdateSustainedLightState()
        {
            if (_isSustainedLightActive)
            {
                // Consumir fe y apagar la luz si se agota.
                bool hasFaith = _playerState.UseFaith(_sustainedFaithCostPerSecond * Time.deltaTime);
                if (!hasFaith)
                {
                    _isSustainedLightActive = false;
                    _sustainedLight.enabled = false;
                    // Opcional: Feedback de que la luz se apagó por falta de Fe.
                    Debug.Log("Fe agotada. La luz sostenida se ha apagado.");
                }
            }
        }

        private void TryActivatePulse()
        {
            if (_pulseCooldownTimer > 0)
            {
                // Opcional: Feedback de que la habilidad está en cooldown.
                Debug.Log("El Pulso de Claridad está en cooldown.");
                return;
            }

            if (_playerState.UseFaith(_pulseFaithCost))
            {
                StartCoroutine(PulseCoroutine());
            }
            else
            {
                // Opcional: Feedback de que no hay suficiente Fe.
                Debug.Log("No hay suficiente Fe para el Pulso de Claridad.");
            }
        }

        private IEnumerator PulseCoroutine()
        {
            _pulseLight.enabled = true;
            _pulseCooldownTimer = _pulseCooldown; // Iniciar cooldown.

            yield return new WaitForSeconds(_pulseDuration);

            _pulseLight.enabled = false;
        }
    }
}
