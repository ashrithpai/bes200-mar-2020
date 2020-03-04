using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static LibraryApi.Domain.Reservation;

namespace LibraryApi.Models
{
    public class GetReservationItemResponse
    {
        public int Id { get; set; }

        public string For { get; set; }


        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ReservationStatus Status { get; set; }

        public DateTime ReservationCreated { get; set; }

        public List<string> Books { get; set; }
    }
}
