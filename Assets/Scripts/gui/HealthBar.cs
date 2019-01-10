using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Text _healthBarText;
    public RectTransform _healthBar;

    private float _healthBarWidth;
    private float _yStart, _zStart, _xStart;

	private void Start ()
    {
        GameObject player = GameObject.FindWithTag("Player");
        _healthBarWidth = _healthBar.rect.width * - 1f;
        _xStart = _healthBar.transform.position.x;
        _yStart = _healthBar.transform.position.y;
        _zStart = _healthBar.transform.position.z;

        // add health change event handler and update to starting health
        Player playerScript = player.GetComponent<Player>();
        playerScript.AddHealthChangeSubscriber(UpdateHealth);
        UpdateHealth(playerScript._playerStats._health, playerScript._playerStats._maxHealth);
    }

    public void UpdateHealth(int newHealthValue, int maxHealthValue)
    {
        _healthBarText.text = newHealthValue.ToString() + " / " + maxHealthValue.ToString();

        float widthToMove = (1f - ((float)newHealthValue / maxHealthValue)) * _healthBarWidth;
        _healthBar.transform.position = new Vector3(_xStart + widthToMove, _yStart, _zStart);
    }
}
