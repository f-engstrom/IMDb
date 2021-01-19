using System;
using System.Collections.Generic;
using System.Text;

namespace IMDb.Domain
{
    class ProductionCompany
    {

        public int Id { get; protected set; }
        public string Name { get; protected set; }


        public ProductionCompany()
        {
            
        }

        public ProductionCompany( string name)
        {
            Name = name;
        }
        public List<Movie> Movies { get; protected set; } = new List<Movie>();


    }
}
