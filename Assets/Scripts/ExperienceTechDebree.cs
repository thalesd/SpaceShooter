public class ExperienceTechDebree : TechDebree
{
    private int evolutionValue = 10;

    //default behavior
    public override void Collect()
    {
        GameManager.instance.playerReference.AddEvolutionValue(evolutionValue);

        Destroy(gameObject);
    }
}
