public class HealingTechDebree : TechDebree
{
    private int healingValue = 10;

    //default behavior
    public override void Collect()
    {
        GameManager.instance.playerReference.Heal(healingValue);

        Destroy(gameObject);
    }
}
