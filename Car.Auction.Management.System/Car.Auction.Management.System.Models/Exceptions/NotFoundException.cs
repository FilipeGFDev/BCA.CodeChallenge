namespace Car.Auction.Management.System.Models.Exceptions;

public class NotFoundException(string entityName, object? entityId)
    : Exception($"There is no '{entityName}' with the ID '{entityId}'");
