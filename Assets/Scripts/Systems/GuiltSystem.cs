using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Umbral
{
    /// <summary>
    /// Gestiona cómo la culpa no resuelta del jugador afecta al mundo del juego,
    /// principalmente atrayendo a tipos específicos de enemigos de forma periódica.
    /// </summary>
    public class GuiltSystem : MonoBehaviour
    {
        [Header("Referencias Clave")]
        [Tooltip("Referencia al estado del jugador para leer su lista de culpas.")]
        [SerializeField] private PlayerState _playerState;

        [Header("Configuración del Sistema")]
        [Tooltip("Arrastrar aquí todos los assets de GuiltKnot que existen en el proyecto.")]
        [SerializeField] private List<GuiltKnot> _allGuiltKnots;
        [Tooltip("Posibles puntos de aparición para los enemigos atraídos por la culpa.")]
        [SerializeField] private List<Transform> _spawnPoints;
        [Tooltip("Intervalo en segundos para comprobar si se debe generar un enemigo.")]
        [SerializeField] private float _spawnCheckInterval = 15f;

        private float _timer;
        private Dictionary<string, GuiltKnot> _guiltKnotDictionary;

        void Awake()
        {
            if (_playerState == null)
            {
                Debug.LogError("PlayerState no está asignado en GuiltSystem. El script se deshabilitará.");
                enabled = false;
                return;
            }

            // Convertir la lista de ScriptableObjects en un diccionario para búsquedas O(1), es mucho más eficiente.
            _guiltKnotDictionary = new Dictionary<string, GuiltKnot>();
            foreach (var knot in _allGuiltKnots)
            {
                if (knot != null && !_guiltKnotDictionary.ContainsKey(knot.guiltId))
                {
                    _guiltKnotDictionary.Add(knot.guiltId, knot);
                }
            }
        }

        void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= _spawnCheckInterval)
            {
                _timer = 0f;
                CheckAndProcessGuilt();
            }
        }

        /// <summary>
        /// Comprueba las culpas del jugador y, si las hay, intenta manifestar una.
        /// </summary>
        private void CheckAndProcessGuilt()
        {
            var unresolvedGuilts = _playerState.UnresolvedGuilts;
            if (unresolvedGuilts.Count == 0) return;

            // Seleccionar una de las culpas no resueltas del jugador al azar para que sea la "activa" en este ciclo.
            string randomGuiltId = unresolvedGuilts[Random.Range(0, unresolvedGuilts.Count)];

            if (_guiltKnotDictionary.TryGetValue(randomGuiltId, out GuiltKnot activeKnot))
            {
                // Si la culpa tiene un enemigo asociado, intentar generarlo.
                if (activeKnot.attractedEnemyPrefab != null)
                {
                    SpawnEnemyFromGuilt(activeKnot);
                }
            }
        }

        /// <summary>
        /// Simula la aparición de un enemigo asociado a una culpa.
        /// </summary>
        private void SpawnEnemyFromGuilt(GuiltKnot guiltSource)
        {
            if (_spawnPoints == null || _spawnPoints.Count == 0)
            {
                Debug.LogWarning("No hay puntos de spawn asignados en el GuiltSystem. No se puede generar enemigo.");
                return;
            }

            // Seleccionar un punto de aparición al azar de la lista.
            Transform spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Count)];

            // --- LÓGICA DE INSTANCIACIÓN ---
            // En un juego completo, la siguiente línea estaría descomentada.
            // Instantiate(guiltSource.attractedEnemyPrefab, spawnPoint.position, spawnPoint.rotation);

            // Simulación con Debug.Log para confirmar que la lógica funciona.
            Debug.Log($"GuiltSystem: La culpa '{guiltSource.displayName}' ha atraído a un enemigo '{guiltSource.attractedEnemyPrefab.name}' en la ubicación {spawnPoint.position}.");
        }
    }
}
