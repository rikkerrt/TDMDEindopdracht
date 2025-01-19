using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDMDEindopdracht.Domain.Model
{
    public interface IDatabaseRepository
    {
        Task Init();
        Task Drop();
        Task Delete(int id);
        Task updateDatabase();
    }
}
