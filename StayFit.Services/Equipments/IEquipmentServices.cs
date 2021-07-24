using System.Collections.Generic;

namespace StayFit.Services.Equipments
{
    public interface IEquipmentServices
    {
        bool DoesEquipmentExist(int id);
        IEnumerable<EquipmentServiceModel> All();
    }
}
