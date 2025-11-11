using System.Text.RegularExpressions;
using BYT_Entities.Enums;

namespace BYT_Entities.Models;

public class Seat
{
    private string _code;
    private SeatType _type;
    
    // 92A, 05J, 96Z
    private static readonly Regex SeatCodeSyntax = new Regex(@"^[0-9]{2}[A-Z]$", RegexOptions.Compiled);


    public string Code
    {
        get => _code;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Seat code can not be empty.");

            if (!SeatCodeSyntax.IsMatch(value))
                throw new ArgumentException("Seat code must have the pattern of two digits followed by one uppercase letter.");

            _code = value;
        }
    }

    public SeatType Type
    {
        get => _type;
        set
        {
            if (!Enum.IsDefined(typeof(SeatType), value))
                throw new ArgumentException("Invalid seat type.");
        }
    }

    public Seat(string code, SeatType type)
    {
        Code = code;
        Type = type;
    }

    public static Seat GetSeatByCode(List<Seat> seats, string code)
    {
        foreach (var seat in seats)
        {
            if(seat.Code.Equals(code, StringComparison.Ordinal))
                return seat;
        }
        return null;
    }
}