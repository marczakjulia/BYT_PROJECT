namespace BYT_Entities.Complex;

public class Address
{
    public string Street { get; set; }
    public string BuildingNumber { get; set; }
    public string City { get; set; }
    public string PostalCode { get; set; }
    public string Country { get; set; }

    public Address() { }

    public Address(string street, string buildingNumber, string city, string postalCode, string country)
    {
        Street = street;
        BuildingNumber = buildingNumber;
        City = city;
        PostalCode = postalCode;
        Country = country;
    }

    public override string ToString()
    {
        return $"{Street} {BuildingNumber}, {PostalCode} {City}, {Country}";
    }
}
