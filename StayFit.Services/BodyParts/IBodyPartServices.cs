using System.Collections.Generic;

namespace StayFit.Services.BodyParts
{
    public interface IBodyPartServices
    {
        bool DoesBodyPartExist(int id);
        IEnumerable<BodyPartServiceModel> All();
    }
}
