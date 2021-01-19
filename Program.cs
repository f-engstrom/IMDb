using System;
using System.Linq;
using System.Threading;
using IMDb.Data;
using IMDb.Domain;
using static System.Console;

namespace IMDb
{

    class Program
    {
        public static IMDbContext context = new IMDbContext();

        static void Main(string[] args)
        {
            Menu();
        }

        static void Menu()
        {
            bool shouldNotExit = true;

            while (shouldNotExit)
            {


                Clear();




                WriteLine("1. Lägg till skådespelare");
                WriteLine("2. Lista skådespelare");
                WriteLine("3. Lägg till filmbolag");
                WriteLine("4. Lägg till film");
                WriteLine("5. Lägg till skådespelare till film");
                WriteLine("6. Lista filmer");
                WriteLine("7. Avsluta");

                ConsoleKeyInfo keyPressed = ReadKey(true);


                switch (keyPressed.Key)
                {

                    case ConsoleKey.D1:

                        AddActor();

                        break;

                    case ConsoleKey.D2:
                        ListActors();

                        break;

                    case ConsoleKey.D3:
                        AddProductionCompany();
                        break;

                    case ConsoleKey.D4:
                        AddMovie();
                        break;
                    case ConsoleKey.D5:
                        AddActorToMovie();
                        break;
                    case ConsoleKey.D6:
                        ListMovies();
                        break;



                    case ConsoleKey.D7:

                        shouldNotExit = false;

                        break;

                }

            }
        }

        private static void ListMovies()
        {

            Clear();


            var productionCompanyMovies = context.ProductionCompany.OrderBy(t => t.Name).Select(t => new
            {
                ProductionCompanyName = t.Movies.Count > 0 ? t.Name : string.Empty,
                Movies = t.Movies.OrderBy(t => t.Title).Select(m => new
                {

                    m.Title,
                    m.Year,
                    m.Description,
                    m.Genre,
                    Actors = m.Actors.OrderBy(a => a.Actor.LastName).Select(a => new
                    {
                        FullName = $"{a.Actor.LastName}, {a.Actor.FirstName}"

                    })

                })


            }).ToList();

            //var productionCompanyMovies = productionCompanyMoviesDB.OrderBy(t => t.ProductionCompanyName)
            //    .GroupBy(t => t.ProductionCompanyName); // t => new
            //{
            //    Key = t.ProductionCompanyName,

            //    Movies = t.Movies.OrderBy(m => m.Title).GroupBy(m => m.Title, m => new
            //    {

            //        Key = m.Title,
            //        m.Year,
            //        m.Description,
            //        m.Genre,
            //        Actors = m.Actors.OrderBy(a => a.FullName),

            //    })


            //});




            foreach (var productionCompany in productionCompanyMovies.Where(x => string.IsNullOrEmpty(x.ProductionCompanyName) == false))
            {
                WriteLine("\n".PadRight(50, '*'));
                WriteLine(productionCompany.ProductionCompanyName);

                WriteLine("".PadRight(50, '-'));
                foreach (var movie in productionCompany.Movies)
                {
                    WriteLine($"Filmtitel:{movie.Title}");
                    WriteLine($"Produktionsår:{movie.Year.Year}");

                    WriteLine($"Beskrivning:{movie.Description}");

                    WriteLine($"Genre:{movie.Genre}");

                    WriteLine("Skådespelare:");
                    foreach (var actor in movie.Actors)
                    {
                        WriteLine(actor.FullName);


                    }

                    if (productionCompany.Movies.Count() > 1)
                    {
                        WriteLine("".PadRight(50, '-'));

                    }

                }


            }
            WriteLine("\n".PadRight(50, '*'));


            ReadKey();

        }

        private static void AddActorToMovie()
        {

            Clear();

            bool customerExists = false;
            bool incorrectKey = true;
            bool shouldNotExit = true;




            do
            {


                WriteLine("Skådespelare (personnummer): ");
                string socialSecurityNumber = ReadLine();

                WriteLine("Filmtitel: ");
                string movieTitle = ReadLine();




                WriteLine("Är detta korrekt? (J)a eller (N)ej");

                ConsoleKeyInfo consoleKeyInfo;

                do
                {

                    consoleKeyInfo = ReadKey(true);

                    incorrectKey = !(consoleKeyInfo.Key == ConsoleKey.J || consoleKeyInfo.Key == ConsoleKey.N);



                } while (incorrectKey);


                if (consoleKeyInfo.Key == ConsoleKey.J)
                {

                    var actor = context.Actor.FirstOrDefault(a => a.SocialSecurityNumber == socialSecurityNumber);
                    var movie = context.Movie.FirstOrDefault(m => m.Title == movieTitle);




                    if (actor != null)
                    {
                        MovieActor movieActor = new MovieActor(actor);
                        movie.Actors.Add(movieActor);
                        context.SaveChanges();

                        shouldNotExit = false;
                        Clear();
                        WriteLine("Skådespelare registrerad på film");
                        Thread.Sleep(1000);

                    }
                    else if (actor == null)
                    {
                        shouldNotExit = false;
                        Clear();
                        WriteLine("Skådespelare ej registrerad");
                        Thread.Sleep(1000);

                    }


                }

                Clear();


            } while (shouldNotExit);



        }

