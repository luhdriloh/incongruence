using UnityEngine;

public class Player : MonoBehaviour
{
    public float _speed;

    private Rigidbody2D _rigidbody;

    private void Start ()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        transform.position += new Vector3(2f, 2f, 0f);
	}
	
	private void Update () 
    {
        Move();
	}

    private void Move()
    {
        // get axis for sniper dude and move him about
        Vector2 movementInDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * Time.deltaTime * _speed;
        Vector2 newPosition = (Vector2)transform.position + movementInDirection;
        _rigidbody.MovePosition(newPosition);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("transporter"))
        {
            // get the transporter coordinates
            Transporter transporter = collision.gameObject.GetComponent<Transporter>();
            transform.position = transporter.GetTransporterCoordinates();
        }
    }
}
