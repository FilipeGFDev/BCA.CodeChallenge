namespace Car.Auction.Management.System.Contracts.Web.Vehicle;

public class VehicleRequest
{
    public VehicleType? VehicleType { get; set; }

    public string Manufacturer { get; set; }

    public string Model { get; set; }

    public int Year { get; set; }

    public decimal StartingBid { get; set; }

    public int? DoorsNumber { get; set; }

    public int? LoadCapacity { get; set; }

    public int? SeatsNumber { get; set; }
}