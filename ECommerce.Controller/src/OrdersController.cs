using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Core;
using ECommerceBusiness;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controller.OrderController
{
    
    [ApiController]
    [Route("api/v1/[controller]")]
    [EnableCors("AllowAny")]
    public class OrdersController : ControllerBase
    {
        private IPurchaseService _service;
        public OrdersController(IPurchaseService service)
        {
            _service = service;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet()]
        public ActionResult<ServiceResponse<IEnumerable<PurchaseReadDTO>>> GetAllOrders([FromQuery] BaseQueryParameter options)
        {
            var serviceResponse = new ServiceResponse<IEnumerable<PurchaseReadDTO>>();
            try
            {
                var orders = _service.GetAllOrders(options);

                if (orders != null && orders.Any())
                {
                    serviceResponse.Data = orders;
                    serviceResponse.Success = true;
                    serviceResponse.Message = "Orders retrieved successfully!";
                    serviceResponse.StatusCode = 200; 
                    return Ok(serviceResponse);
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No orders found.";
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

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public ActionResult<ServiceResponse<PurchaseReadDTO>> GetOrderById([FromRoute] Guid id)
        {
            var serviceResponse = new ServiceResponse<PurchaseReadDTO>();
            try
            {
                var order = _service.GetOrderById(id);

                if (order != null)
                {
                    serviceResponse.Data = order;
                    serviceResponse.Success = true;
                    serviceResponse.Message = "Order retrieved successfully!";
                    serviceResponse.StatusCode = 200; 
                    return Ok(serviceResponse);
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Order not found.";
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
  

        [Authorize(Roles = "Customer")]
        [HttpPost()]
        public ActionResult<PurchaseReadDTO> CreateOrder(PurchaseCreateDTO  newOrder)
        {
            var serviceResponse = new ServiceResponse<PurchaseReadDTO>();
            try
            {
                var userId = GetUserIdClaim();
                var createdOrder = _service.CreateOrder(userId,newOrder);
                serviceResponse.Data = createdOrder;
                serviceResponse.Success = true;
                serviceResponse.Message = "Order created successfully!";
                serviceResponse.StatusCode = 201; 
                return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.PurchaseId }, serviceResponse);
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
        public ActionResult<PurchaseReadDTO> UpdateOrderStatus([FromRoute] Guid id, [FromBody] PurchaseUpdateDTO update)
        {
            var serviceResponse = new ServiceResponse<PurchaseReadDTO>();
            try
            {
                var updatedOrder = _service.UpdateOrderStatus(id, update);
                serviceResponse.Data = updatedOrder;
                serviceResponse.Success = true;
                serviceResponse.Message = "Order status updated successfully!";
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
        public ActionResult<bool> CancelOrder([FromRoute] Guid id)
        {
            var serviceResponse = new ServiceResponse<bool>();
            try
            {
                var isCancelled = _service.CancelOrder(id);
                serviceResponse.Data = isCancelled;
                serviceResponse.Success = true;
                serviceResponse.Message = isCancelled ? "Order cancelled successfully!" : "Order not found or couldn't be cancelled.";
                serviceResponse.StatusCode = isCancelled ? 200 : 404; 
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
        private Guid GetUserIdClaim()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                throw new Exception("User ID claim not found");
            }
            if (!Guid.TryParse(userIdClaim.Value, out var userId))
            {
                throw new Exception("Invalid user ID format");       
            }
            return userId;
        }

    }
}


    

   
