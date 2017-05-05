using CodeChallenge.Domain.Exceptions.Parcel;
using CodeChallenge.Domain.Model.Parcel;

namespace CodeChallenge.Domain
{
    public class ParcelFactory
    {
	    public static BaseParcel CreateParcel(int weight, int width, int height, int depth)
	    {
		    int volume = CalculateVolume(width, height, depth);

			if (weight > 50)
			{
				throw new RejectedParcelException();
			}
			else if (weight > 10)
			{
				return new HeavyParcel(weight, width, height, depth, volume);
			}
			else if (volume < 1500)
			{
				return new SmallParcel(weight, width, height, depth, volume);
			}
			else if (volume < 2500)
			{
				return new MediumParcel(weight, width, height, depth, volume);
			}
			else
			{
				return new LargeParcel(weight, width, height, depth, volume);
			}
		}

		public static int CalculateVolume(int width, int height, int depth)
		{
			return width * height * depth;
		}
	}
}
