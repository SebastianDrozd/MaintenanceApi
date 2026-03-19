using MaintenanceApi.Data.Dapper;
using MaintenanceApi.Dto.Mechanics;

namespace MaintenanceApi.Service
{
    public class MechanicService
    {
        private readonly MechanicsRepo _repo;

        public MechanicService(MechanicsRepo repo) 
        {
            _repo = repo;
        }

        public async Task<List<MechanicResponse>> GetAllMechanics() 
        {
            var mechanics = await _repo.GetAllMechanics();
            return mechanics;
        }
    }
}
