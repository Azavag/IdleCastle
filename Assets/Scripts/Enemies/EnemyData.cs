using UnityEngine;

public class EnemyData : MonoBehaviour
{
    EnemyScriptableObject enemyType;
    GameObject prefabModel;

    BoxCollider boxCollider;

    public Vector3 colliderScale;
    public int moveSpeed { get; set; }
    public float maxHealth { get; set; }
    public float currentHealth { get; set; }
    public float damage { get; set; }
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
    public void SetStats(float stats)
    {
        maxHealth = stats;
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
