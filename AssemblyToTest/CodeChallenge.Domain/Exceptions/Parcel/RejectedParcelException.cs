using System;

namespace CodeChallenge.Domain.Exceptions.Parcel
{
    public class RejectedParcelException : Exception
    {
	    public RejectedParcelException()
		    : base("Parcel exceeds maximum allowable size.")
	    {
	    }
    }
}
