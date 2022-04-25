using Server.Models;

namespace Server.Services.Interfaces;

public interface IReservationService:IService<Reservation>
{
    Reservation? Get(int id);
}
