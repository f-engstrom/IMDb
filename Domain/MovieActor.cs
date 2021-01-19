using System;
using System.Collections.Generic;
using System.Text;

namespace IMDb.Domain
{
    class MovieActor
    {
        public int ActorId { get; protected set; }
        public int MovieId { get; protected set; }
        public Movie movie { get; protected set; }

        public Actor Actor { get; protected set; }


        public MovieActor()
        {
            
        }

        public MovieActor(Actor actor)
        {
            Actor = actor;
        }
    }
}
