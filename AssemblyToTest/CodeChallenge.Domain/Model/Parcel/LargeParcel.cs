namespace CodeChallenge.Domain.Model.Parcel
{
    public class LargeParcel: BaseParcel
	{
		public LargeParcel(int weight, int height, int width, int depth, int volume)
			: base(weight, height, width, depth, volume)
		{
		}

	    public override decimal CalculateCost()
	    {
			return (decimal)0.03 * Volume;
		}
	}
}
