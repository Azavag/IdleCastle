using UnityEngine;

[CreateAssetMenu(fileName = "New Cannon", menuName = "Cannon")]
public class CannonScriptableObject : ScriptableObject
{
    public string cannonName;
    public float price;

    public GameObject cannonModel;
   
}
