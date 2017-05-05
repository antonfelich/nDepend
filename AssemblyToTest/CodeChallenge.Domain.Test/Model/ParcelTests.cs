using Xunit;
using CodeChallenge.Domain.Model.Parcel;

namespace CodeChallenge.Domain.Tests.Model
{
    public class ParcelTests
    {
	    [Fact]
	    public void CanCalculateHeavyParcel()
	    {
		    HeavyParcel target = new HeavyParcel(22, 5, 5, 5, (5 * 5 * 5));
			Assert.Equal(target.CalculateCost(), (decimal)330.00);
	    }

		[Fact]
		public void CanCalculateSmallParcel()
		{
			SmallParcel target = new SmallParcel(2, 3, 10, 12, (3 * 10 * 12));
			Assert.Equal(target.CalculateCost(), (decimal)18.00);
		}

		[Fact]
		public void CanCalculateMediumParcel()
		{
			MediumParcel target = new MediumParcel(10, 20, 5, 20, (20 * 5 * 20));
			Assert.Equal(target.CalculateCost(), (decimal)80.00);
		}

		[Fact]
		public void CanCalculateLargeParcel()
		{
			LargeParcel target = new LargeParcel(10, 20, 5, 30, (20 * 5 * 30));
			Assert.Equal(target.CalculateCost(), (decimal)90.00);
		}
	}
}
