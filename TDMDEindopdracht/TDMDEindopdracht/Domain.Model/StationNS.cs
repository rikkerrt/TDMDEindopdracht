using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDMDEindopdracht.Domain.Model
{
    public class StationNS
    {
        public string name { get; set; }
        [PrimaryKey, AutoIncrement, Column("Index")]
        public int index { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }  
    }
}
