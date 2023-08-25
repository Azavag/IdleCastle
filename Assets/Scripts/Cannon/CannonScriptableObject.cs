using UnityEngine;

[CreateAssetMenu(fileName = "New Cannon", menuName = "Cannon")]
public class CannonScriptableObject : ScriptableObject
{
    public string cannonName;
    public float bulletSpeed;
    public float timeBetweenShots;
    public float bulletDamage;

    public GameObject cannonModel;
   
}
