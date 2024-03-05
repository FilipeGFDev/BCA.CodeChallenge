namespace Car.Auction.Management.System.Models.ErrorCodes;

public static class ErrorCodes
{
    public static class Application
    {
        public static readonly ErrorCode UnexpectedError = new(
            "1",
            "An unexpected error occurred during the request.");

        public static readonly ErrorCode InvalidJsonPatchOperationType = new(
            "2",
            "HTTP Patch request only supports 'Replace' operations.");
    }

    public static class Vehicle
    {
        public static readonly ErrorCode MandatoryVehicleType = new(
            "100",
            "'VehicleType' field is required.");

        public static readonly ErrorCode InvalidVehicleType = new(
            "101",
            "'VehicleType' provided is invalid.");

        public static readonly ErrorCode MandatoryVehicleManufacturer = new(
            "102",
            "Vehicle 'Manufacturer' is required.");

        public static readonly ErrorCode MandatoryVehicleModel = new(
            "103",
            "Vehicle 'Model' is required.");

        public static readonly ErrorCode InvalidVehicleYear = new(
            "104",
            "Vehicle 'Year' must be greater or equal 1885.");

        public static readonly ErrorCode InvalidVehicleStartingBid = new(
            "105",
            "Vehicle 'StartingBid' must be greater or equal 1.");

        public static readonly ErrorCode InvalidVehicleDoorsNumber = new(
            "106",
            "Vehicle 'DoorsNumber' must be greater or equal 3, when 'VehicleType' is 'Hatchback' or 'Sedan'.");

        public static readonly ErrorCode InvalidVehicleSeatsNumber = new(
            "107",
            "Vehicle 'SeatsNumber' must be greater than 0, when 'VehicleType' is 'SUV'.");

        public static readonly ErrorCode InvalidVehicleLoadCapacity = new(
            "108",
            "Vehicle 'LoadCapacity' must be greater than 0, when 'VehicleType' is 'Truck'.");
    }

    public static class Auction
    {
        public static readonly ErrorCode MandatoryAuctionVehicleId = new(
            "200",
            "'VehicleId' field is required, when creating a new Auction.");

        public static readonly ErrorCode InvalidAuctionVehicleId = new(
            "201",
            "'VehicleId' provided is invalid.");

        public static readonly ErrorCode AuctionVehicleNotFound = new(
            "202",
            "Vehicle with provided Id wasn't found.");

        public static readonly ErrorCode AuctionVehicleIsUnavailable = new(
            "203",
            "Vehicle with provided Id is unavailable.");

        public static readonly ErrorCode AuctionVehicleFoundedOnExistentAuction = new(
            "204",
            "Vehicle with provided Id was founded on an existent Auction.");

        public static readonly ErrorCode InvalidAuctionFieldUpdate = new(
            "205",
            "Only Auction 'IsActive' field can be updated.");

        public static readonly ErrorCode InvalidAuctionIsActiveFieldUpdate = new(
            "205",
            "Auction 'IsActive' only can be updated to 'false'");
    }

    public static class Bid
    {
        public static readonly ErrorCode MandatoryBidUserId = new(
            "300",
            "'UserId' field is required, when creating a new Bid.");

        public static readonly ErrorCode InvalidBidUserId = new(
            "301",
            "'UserId' provided is invalid.");

        public static readonly ErrorCode MandatoryBidAuctionId = new(
            "302",
            "'AuctionId' field is required, when creating a new Bid.");

        public static readonly ErrorCode BidAuctionNotFound = new(
            "303",
            "Auction with provided Id wasn't found.");

        public static readonly ErrorCode BidAuctionUnavailable = new(
            "304",
            "Provided Auction to place the bid is already closed.");

        public static readonly ErrorCode AmountSmallerThanVehicleStaringBid = new(
            "305",
            "'Amount' value of the Bid is smaller than the vehicle 'StartingBid'.");

        public static readonly ErrorCode AmountSmallerThanCurrentBid = new(
            "306",
            "'Amount' value of the Bid is smaller than the current Bid.");
    }
}