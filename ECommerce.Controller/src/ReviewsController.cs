using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using ECommerceBusiness;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controller.src
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private IReviewService _service;
        public ReviewsController(IReviewService service)
        {
            _service = service;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost()]
        public ActionResult<ServiceResponse<ReviewReadDTO>> Create([FromBody] ReviewCreateDTO  review)
        {
            var serviceResponse = new ServiceResponse<ReviewReadDTO>();
            try
            {
                var createdReview = _service.Create(review);

                serviceResponse.Data = createdReview;
                serviceResponse.Success = true;
                serviceResponse.Message = "Review created successfully!";
                serviceResponse.StatusCode = 201; // 设置成功创建的状态码 201 Created
                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                serviceResponse.StatusCode = 500; 
                return StatusCode(serviceResponse.StatusCode, serviceResponse);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet()]
        public ActionResult<ServiceResponse<IEnumerable<ReviewReadDTO>>> GetAll([FromQuery] BaseQueryParameter options)
        {
            var serviceResponse = new ServiceResponse<IEnumerable<ReviewReadDTO>>();
            try
            {
                var reviews = _service.GetAll(options);

                serviceResponse.Data = reviews;
                serviceResponse.Success = true;
                serviceResponse.Message = "Reviews retrieved successfully!";
                serviceResponse.StatusCode = 200;
                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                serviceResponse.StatusCode = 500;
                return StatusCode(serviceResponse.StatusCode, serviceResponse);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public ActionResult<ServiceResponse<ReviewReadDTO>> GetById([FromRoute] Guid id)
        {
            var serviceResponse = new ServiceResponse<ReviewReadDTO>();
            try
            {
                var review = _service.GetById(id);

                if (review == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"Review with ID: {id} not found.";
                    serviceResponse.StatusCode = 404; 
                    return NotFound(serviceResponse);
                }

                serviceResponse.Data = review;
                serviceResponse.Success = true;
                serviceResponse.Message = "Review retrieved successfully!";
                serviceResponse.StatusCode = 200; 
                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                serviceResponse.StatusCode = 500;
                return StatusCode(serviceResponse.StatusCode, serviceResponse);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public ActionResult<ServiceResponse<ReviewReadDTO>>Update([FromRoute] Guid id, [FromBody] ReviewUpdateDTO reviewUpdateDto)
        {
            var serviceResponse = new ServiceResponse<ReviewReadDTO>();
            try
            {
                var updatedReview = _service.Update(id, reviewUpdateDto);

                if (updatedReview == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"Review with ID: {id} not found.";
                    serviceResponse.StatusCode = 404; 
                    return NotFound(serviceResponse);
                }

                serviceResponse.Data = updatedReview;
                serviceResponse.Success = true;
                serviceResponse.Message = "Review updated successfully!";
                serviceResponse.StatusCode = 200; 
                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                serviceResponse.StatusCode = 500; 
                return StatusCode(serviceResponse.StatusCode, serviceResponse);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public ActionResult<ServiceResponse<bool>> Cancel([FromRoute] Guid id)
        {
            var serviceResponse = new ServiceResponse<bool>();
            try
            {
                var isCancelled = _service.Cancel(id);

                if (!isCancelled)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"Failed to cancel review with ID: {id}.";
                    serviceResponse.StatusCode = 400; 
                    return BadRequest(serviceResponse);
                }
                serviceResponse.Data = true;
                serviceResponse.Success = true;
                serviceResponse.Message = $"Review with ID: {id} has been cancelled successfully!";
                serviceResponse.StatusCode = 200; 
                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                serviceResponse.StatusCode = 500;
                return StatusCode(serviceResponse.StatusCode, serviceResponse);
            }
        }
       
    }
}




