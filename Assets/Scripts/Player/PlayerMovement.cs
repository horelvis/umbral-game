using UnityEngine;

namespace Umbral
{
    /// <summary>
    /// Controla el movimiento en primera persona del jugador y la rotación de la cámara.
    /// Requiere un componente CharacterController en el mismo GameObject.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Configuración de Movimiento")]
        [Tooltip("Velocidad de desplazamiento del jugador.")]
        [SerializeField] private float moveSpeed = 5.0f;

        [Header("Configuración de la Cámara")]
        [Tooltip("Referencia a la cámara del jugador. Debe ser hija de este objeto.")]
        [SerializeField] private Camera playerCamera;
        [Tooltip("Sensibilidad del ratón para la rotación.")]
        [SerializeField] private float lookSpeed = 2.0f;
        [Tooltip("Límite de rotación vertical de la cámara en grados para no dar la vuelta.")]
        [SerializeField] private float lookXLimit = 80.0f;

        // Componentes y variables internas
        private CharacterController _characterController;
        private Vector3 _moveDirection = Vector3.zero;
        private float _rotationX = 0;
        private bool _canMove = true;

        void Awake()
        {
            _characterController = GetComponent<CharacterController>();

            if (playerCamera == null)
            {
                Debug.LogError("La cámara del jugador no está asignada en el PlayerMovement script.");
            }
        }

        void Start()
        {
            // Bloquear el cursor en el centro de la pantalla y hacerlo invisible al iniciar el juego.
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update()
        {
            if (_canMove)
            {
                HandleMovement();
                HandleRotation();
            }
        }

        private void HandleMovement()
        {
            // Mantiene la dirección actual en el plano XZ pero resetea la Y para el cálculo de gravedad
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);

            // Input del jugador
            float curSpeedX = moveSpeed * Input.GetAxis("Vertical");
            float curSpeedY = moveSpeed * Input.GetAxis("Horizontal");
            float movementDirectionY = _moveDirection.y; // Guardar la velocidad vertical actual

            _moveDirection = (forward * curSpeedX) + (right * curSpeedY);

            // Aplicar gravedad
            if (!_characterController.isGrounded)
            {
                _moveDirection.y = movementDirectionY - (9.81f * Time.deltaTime);
            }
            else
            {
                // Si está en el suelo, la velocidad vertical debería ser un pequeño valor negativo
                // para asegurar que se mantenga pegado al suelo.
                _moveDirection.y = -0.5f;
            }

            // Mover el personaje
            _characterController.Move(_moveDirection * Time.deltaTime);
        }

        private void HandleRotation()
        {
            // Rotación vertical (cámara)
            _rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            _rotationX = Mathf.Clamp(_rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);

            // Rotación horizontal (jugador)
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        /// <summary>
        /// Permite a otros scripts habilitar o deshabilitar el movimiento y la rotación del jugador.
        /// </summary>
        /// <param name="canMoveStatus">True para permitir el movimiento, false para bloquearlo.</param>
        public void SetCanMove(bool canMoveStatus)
        {
            _canMove = canMoveStatus;
            Cursor.lockState = _canMove ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !_canMove;
        }
    }
}
