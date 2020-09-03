using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    [SerializeField]
    private int powerUpId;
    [SerializeField]
    private AudioClip powerupClip;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y <= -6.5f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(powerupClip, transform.position);

            Destroy(this.gameObject);
            Player player = other.GetComponent<Player>();

            if (player)
            {
                switch (powerUpId)
                {
                    case (int)PowerUpEnum.TripleShot:
                        player.SetTripleShot();
                        break;
                    case (int)PowerUpEnum.Speed:
                        player.SetSpeed(2);
                        break;
                    case (int)PowerUpEnum.Shield:
                        player.SetShield();
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private enum PowerUpEnum
    {
        TripleShot = 0,
        Speed = 1,
        Shield = 2
    }
}
