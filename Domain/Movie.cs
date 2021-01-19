using System;
using System.Collections.Generic;
using System.Text;

namespace IMDb.Domain
{
    class Movie
    {
        public int Id { get; protected set; }
        public string Title { get; protected set; }
        public string Description { get; protected set; }

        public DateTime Year { get; protected set; }

        public string Genre { get; protected set; }

        public string Director { get; protected set; }

        public ProductionCompany ProductionCompany { get; protected set; }

        public int ProductionCompanyId { get; protected set; }


        public Movie()
        {
            
        }

        public Movie(string title, string description, DateTime year, string genre, string director)
        {
            Title = title;
            Description = description;
            Year = year;
            Genre = genre;
            Director = director;
        }

        public List<MovieActor> Actors { get; protected set; } = new List<MovieActor>();
    }
}
