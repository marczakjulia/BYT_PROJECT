using BYT_Entities.Enums;
using System;
using System.Collections.Generic;

namespace BYT_Entities.Models
{
    public class Auditorium
    {
        public int Id { get; set; }
        private string _name;
        private AuditoriumScreenType _auditoriumScreenType;
        private AuditoriumSoundsSystem _soundSystem;
        private List<Seat> _seats;

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Auditorium name cannot be empty.");
                _name = value;
            }
        }

        public AuditoriumScreenType AuditoriumScreenType
        {
            get => _auditoriumScreenType;
            set
            {
                if (!Enum.IsDefined(typeof(AuditoriumScreenType), value))
                    throw new ArgumentException("Invalid screen type.");
                _auditoriumScreenType = value;
            }
        }

        public AuditoriumSoundsSystem SoundSystem
        {
            get => _soundSystem;
            set
            {
                if (!Enum.IsDefined(typeof(AuditoriumSoundsSystem), value))
                    throw new ArgumentException("Invalid sound system type.");
                _soundSystem = value;
            }
        }

        public List<Seat> Seats
        {
            get => _seats;
            set
            {
                if (value == null)
                    throw new ArgumentException("Seats cannot be null.");
                if (value.Count < 12)
                    throw new ArgumentException("There must be at least 12 seats");
                _seats = value;
            }
        }

        public Auditorium(string name, AuditoriumScreenType auditoriumScreenType, AuditoriumSoundsSystem soundSystem, List<Seat> seats, int id)
        {
            Name = name;
            AuditoriumScreenType = auditoriumScreenType;
            SoundSystem = soundSystem;
            Seats = seats;
            Id = id;
        }

        public void AddSeat(Seat seat)
        {
            if (seat == null)
                throw new ArgumentException("Cannot add null seat.");

            _seats.Add(seat);
        }

        public void RemoveSeat(Seat seat)
        {
            if (seat == null)
                throw new ArgumentException("Cannot remove null seat.");

            if (_seats.Count <= 12)
                throw new InvalidOperationException("Cannot remove seat: auditorium must have at least 12 seats.");

            _seats.Remove(seat);
        }
    }
}
