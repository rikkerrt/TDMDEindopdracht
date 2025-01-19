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

        public async Task Delete(int id)
        {
            await _connection.DeleteAsync(id);
        }

        public async Task Drop()
        {
        //    await _connection.DropTableAsync<T>();
        }

        public async Task Init()
        {
            //
        }

        public Task updateDatabase()
        {
            throw new NotImplementedException();
        }
    }
}
