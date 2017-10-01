using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using WebApisample1.Services;  

namespace WebApisample1.Controllers
{
    [Route("api/cities")]
    public class PointsOfInterestController : Controller
    {
        private ILogger<PointsOfInterestController> _logger;
        private IMailService _mailService;
        private ICityInfoRepository _citiInfoRepository;


        public PointsOfInterestController(ILogger<PointsOfInterestController> logger
            , IMailService mailService
            , ICityInfoRepository cityInfoRepository)
        {
            _logger = logger;
            _mailService = mailService;
            _citiInfoRepository = cityInfoRepository;
        }

        [Route("{cityid}/pointsofinterest")]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            try
            {

                //var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

                //if (city == null)
                //{
                //    _logger.LogInformation($"City with id {cityId} wasn't found when accessing points of interest.");
                //    return NotFound();
                //}

                //return Ok(city.PointsOfInterest);

                if (!_citiInfoRepository.CityExists(cityId))
                {
                    _logger.LogInformation($"City with id {cityId} wasn't found when accessing points of interest.");
                    return NotFound();
                }

                var pointsOfInterestForCity = _citiInfoRepository.GetPointsOfInterestForCity(cityId);

                //var pointsOfInterestForCityResults = new List<Models.PointOfInterestDto>();  
                //foreach(var poi in pointsOfInterestForCity)
                //{
                //    pointsOfInterestForCityResults.Add(new Models.PointOfInterestDto
                //    {
                //        Id = poi.Id,  
                //        Name = poi.Name,  
                //        Description = poi.Description
                //    }); 
                //}

                var pointsOfInterestForCityResults = AutoMapper.Mapper.Map<IEnumerable<Models.PointOfInterestDto>>(pointsOfInterestForCity);

                return Ok(pointsOfInterestForCityResults);
            }
            catch(Exception ex)
            {
                _logger.LogCritical($"Exception while getting points of interest for city with id {cityId}.", ex);
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [Route("{cityid}/pointsofinterest/{id}", Name = "GetPointOfInterest")]
        public IActionResult GetPointOfInterest(int cityId, int id)
        {
            //var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            //if (city == null)
            //{
            //    return NotFound();
            //}

            //var pointOfInterest = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);

            //if(pointOfInterest == null)
            //{
            //    return NotFound();
            //}

            //return Ok(pointOfInterest);

            if (!_citiInfoRepository.CityExists(cityId))
            {
                return NotFound();
            }

            var poinOfInterest = _citiInfoRepository.GetPointOfInterestForCity(cityId, id); 

            if( poinOfInterest == null)
            {
                return NotFound(); 
            }

            //var pointOfInterestResult = new Models.PointOfInterestDto
            //{
            //    Id = poinOfInterest.Id,  
            //    Name = poinOfInterest.Name, 
            //    Description = poinOfInterest.Description
            //};

            var pointOfInterestResult = AutoMapper.Mapper.Map<Models.PointOfInterestDto>(poinOfInterest);

            return Ok(pointOfInterestResult);

        }

        [HttpPost("{cityid}/pointsofinterest")]
        public IActionResult CreatePointOfInterest(int cityId, 
            [FromBody] Models.PointOfInterestForCreationDto pointOfInterest)
        {
            if(pointOfInterest == null)
            {
                return BadRequest();
            }

            if(pointOfInterest.Description == pointOfInterest.Name)
            {
                ModelState.AddModelError("Description", "The provided description should be different from name.");
            }

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //var city = _citiInfoRepository.GetCity(cityId)

            //if(city == null)
            //{
            //    return NotFound();
            //}


            //var maxPointOfInterestId = CitiesDataStore.Current.Cities.SelectMany(c => c.PointsOfInterest).Max(p => p.Id);

            //var finalPointOfInterest = new Models.PointOfInterestDto
            //{
            //    Id = ++maxPointOfInterestId,  
            //    Name = pointOfInterest.Name,  
            //    Description = pointOfInterest.Description
            //};

            //city.PointsOfInterest.Add(finalPointOfInterest);

            //return CreatedAtRoute("GetPointOfInterest", new { cityId = cityId, id = finalPointOfInterest.Id }, finalPointOfInterest);


            #region [Repository Create]
            if (!_citiInfoRepository.CityExists(cityId))
            {
                return NotFound();
            }

            var finalPointOfInterest = AutoMapper.Mapper.Map<Entities.PointOfInterest>(pointOfInterest);

            _citiInfoRepository.AddPointOfInterestForCity(cityId, finalPointOfInterest);

            if (!_citiInfoRepository.Save())
            {
                return StatusCode(500, "A problem happened while hadling your request");
            }

            var createdPointOfInterest = AutoMapper.Mapper.Map<Models.PointOfInterestDto>(finalPointOfInterest);

            return CreatedAtRoute("GetPointOfInterest", new { cityId = cityId, id = finalPointOfInterest.Id }, createdPointOfInterest);

            #endregion 
        }

        [HttpPut("{cityid}/pointsofinterest/{id}")]
        public IActionResult UpdatePointOfInterest(int cityId, int id,
            [FromBody] Models.PointOfInterestForCreationDto pointOfInterest)
        {
            if (pointOfInterest == null)
            {
                return BadRequest();
            }

            if (pointOfInterest.Description == pointOfInterest.Name)
            {
                ModelState.AddModelError("Description", "The provided description should be different from name.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            //if (city == null)
            //{
            //    return NotFound();
            //}

            //var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);

            //if (pointOfInterestFromStore == null)
            //{
            //    return NotFound();
            //}

            //pointOfInterestFromStore.Name = pointOfInterest.Name;
            //pointOfInterestFromStore.Description = pointOfInterest.Description;

            if (!_citiInfoRepository.CityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity = _citiInfoRepository.GetPointOfInterestForCity(cityId, id);

            if(pointOfInterestEntity == null)
            {
                return NotFound();  
            }

            AutoMapper.Mapper.Map(pointOfInterest, pointOfInterestEntity);

            if (!_citiInfoRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
             
            return NoContent();
        }

        [HttpPatch("{cityid}/pointsofinterest/{id}")]
        public IActionResult PartiallyUpdatePointOfInterest(int cityId, int id,
            [FromBody] JsonPatchDocument<Models.PointOfInterestForUpdateDto> patchDoc)
        {
            if(patchDoc == null)
            {
                return BadRequest();
            }

            //    var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            //    if (city == null)
            //    {
            //        return NotFound();
            //    }

            //    var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);

            //    if (pointOfInterestFromStore == null)
            //    {
            //        return NotFound();
            //    }

            //    var pointOfInterestToPatch = new Models.PointOfInterestForUpdateDto
            //    {
            //        Name = pointOfInterestFromStore.Name,  
            //        Description = pointOfInterestFromStore.Description
            //    };

            //    patchDoc.ApplyTo(pointOfInterestToPatch, ModelState);

            //    if (!ModelState.IsValid)
            //    {
            //        return BadRequest(ModelState);
            //    }

            //    if ( pointOfInterestToPatch.Description  == pointOfInterestToPatch.Name)
            //    {
            //        ModelState.AddModelError("Description", "The provided description should be different from name.");
            //    }

            //    TryValidateModel(pointOfInterestToPatch);

            //    if (!ModelState.IsValid)
            //    {
            //        return BadRequest(ModelState);
            //    }



            //    pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
            //    pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;

            if (!_citiInfoRepository.CityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity = _citiInfoRepository.GetPointOfInterestForCity(cityId, id);

            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }


            var pointOfInterestToPatch = AutoMapper.Mapper.Map<Models.PointOfInterestForUpdateDto>(pointOfInterestEntity);

            patchDoc.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (pointOfInterestToPatch.Description == pointOfInterestToPatch.Name)
            {
                ModelState.AddModelError("Description", "The provided description should be different from name.");
            }

            TryValidateModel(pointOfInterestToPatch);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AutoMapper.Mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);

            if (!_citiInfoRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }

        [HttpDelete("{cityid}/pointsofinterest/{id}")]
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {
            //var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            //if (city == null)
            //{
            //    return NotFound();
            //}

            //var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);

            //if (pointOfInterestFromStore == null)
            //{
            //    return NotFound();
            //}

            // city.PointsOfInterest.Remove(pointOfInterestFromStore);

            if (!_citiInfoRepository.CityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity = _citiInfoRepository.GetPointOfInterestForCity(cityId, id);

            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }


            _citiInfoRepository.DeletePointOfInterest(pointOfInterestEntity);

            if (!_citiInfoRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
           
            _mailService.Send("Point of interest deleted.",
                   $"Point of interest {pointOfInterestEntity.Name} with id {pointOfInterestEntity.Id} was deleted.");

            return NoContent();
        }
     }
}
