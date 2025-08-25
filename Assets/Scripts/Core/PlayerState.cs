using UnityEngine;
using System.Collections.Generic;

namespace Umbral
{
    /// <summary>
    /// Gestiona el estado central del jugador, incluyendo Miedo, Fe y Culpa.
    /// Este componente es el "cerebro" de la psique del personaje y otros sistemas leerán de él.
    /// </summary>
    public class PlayerState : MonoBehaviour
    {
        [Header("Estado Psicológico")]

        [Tooltip("Nivel de Miedo actual (0-100). Afecta la percepción del mundo.")]
        [Range(0f, 100f)]
        [SerializeField] private float _fear = 0f;
        public float Fear => _fear;

        [Tooltip("Recurso espiritual para usar la Luz Interior (0-100).")]
        [Range(0f, 100f)]
        [SerializeField] private float _faith = 100f;
        public float Faith => _faith;

        [Tooltip("Máximo nivel de Fe que se puede acumular.")]
        [SerializeField] private float _maxFaith = 100f;
        public float MaxFaith => _maxFaith;

        [Header("Carga Espiritual")]

        [Tooltip("Lista de culpas no resueltas que atraen amenazas y generan alucinaciones.")]
        [SerializeField] private List<string> _unresolvedGuilts = new List<string>();
        // Exponemos la lista como de solo lectura para que otros scripts no puedan modificarla directamente.
        public IReadOnlyList<string> UnresolvedGuilts => _unresolvedGuilts.AsReadOnly();

        #region API Pública para Modificar el Estado

        /// <summary>
        /// Aumenta el nivel de Miedo del jugador, asegurando que no exceda 100.
        /// </summary>
        /// <param name="amount">Cantidad de miedo a añadir.</param>
        public void AddFear(float amount)
        {
            _fear = Mathf.Clamp(_fear + amount, 0f, 100f);
            Debug.Log($"Miedo ha cambiado a: {_fear}");
        }

        /// <summary>
        /// Reduce el nivel de Miedo del jugador, asegurando que no baje de 0.
        /// </summary>
        /// <param name="amount">Cantidad de miedo a reducir.</param>
        public void ReduceFear(float amount)
        {
            _fear = Mathf.Clamp(_fear - amount, 0f, 100f);
            Debug.Log($"Miedo ha cambiado a: {_fear}");
        }

        /// <summary>
        /// Consume Fe si hay suficiente. Devuelve true si la operación fue exitosa.
        /// </summary>
        /// <param name="amount">Cantidad de Fe a usar.</param>
        /// <returns>True si la Fe fue consumida, false si no había suficiente.</returns>
        public bool UseFaith(float amount)
        {
            if (_faith >= amount)
            {
                _faith -= amount;
                Debug.Log($"Fe usada. Fe actual: {_faith}");
                return true;
            }
            Debug.LogWarning("No hay suficiente Fe para realizar la acción.");
            return false;
        }

        /// <summary>
        /// Restaura la Fe del jugador, sin exceder el máximo.
        /// </summary>
        /// <param name="amount">Cantidad de Fe a restaurar.</param>
        public void RestoreFaith(float amount)
        {
            _faith = Mathf.Clamp(_faith + amount, 0f, _maxFaith);
            Debug.Log($"Fe restaurada. Fe actual: {_faith}");
        }

        /// <summary>
        /// Añade un nuevo Nudo de Culpa al estado del jugador si no existe ya.
        /// </summary>
        /// <param name="guiltId">Identificador único de la culpa (ej: "Culpa_Traicion").</param>
        public void AddGuilt(string guiltId)
        {
            if (!_unresolvedGuilts.Contains(guiltId))
            {
                _unresolvedGuilts.Add(guiltId);
                Debug.Log($"Nueva culpa añadida: {guiltId}");
            }
        }

        /// <summary>
        /// Resuelve un Nudo de Culpa, eliminándolo del estado del jugador.
        /// </summary>
        /// <param name="guiltId">Identificador de la culpa a resolver.</param>
        public void ResolveGuilt(string guiltId)
        {
            if (_unresolvedGuilts.Remove(guiltId))
            {
                Debug.Log($"Culpa resuelta: {guiltId}");
            }
        }

        #endregion
    }
}
