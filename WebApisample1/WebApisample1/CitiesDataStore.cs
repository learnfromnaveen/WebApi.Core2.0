using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApisample1
{
    public class CitiesDataStore
    {
        public static CitiesDataStore Current { get; } = new CitiesDataStore();

        public List<WebApisample1.Models.CityDto> Cities { get; set; }

        public CitiesDataStore()
        {
            Cities = new List<Models.CityDto>
            {
                new Models.CityDto
                {
                    Id = 1, 
                    Name ="New York City", 
                    Description ="The one with that big park", 
                    PointsOfInterest = new List<Models.PointOfInterestDto>
                    {
                        new Models.PointOfInterestDto
                        {
                             Id = 1,
                             Name = "Central Park",
                             Description = "The most visited urban park in the United States."
                        }, 

                        new Models.PointOfInterestDto
                        {
                             Id = 2,
                             Name = "Empire State Building",
                             Description = "A 102-story skyscraper located in Midtown Manhattan."
                        }
                    }
                },
                new Models.CityDto
                {
                    Id = 2,
                    Name ="Antwerp",
                    Description ="The one with the cathedral that was never really finished",
                    PointsOfInterest = new List<Models.PointOfInterestDto>
                    {
                        new Models.PointOfInterestDto
                        {
                             Id = 3,
                             Name = "Cathedral of Our Lady",
                             Description = "A Gothic style cathedral, conceived by architects Jan and Pieter Appelmans."
                        },

                        new Models.PointOfInterestDto
                        {
                            Id = 4,
                             Name = "Antwerp Central Station",
                             Description = "The the finest example of railway architecture in Belgium."
                        }
                    }
                },
                new Models.CityDto
                {
                    Id = 3,
                    Name ="Paris",
                    Description ="The one with that big tower",
                    PointsOfInterest = new List<Models.PointOfInterestDto>
                    {
                        new Models.PointOfInterestDto
                        {
                            Id = 5,
                             Name = "Eiffel Tower",
                             Description = "A wrought iron lattice tower on the Champ de Mars, named after engineer Gustave Eiffel."
                        },

                        new Models.PointOfInterestDto
                        {
                             Id = 6,
                             Name = "The Louvre",
                             Description = "The world's largest museum."
                        }
                    }
                }
            };
        }
    }
}
