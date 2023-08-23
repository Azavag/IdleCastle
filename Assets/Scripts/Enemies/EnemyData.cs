using UnityEngine;

public class EnemyData : MonoBehaviour
{
    EnemyScriptableObject enemyType;
    GameObject prefabModel;

    BoxCollider boxCollider;

    public Vector3 colliderScale;
    public int moveSpeed { get; set; }
    public int maxHealth { get; set; }
    public int currentHealth { get; set; }
    public int damage { get; set; }
    public float cost = 1;

    private void Start()
    {
        colliderScale = enemyType.prefab.transform.localScale;
        boxCollider = gameObject.GetComponent<BoxCollider>();

        boxCollider.center = new Vector3(enemyType.collider.center.x * colliderScale.x, 
            enemyType.collider.center.y * colliderScale.y, 
            enemyType.collider.center.z * colliderScale.z);
 
        boxCollider.size = new Vector3(enemyType.collider.size.x * colliderScale.x,
            enemyType.collider.size.y * colliderScale.y,
            enemyType.collider.size.z * colliderScale.z);

        enemyType.collider.enabled = false;        

        SetEnemyData();
    }
    public void ChooseEnemyType(EnemyScriptableObject type)
    {
        enemyType = type;
    }
    //Цена устаналвивается при спавне
   
    void SetEnemyData()
    {
        prefabModel = enemyType.prefab;
        Instantiate(prefabModel, new Vector3(transform.position.x, transform.position.y, transform.position.z),
            transform.rotation, transform);

        this.name = enemyType.enemyName;
    }
    public void SetMultiplier(float multipleir)
    {
        cost *= multipleir;
    }
    public void SetStats(int health)
    {
        maxHealth = health;
        currentHealth = maxHealth;
        damage = maxHealth;
    }
    public void SetAttackDmg(int dmg)
    {
        damage = dmg;
    }
    public void SetSpeed(int speed)
    {
        moveSpeed = speed;
    }
}
