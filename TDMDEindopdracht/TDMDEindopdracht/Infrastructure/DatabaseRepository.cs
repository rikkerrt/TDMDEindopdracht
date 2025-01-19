using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDMDEindopdracht.Domain.Model;

namespace TDMDEindopdracht.Infrastructure
{
    public class DatabaseRepository(string path) : IDatabaseRepository
    {
        private readonly SQLiteAsyncConnection _connection = new(path);

        public async Task addStation(StationNS station)
        {
            await _connection.InsertAsync(station);
        }

        public async Task Delete(int id)
        {
            await _connection.DeleteAsync<StationNS>(id);
        }

        public async Task Drop()
        {
            await _connection.DropTableAsync<StationNS>();
        }

        public Task<List<string>> getVisitedStations()
        {
            throw new NotImplementedException();
        }

        public async Task Init()
        {
            await _connection.CreateTableAsync<StationNS>();
        }
        public async Task updateDatabase(string name)
        {
            var stationInDatabase = await _connection.FindAsync<StationNS>(name);
            if (stationInDatabase != null)
            {
                await _connection.UpdateAsync(stationInDatabase);
            }
        }
    }
}
