using server.DTO;

namespace server.IRepositories;

public interface IReservationRepository
{
    public Task<ResultDTO> createReservation(ReservationDTO data, string id);
    public Task<ResultDTO> editReservation(UpdateReservationDTO data, int id);

}