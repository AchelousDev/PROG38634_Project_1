using UnityEngine;

[RequireComponent(typeof(Animator))]
public class WizardSpellController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private KeyCode castKey = KeyCode.F;

    [Header("Animator")]
    [SerializeField] private string castTrigger = "CastSpell";

    [Header("AOE")]
    [SerializeField] private float spellRadius = 6f;
    [SerializeField] private LayerMask monsterLayer;

    [Header("Timing")]
    [SerializeField] private float spellDelay = 1.6f;
    
    [SerializeField] private GameObject aoeFireEffectPrefab;
    [SerializeField] private Transform spellAreaPoint;

    private Animator animator;
    private bool isCasting = false;
    
    public void CastSpellFromTouch()
    {
        if (!isCasting)
        {
            CastSpell();
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(castKey) && !isCasting)
        {
            CastSpell();
        }
    }

    private void CastSpell()
    {
        animator.SetTrigger(castTrigger);
        StartCoroutine(SpellRoutine());
    }

    private System.Collections.IEnumerator SpellRoutine()
    {
        isCasting = true;

        // 等动画播放到真正释放魔法的那一刻
        yield return new WaitForSeconds(spellDelay);

        SpawnAOEEffect();

        ApplyAOE();

        // 防止疯狂连按
        yield return new WaitForSeconds(0.5f);

        isCasting = false;
    }

    private void ApplyAOE()
    {
        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            spellRadius,
            monsterLayer
        );

        Debug.Log($"AOE hit {hits.Length} monster(s).");

        foreach (Collider hit in hits)
        {
            MonsterDeath monster = hit.GetComponentInParent<MonsterDeath>();

            if (monster != null)
            {
                monster.Die();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, spellRadius);
    }
    
    private void SpawnAOEEffect()
    {
        if (aoeFireEffectPrefab == null ||
            spellAreaPoint == null)
        {
            return;
        }

        GameObject effect = Instantiate(
            aoeFireEffectPrefab,
            spellAreaPoint.position,
            Quaternion.identity
        );

        Destroy(effect, 3f);
    }
}