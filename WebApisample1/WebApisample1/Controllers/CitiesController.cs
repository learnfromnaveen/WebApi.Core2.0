using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApisample1.Services;  

namespace WebApisample1.Controllers
{
    [Route("api/cities")]
    public class CitiesController : Controller
    {
        private ICityInfoRepository _citiInfoRepository;  

        public CitiesController(ICityInfoRepository cityInfoRepository)
        {
            _citiInfoRepository = cityInfoRepository;
        }

        [HttpGet()]
        public IActionResult GetCities()
        {
            //return Ok(CitiesDataStore.Current.Cities);
            var cityEntities = _citiInfoRepository.GetCities();
            //var results = new List<Models.CityWithoutPointsOfInterestDto>();
            //foreach(var cityEntity in cityEntities)
            //{
            //    results.Add(new Models.CityWithoutPointsOfInterestDto
            //    {
            //        Id = cityEntity.Id,
            //        Name = cityEntity.Name,
            //        Description = cityEntity.Description
            //    });
            //}

            //Automapper  
            var results = AutoMapper.Mapper.Map<IEnumerable<Models.CityWithoutPointsOfInterestDto>>(cityEntities);

            return Ok(results);
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id, bool includePointsOfInterest = false)
        {
            var city = _citiInfoRepository.GetCity(id, includePointsOfInterest);  

            if(city == null)
            {
                return NotFound();
            }

            if (includePointsOfInterest)
            {
                //var cityResult = new Models.CityDto
                //{
                //    Id = city.Id,  
                //    Name  = city.Name,  
                //    Description = city.Description
                //};

                //foreach(var poi in city.PointsOfInterest)
                //{
                //    cityResult.PointsOfInterest.Add(
                //        new Models.PointOfInterestDto
                //        {
                //            Id = poi.Id, 
                //            Name = poi.Name,  
                //            Description = poi.Description
                //        });
                //}

                var cityResult = AutoMapper.Mapper.Map<Models.CityDto>(city);

                return Ok(cityResult);
            }

            //var cityWitoutPointOfInterests = new Models.CityWithoutPointsOfInterestDto
            //{
            //    Id  =  city.Id,  
            //    Name  = city.Name,  
            //    Description = city.Description
            //};

            var cityWitoutPointOfInterests = AutoMapper.Mapper.Map<Models.CityWithoutPointsOfInterestDto>(city);

            return Ok(cityWitoutPointOfInterests);

            //var cityToReturn = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == id);  
            //if( cityToReturn == null )
            //{
            //    return NotFound();
            //}

            //return Ok(cityToReturn);
        }
    }
}
