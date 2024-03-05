namespace Car.Auction.Management.System.Models.Core;

public struct ChangeRequest<T>
{
    private readonly T value;

    public ChangeRequest(T value)
    {
        this.value = value;
        IsSet = true;
    }

    public bool IsSet { get; }

    public T Value => IsSet ? value : throw new ArgumentException("The change request's value wasn't set");

    public static implicit operator ChangeRequest<T>(T input) => new(input);

    public T GetValueOrDefault(T defaultValue) => IsSet ? value : defaultValue;
}