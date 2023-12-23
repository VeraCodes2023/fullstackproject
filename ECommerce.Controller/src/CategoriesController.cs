using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using ECommerceBusiness;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controller.src
{
    [EnableCors("AllowAny")]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private ICategoryService _service;
        public CategoriesController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet()]
        public ActionResult<ServiceResponse<IEnumerable<CategoryReadDTO>>> GetAll()
        {
            var serviceResponse = new ServiceResponse<IEnumerable<CategoryReadDTO>>();
            try
            {
                var categories = _service.GetAll();
                serviceResponse.Data = categories;
                serviceResponse.Success = true;
                serviceResponse.Message = "Categories retrieved successfully!";
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

        [HttpPost()]
        public ActionResult<ServiceResponse<CategoryReadDTO>> CreateNew([FromBody] CategoryCreateDTO category)
        {
           var serviceResponse = new ServiceResponse<CategoryReadDTO>();
           try
            {
                var createdCategory = _service.CreateNew(category);
                
                if (createdCategory != null)
                {
                    serviceResponse.Data = createdCategory;
                    serviceResponse.Success = true;
                    serviceResponse.Message = "Category created successfully!";
                    serviceResponse.StatusCode = 201; 
                    return StatusCode(serviceResponse.StatusCode, serviceResponse);
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Failed to create category.";
                    serviceResponse.StatusCode = 500; 
                    return StatusCode(serviceResponse.StatusCode, serviceResponse);
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                serviceResponse.StatusCode = 500; 
                return StatusCode(serviceResponse.StatusCode, serviceResponse);
            }
        } 

        [HttpGet("{id}")]
        public ActionResult<ServiceResponse<CategoryReadDTO>> GetById(Guid id)
        {
            var serviceResponse = new ServiceResponse<CategoryReadDTO>();
            try
            {
                var category = _service.GetById(id);
                if (category != null)
                {
                    serviceResponse.Data = category;
                    serviceResponse.Success = true;
                    serviceResponse.Message = "Category retrieved successfully!";
                    serviceResponse.StatusCode = 200; 
                    return Ok(serviceResponse);
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"Category with ID {id} not found.";
                    serviceResponse.StatusCode = 404;
                    return NotFound(serviceResponse);
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                serviceResponse.StatusCode = 500; 
                return StatusCode(serviceResponse.StatusCode, serviceResponse);
            }
        
        }


        [HttpPut("{id}")]
        public ActionResult<ServiceResponse<CategoryReadDTO>> Update(Guid id, [FromBody] CategoryUpdateDTO category)
        {
            var serviceResponse = new ServiceResponse<CategoryReadDTO>();
            try
            {
                var updatedCategory = _service.Update(id, category);
                if (updatedCategory != null)
                {
                    serviceResponse.Data = updatedCategory;
                    serviceResponse.Success = true;
                    serviceResponse.Message = "Category updated successfully!";
                    serviceResponse.StatusCode = 200; 
                    return Ok(serviceResponse);
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"Category with ID {id} not found.";
                    serviceResponse.StatusCode = 404; 
                    return NotFound(serviceResponse);
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                serviceResponse.StatusCode = 500; 
                return StatusCode(serviceResponse.StatusCode, serviceResponse);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<ServiceResponse<bool>> Delete(Guid id)
        {
            var serviceResponse = new ServiceResponse<bool>();
            try
            {
                bool isDeleted = _service.Delete(id);

                if (isDeleted)
                {
                    serviceResponse.Data = true;
                    serviceResponse.Success = true;
                    serviceResponse.Message = "Category deleted successfully!";
                    serviceResponse.StatusCode = 200; 
                    return Ok(serviceResponse);
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"Category with ID {id} not found.";
                    serviceResponse.StatusCode = 404; 
                    return NotFound(serviceResponse);
                }
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