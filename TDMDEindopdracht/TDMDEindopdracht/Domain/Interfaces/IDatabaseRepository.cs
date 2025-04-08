using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDMDEindopdracht.Domain.Models;

namespace TDMDEindopdracht.Domain.Interfaces
{
    public interface IDatabaseRepository
    {
        Task Init();
        Task Drop();
        Task Delete(int id);
        Task updateDatabase(string name);
        Task addStation(StationNS station);
        Task<List<string>> getVisitedStations();

    }
}
