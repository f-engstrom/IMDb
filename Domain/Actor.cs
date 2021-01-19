using System;
using System.Collections.Generic;
using System.Text;

namespace IMDb.Domain
{
    class Actor
    {

        public int Id { get; protected set; }
        public string FirstName { get; protected set; }
        public string LastName { get; protected set; }
        public string  SocialSecurityNumber { get; protected set; }

        public Address Address { get; protected set; }

        public List<MovieActor> Movies { get; protected set; } = new List<MovieActor>();


        public Actor()
        {
            
        }

        public Actor(string firstName, string lastName, string socialSecurityNumber, Address address)
        {
            FirstName = firstName;
            LastName = lastName;
            SocialSecurityNumber = socialSecurityNumber;
            Address = address;
        }
    }
}
