public class LaserAmmoTechDebree : TechDebree
{
    public int ammoValue = 10;

    public override void Collect()
    {
        GameManager.instance.playerReference.RefreshLaserBeamAmmo(ammoValue);

        Destroy(gameObject);
    }
}
