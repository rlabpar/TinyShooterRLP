using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    // MOVIMIENTO
    private Rigidbody rb;
    private Vector2 move;
    private float directionX;
    private float directionZ;
    public float speed = 5f;

    // SALTO
    private bool jump;
    private bool isGrounded;
    public float thrustForce = 16000f;

    // DISPARAR Y MOUSE
    private bool _isAttack;
    private Vector2 mousePos;
    private RaycastHit _hitMouse;
    private Vector3 _lookAtPos;
    [SerializeField] private LayerMask _clickableLayers; // Capa del suelo para el Raycast
    [SerializeField] private GameObject _rocketPrefab;
    [SerializeField] private Transform _fireStartPos; // Un Empty hijo de Player desde donde sale el disparo
 
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (jump)
        {
            Jump();
        }

        Move();

        if(_isAttack)
        {
            _isAttack = false;
            Fire(); // True o false si el boton esta pausado o no
        }
    }

    private void Move()
    {
        directionX = move.x;
        directionZ = move.y;

        transform.Translate(directionX * speed * Time.deltaTime,
                            0f,
                            directionZ * speed * Time.deltaTime);
    }

    private void Jump()
    {
        jump = false;

        if (!isGrounded) return;

        Debug.Log("Jumping");
        rb.AddRelativeForce(Vector3.up * thrustForce * Time.fixedDeltaTime);
    }

    private void Fire()
    {
        if (!isGrounded) return; // Mientras salto no puedo disparar
        if (_rocketPrefab == null || _fireStartPos == null) return;
        RocketMover newProjectile = Instantiate(_rocketPrefab, _fireStartPos.position, this.transform.rotation).GetComponent<RocketMover>();
        if (newProjectile != null)
        {
            newProjectile.SetOwner(transform);
            newProjectile.transform.LookAt(_lookAtPos);
        }

    }

    public void OnMove(InputValue value)
    {
        move = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        jump = value.isPressed;
    }

     public void OnAttack(InputValue value)
    {
        _isAttack = value.isPressed; // True o false si el boton esta pulsado o no
        isGrounded = true;
    }

    public void OnMouseMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        mousePos = input; // Guardamos la posicion del mouse en coordenadas de pantalla en un vector 2D
        UpdateMousePosInWorld(); // Hay que convertir las coordenadas de pantalla (en 2D, x e y), a coordenadas 3D (x, y, z) del mundo
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.aquamarine;
        if (_fireStartPos)
        {
            Gizmos.DrawLine(_fireStartPos.position, _lookAtPos);
        }
        Gizmos.DrawWireSphere(_lookAtPos, 0.10f);
    }

    private void UpdateMousePosInWorld()
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        Physics.Raycast(ray, out _hitMouse, 1000, _clickableLayers); // _clickableLayers es la mascara de la layer que queremos considerar
        _lookAtPos = new Vector3(_hitMouse.point.x,
                                 this.transform.position.y,
                                 _hitMouse.point.z); // Posicion 3D que queremos guardar
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Ground Trigger");
        isGrounded = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isGrounded = false;
    }
}