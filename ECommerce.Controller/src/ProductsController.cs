using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Core;
using ECommerceBusiness;
using Microsoft.AspNetCore.Authorization;

namespace ECommerce.Controller.UserController
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductsController : ControllerBase
    {
        private  IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        
        [HttpGet()]
        public ActionResult<ServiceResponse<IEnumerable <ProductReadDTO>>> GetAllProducts([FromQuery] ProductQueryParameters options)
        {
            var serviceResponse = new ServiceResponse<IEnumerable<ProductReadDTO>>();
            try
            {
                var products = _productService.GetAllProducts(options);
                serviceResponse.Success = true;
                serviceResponse.Data = products;
                serviceResponse.Message = "All products retrieved successfully!";
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
        [HttpPost(),Authorize(Roles = "Admin")]
        public ActionResult<ServiceResponse<ProductReadDTO>> CreateProduct([FromBody] ProductCreateDTO productCreateDTO)
        {
            var serviceResponse = new ServiceResponse<ProductReadDTO>();
            try
            {
                var createdProduct = _productService.CreateProduct(productCreateDTO);
                serviceResponse.Data = createdProduct;
                serviceResponse.Success = true;
                serviceResponse.Message = "Product created successfully!";
                serviceResponse.StatusCode = 201; 
                return StatusCode(serviceResponse.StatusCode, serviceResponse);
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
        public ActionResult<ServiceResponse<ProductReadDTO>> GetProductById(Guid id)
        {
            var serviceResponse = new ServiceResponse<ProductReadDTO>();
            try
            {
                var product = _productService.GetProductById(id);
                if (product != null)
                {
                    serviceResponse.Data = product;
                    serviceResponse.Success = true;
                    serviceResponse.Message = "Product retrieved successfully!";
                    serviceResponse.StatusCode = 200; 
                    return Ok(serviceResponse);
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"Product with ID {id} not found.";
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
        [HttpDelete("{id}"),Authorize(Roles = "Admin")]
        public ActionResult<ServiceResponse<bool>> DeleteProduct(Guid id)
        {
            var serviceResponse = new ServiceResponse<bool>();
            try
            {
                var isDeleted = _productService.DeleteProduct(id);
                if (isDeleted)
                {
                    serviceResponse.Data = true;
                    serviceResponse.Success = true;
                    serviceResponse.Message = "Product deleted successfully!";
                    serviceResponse.StatusCode = 200; 
                    return Ok(serviceResponse);
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"Product with ID {id} not found.";
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
        [HttpPut("{id}"),Authorize(Roles = "Admin")]
        public ActionResult<ServiceResponse<ProductReadDTO>> UpdateProduct(Guid id, [FromBody] ProductUpdateDTO product)
        {
            var serviceResponse = new ServiceResponse<ProductReadDTO>();
            try
            {
                var updatedProduct = _productService.UpdateProduct(id, product);
                if (updatedProduct != null)
                {
                    serviceResponse.Data = updatedProduct;
                    serviceResponse.Success = true;
                    serviceResponse.Message = "Product updated successfully!";
                    serviceResponse.StatusCode = 200;
                    return Ok(serviceResponse);
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"Product with ID {id} not found.";
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