        private static void AddMovie()
        {
            Clear();

            bool customerExists = false;
            bool isCorrectKey = true;
            bool shouldNotExit = true;




            do
            {

                WriteLine("Titel: ");
                string title = ReadLine();
                WriteLine("Beskrivning: ");
                string description = ReadLine();
                WriteLine("Produktionsår: ");
                DateTime year = Convert.ToDateTime(ReadLine());
                WriteLine("Genres: ");
                string genre = ReadLine();
                WriteLine("Reggisör: ");
                string director = ReadLine();
                WriteLine("Produktionsbolag: ");
                string productionCompanyName = ReadLine();


                Movie movie = new Movie(title, description, year, genre, director);

                WriteLine("Är detta korrekt? (J)a eller (N)ej");

                ConsoleKeyInfo consoleKeyInfo;

                do
                {

                    consoleKeyInfo = ReadKey(true);

                    isCorrectKey = !(consoleKeyInfo.Key == ConsoleKey.J || consoleKeyInfo.Key == ConsoleKey.N);



                } while (isCorrectKey);


                if (consoleKeyInfo.Key == ConsoleKey.J)
                {


                    bool isMovie = context.Movie.Any(m => m.Title == title && m.Year == year && m.Director == director);

                    bool isproductionCompany = context.ProductionCompany.Any(p => p.Name == productionCompanyName);

                    var productionCompany = context.ProductionCompany.FirstOrDefault(x => x.Name == productionCompanyName);

                    if (!isMovie && isproductionCompany)
                    {
                        productionCompany.Movies.Add(movie);
                        context.SaveChanges();

                        shouldNotExit = false;
                        Clear();
                        WriteLine("Film registrerad");
                        Thread.Sleep(1000);

                    }
                    else if (isMovie)
                    {
                        shouldNotExit = false;
                        Clear();
                        WriteLine("Film redan registrerad");
                        Thread.Sleep(1000);

                    }
                    else if (!isproductionCompany)
                    {
                        shouldNotExit = false;
                        Clear();
                        WriteLine("Produktionsbolag ej registrerat");
                        Thread.Sleep(1000);

                    }


                }

                Clear();


            } while (shouldNotExit);

        }

        private static void AddProductionCompany()
        {
            Clear();

            bool customerExists = false;
            bool inCorrectKey = true;
            bool shouldNotExit = true;




            do
            {

                WriteLine("Namn: ");
                string name = ReadLine();

                ProductionCompany productionCompany = new ProductionCompany(name);

                WriteLine("Är detta korrekt? (J)a eller (N)ej");

                ConsoleKeyInfo consoleKeyInfo;

                do
                {

                    consoleKeyInfo = ReadKey(true);
                    inCorrectKey = !(consoleKeyInfo.Key == ConsoleKey.J || consoleKeyInfo.Key == ConsoleKey.N);



                } while (inCorrectKey);


                if (consoleKeyInfo.Key == ConsoleKey.J)
                {


                    bool isCompany = context.ProductionCompany.Any(p => p.Name == name);

                    if (!isCompany)
                    {
                        context.ProductionCompany.Add(productionCompany);
                        context.SaveChanges();

                        shouldNotExit = false;
                        Clear();
                        WriteLine("Produktionsbolag registrerat");
                        Thread.Sleep(1000);

                    }
                    else if (isCompany)
                    {
                        shouldNotExit = false;
                        Clear();
                        WriteLine("Produktionsbolag redan registrerat");
                        Thread.Sleep(1000);

                    }


                }

                Clear();


            } while (shouldNotExit);

        }

        private static void ListActors()
        {
            Clear();

            var actors = context.Actor.OrderBy(a => a.LastName).Select(a => new
            {
                FullName = $"{a.LastName} {a.FirstName}",
                SocialSecurityNumber = a.SocialSecurityNumber,
                Address = $"{a.Address.Street}, {a.Address.PostCode} {a.Address.City}"

            }).ToList();

            WriteLine("Skådespelare");
            WriteLine("".PadRight(50, '-'));

            foreach (var actor in actors)
            {
                WriteLine($"{actor.FullName}");
                WriteLine($"personummer: {actor.SocialSecurityNumber} ");
                WriteLine($"Address: { actor.Address}");
                WriteLine("".PadRight(50, '-'));

            }

            ReadKey();

        }

        private static void AddActor()
        {

            Clear();

            bool customerExists = false;
            bool shoudNotExit = true;
            bool inCorrectKey = true;



            do
            {

                WriteLine("First Name: ");
                string firstName = ReadLine();
                WriteLine("Last Name: ");
                string lastName = ReadLine();
                WriteLine("Social security number: ");
                string socialSecurityNumber = ReadLine();

                WriteLine("Street: ");
                string street = ReadLine();
                WriteLine("Postcode: ");
                string postcode = ReadLine();
                WriteLine("City: ");
                string city = ReadLine();

                Address address = new Address(street, postcode, city);


                Actor actor = new Actor(firstName, lastName, socialSecurityNumber, address);

                WriteLine("Är detta korrekt? (J)a eller (N)ej");

                ConsoleKeyInfo consoleKeyInfo;

                do
                {

                    consoleKeyInfo = ReadKey(true);

                    inCorrectKey = !(consoleKeyInfo.Key == ConsoleKey.J || consoleKeyInfo.Key == ConsoleKey.N);



                } while (inCorrectKey);


                if (consoleKeyInfo.Key == ConsoleKey.J)
                {


                    bool isActor = context.Actor.Any(r => r.SocialSecurityNumber == socialSecurityNumber);

                    if (!isActor)
                    {
                        context.Actor.Add(actor);
                        context.SaveChanges();

                        shoudNotExit = false;
                        Clear();
                        WriteLine("Skådespelare registrerad");
                        Thread.Sleep(1000);

                    }
                    else if (isActor)
                    {
                        shoudNotExit = false;
                        Clear();
                        WriteLine("Skådespelare redan registrerad");
                        Thread.Sleep(1000);

                    }


                }

                Clear();


            } while (shoudNotExit);



        }
    }
}
