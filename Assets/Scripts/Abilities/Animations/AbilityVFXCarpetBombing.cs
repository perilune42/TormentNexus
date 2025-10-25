using System.Collections;
using UnityEngine;

public class AbilityVFXCarpetBombing : AbilityVFX
{
    [SerializeField] float duration = 10f;
    [SerializeField] int bombCount = 50;
    [SerializeField] float bombRadius = 0.5f;
    [SerializeField] float bombDelay = 0.05f;
    [SerializeField] float bombScale = 1f;

    [SerializeField] GameObject bombPrefab;

    public override void Play(Faction attacker)
    {
        if (attacker == FactionManager.instance.playerFaction)
        {
            AudioManager.instance.Play(AudioManager.instance.ExplosionHeavyThud);
        }
        base.Play(attacker);
        StartCoroutine(CarpetBomb());
        StartCoroutine(Destroy());
    }

    private IEnumerator CarpetBomb()
    {
        for (int i = 0; i < bombCount; i++)
        {
            GameObject bomb = Instantiate(bombPrefab, transform);
            bomb.transform.localScale *= bombScale;
            bomb.transform.Rotate(Vector3.forward, Random.Range(0, 360));
            bomb.transform.Translate(Vector2.right * Random.Range(0, bombRadius));
            yield return new WaitForSeconds(bombDelay);
        }
    }

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(duration);
        StopAllCoroutines();
        Destroy(gameObject);
    }
}