using Xunit;
using CodeChallenge.Domain;
using CodeChallenge.Domain.Exceptions.Parcel;
using System;
using CodeChallenge.Domain.Model.Parcel;

namespace CodeChallenge.Domain.Test
{
    public class ParcelFactoryTests
    {
		[Theory]
		[InlineData(10, 20, 5, 20, typeof(MediumParcel))]
		[InlineData(22, 5, 5, 5, typeof(HeavyParcel))]
		[InlineData(2, 3, 10, 12, typeof(SmallParcel))]
		[InlineData(5, 20, 55, 120, typeof(LargeParcel))]
		public void CanCreateParcel(int weight, int height, int width, int depth, Type t)
		{
			BaseParcel parcel = ParcelFactory.CreateParcel(weight, height, width, depth);
			Assert.Equal(parcel.GetType(), t);
		}

	    [Theory]
		[InlineData(110, 20, 55, 120)]
		public void CanRejectOversizedParcel(int weight, int height, int width, int depth)
		{
		    Exception ex =
			    Assert.Throws<RejectedParcelException>(() => ParcelFactory.CreateParcel(weight, height, width, depth));

			Assert.Equal(ex.GetType(), typeof(RejectedParcelException));
		}


		[Theory]
		[InlineData(20, 5, 20, 2000)]
		[InlineData(5, 5, 5, 125)]
		[InlineData(3, 10, 12, 360)]
		[InlineData(20, 55, 120, 132000)]
		public void CanCalculateVolume(int height, int width, int depth, int volume)
		{
			Assert.Equal(ParcelFactory.CalculateVolume(height, width, depth), volume);
		}
	}
}
