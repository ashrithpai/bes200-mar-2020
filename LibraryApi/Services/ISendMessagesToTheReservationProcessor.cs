using LibraryApi.Models;

namespace LibraryApi.Services
{
    public interface ISendMessagesToTheReservationProcessor
    {
        void SendReservationForProcessing(GetReservationItemResponse reservation);
    }
}