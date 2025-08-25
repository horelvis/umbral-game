using UnityEngine;

namespace Umbral
{
    /// <summary>
    /// ScriptableObject que define un "Nudo de Culpa".
    /// Permite a los diseñadores crear diferentes tipos de culpas como assets en el proyecto,
    /// vinculando un ID de culpa a efectos de juego específicos como atraer enemigos.
    /// </summary>
    [CreateAssetMenu(fileName = "NewGuiltKnot", menuName = "Umbral/Guilt Knot")]
    public class GuiltKnot : ScriptableObject
    {
        [Header("Identificación")]
        [Tooltip("Identificador único de la culpa. Debe coincidir con el ID usado en PlayerState (ej: 'Culpa_Abandono').")]
        public string guiltId;

        [Tooltip("Nombre de la culpa que podría mostrarse en la UI o en logs.")]
        public string displayName;

        [Tooltip("Descripción de la culpa para uso interno o del diseñador.")]
        [TextArea(3, 5)]
        public string description;

        [Header("Efectos en el Juego")]
        [Tooltip("Prefab del enemigo que es atraído por esta culpa. Arrastrar el prefab del enemigo aquí.")]
        public GameObject attractedEnemyPrefab;

        // En el futuro, se podrían añadir más efectos aquí:
        // public AudioClip hallucinationSound;
        // public Material skyboxOverrideMaterial;
    }
}
