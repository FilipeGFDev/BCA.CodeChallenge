namespace Car.Auction.Management.System.Models.Exceptions.Vehicle;

public class InvalidVehicleTypeException(string errorCode, string? message) : Exception(message)
{
    public string ErrorCode { get; } = errorCode;
}