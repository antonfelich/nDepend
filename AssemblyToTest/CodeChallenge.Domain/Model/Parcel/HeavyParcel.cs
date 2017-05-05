namespace CodeChallenge.Domain.Model.Parcel
{
    public class HeavyParcel: BaseParcel
    {
	    public HeavyParcel(int weight, int height, int width, int depth, int volume)
			: base(weight, height, width, depth, volume)
		{
	    }

	    public override decimal CalculateCost()
	    {
			return 15 * Weight;
		}
    }
}
