using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class GunBox : MonoBehaviour
{
    public List<GameObject> _weaponsList;
    public List<int> _weaponRates;
    public Sprite _openBoxSprite;

    private BoxCollider2D _boxcollider;
    private List<float> _weaponPercentPoints;

	private void Start ()
    {
        _boxcollider = GetComponent<BoxCollider2D>();
        _weaponPercentPoints = new List<float>(_weaponRates.Count);

        int totalWeaponPoints = _weaponRates.Aggregate(0, (acc, newValue) => acc + newValue);
        float aggregatedPoints = 0f;

        for (int i = 0; i < _weaponRates.Count; i++)
        {
            _weaponPercentPoints.Add(aggregatedPoints / totalWeaponPoints);
            aggregatedPoints += _weaponRates[i];
        }
    }

    private void OnTriggerEnter2D()
    {
        float percent = Random.value;
        int weaponSelected = -1;

        for (int i = _weaponPercentPoints.Count - 1; i >= 0; i--)
        {
            if (percent >= _weaponPercentPoints[i])
            {
                weaponSelected = i;
                break;
            }
        }

        Vector3 randomOffset = new Vector3(Random.Range(-.2f, .2f), Random.Range(-.2f, .2f), 0);
        GameObject weapon = Instantiate(_weaponsList[weaponSelected], transform.position + randomOffset, Quaternion.identity);
        weapon.transform.eulerAngles = new Vector3(0f, 0f, Random.Range(0, 359));
        _boxcollider.enabled = false;
        GetComponent<SpriteRenderer>().sprite = _openBoxSprite;
    }
}
