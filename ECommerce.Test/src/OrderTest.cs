using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace ECommerceTest;
public class OrderTest
{
   [Fact]
    public void CancelOrder_ValidId_CancelsOrderAndReturnsTrue()
    {
        var orderId = Guid.NewGuid(); 
        var mockRepo = new Mock<IOrderRepository>();
        var orderService = new OrderService(mockRepo.Object /* pass other dependencies */);

        var targetOrder = new Order { Id = orderId };
        mockRepo.Setup(repo => repo.GetById(orderId)).Returns(targetOrder);
        var result = orderService.CancelOrder(orderId);

        Assert.True(result);
        mockRepo.Verify(repo => repo.Cancel(orderId), Times.Once); 
    }

    [Fact]
    public void CancelOrder_EmptyId_ThrowsException()
    {
        var orderId = Guid.Empty; 

        var mockRepo = new Mock<IOrderRepository>();
        var orderService = new OrderService(mockRepo.Object /* pass other dependencies */);

        Assert.Throws<Exception>(() => orderService.CancelOrder(orderId));
        mockRepo.Verify(repo => repo.Cancel(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public void CancelOrder_NonexistentId_ReturnsFalse()
    {
        var orderId = Guid.NewGuid(); 

        var mockRepo = new Mock<IOrderRepository>();
        var orderService = new OrderService(mockRepo.Object /* pass other dependencies */);
        mockRepo.Setup(repo => repo.GetById(orderId)).Returns(() => null);
        var result = orderService.CancelOrder(orderId);
        Assert.False(result);
        mockRepo.Verify(repo => repo.Cancel(It.IsAny<Guid>()), Times.Never); 
    }

    [Fact]
    public void CreateOrder_ValidInput_ReturnsPurchaseReadDTO()
    {
        var userId = Guid.NewGuid();
        var purchaseItemCreateDTOs = new List<PurchaseItemCreateDTO>
        {
        
        };
        var newOrder = new PurchaseCreateDTO
        {
            UserId = userId,
            PurchaseItemCreateDTOs = purchaseItemCreateDTOs
        };

        var mockRepo = new Mock<IOrderRepository>();
        var mockUserRepo = new Mock<IUserRepo>();
        var mockProductRepo = new Mock<IProductRepo>();
        var mockMapper = new Mock<IMapper>();
        var orderService = new OrderService(mockRepo.Object, mockUserRepo.Object, mockProductRepo.Object, mockMapper.Object /* pass other dependencies */);

        var createdOrder = new Order { };
        var user = new User {  };
        var product = new Product { };

        mockMapper.Setup(mapper => mapper.Map<List<PurchaseItem>>(newOrder.PurchaseItemCreateDTOs)).Returns(new List<PurchaseItem>());
        mockRepo.Setup(repo => repo.CreateNew(userId, It.IsAny<List<PurchaseItem>>())).Returns(createdOrder);
        mockUserRepo.Setup(repo => repo.GetById(userId)).Returns(user);
        mockProductRepo.Setup(repo => repo.GetById(It.IsAny<Guid>())).Returns(product);

        var result = orderService.CreateOrder(newOrder);
        Assert.NotNull(result);
   
    }
    [Fact]
    public void CreateOrder_ForCurrentAuthenticatedUser_ReturnPurchaseReadDTO()
    {
        var userId = Guid.NewGuid(); 
        var purchaseItemCreateDTOs = new List<PurchaseItemCreateDTO>
        {
           
        };
        var newOrder = new PurchaseCreateDTO
        {
            PurchaseItemCreateDTOs = purchaseItemCreateDTOs
        };

        var mockRepo = new Mock<IOrderRepository>();
        var mockUserRepo = new Mock<IUserRepo>();
        var mockProductRepo = new Mock<IProductRepo>();
        var mockMapper = new Mock<IMapper>();
        var orderService = new OrderService(mockRepo.Object, mockUserRepo.Object, mockProductRepo.Object, mockMapper.Object /* pass other dependencies */);

        var user = new User { Id = userId  };
        var product = new Product { };

        var mockHttpContext = new Mock<HttpContext>();
        var mockIdentity = new Mock<IIdentity>();
        mockIdentity.SetupGet(i => i.Name).Returns(userId.ToString());
        var mockUser = new Mock<ClaimsPrincipal>();
        mockUser.SetupGet(u => u.Identity).Returns(mockIdentity.Object);
        mockHttpContext.SetupGet(c => c.User).Returns(mockUser.Object);
        var featureCollection = new FeatureCollection();
        featureCollection.Set<IHttpContextAccessor>(new HttpContextAccessor { HttpContext = mockHttpContext.Object });

        mockUserRepo.Setup(repo => repo.GetById(userId)).Returns(user);
        mockProductRepo.Setup(repo => repo.GetById(It.IsAny<Guid>())).Returns(product);
        var result = orderService.CreateOrder(newOrder);
    }

    [Fact]
    public void GetAllOrders_ReturnsAllPurchaseReadDTOs()
    {
        var mockRepo = new Mock<IOrderRepository>();
        var mockMapper = new Mock<IMapper>();
        var orderService = new OrderService(mockRepo.Object, mockMapper.Object /* pass other dependencies */);

        var purchases = new List<Purchase>(); 
        var purchaseDTOs = new List<PurchaseReadDTO>();

        mockRepo.Setup(repo => repo.GetAll(It.IsAny<BaseQueryParameter>())).Returns(purchases);
        mockMapper.Setup(mapper => mapper.Map<Purchase, PurchaseReadDTO>(It.IsAny<Purchase>()))
            .Returns((Purchase purchase) => purchaseDTOs.FirstOrDefault(dto => dto.Id == purchase.Id)); 

        var result = orderService.GetAllOrders(new BaseQueryParameter());
        Assert.NotNull(result);
        Assert.Equal(purchaseDTOs.Count, result.Count()); 

    }
    [Fact]
    public void GetOrderById_ValidId_ReturnsPurchaseReadDTO()
    {
    
        var orderId = Guid.NewGuid(); // valid id
        var targetOrder = new Purchase { Id = orderId };
        var mappedResult = new PurchaseReadDTO { Id = orderId, };

        var mockRepo = new Mock<IOrderRepository>();
        var mockMapper = new Mock<IMapper>();
        var orderService = new OrderService(mockRepo.Object, mockMapper.Object /* pass other dependencies */);

        mockRepo.Setup(repo => repo.GetById(orderId)).Returns(targetOrder);
        mockMapper.Setup(mapper => mapper.Map<PurchaseReadDTO>(targetOrder)).Returns(mappedResult);
        var result = orderService.GetOrderById(orderId);
        Assert.NotNull(result);
        Assert.Equal(mappedResult.Id, result.Id);
    }

    [Fact]
    public void GetOrderById_EmptyId_ThrowsException()
    {
        
        var mockRepo = new Mock<IOrderRepository>();
        var mockMapper = new Mock<IMapper>();
        var orderService = new OrderService(mockRepo.Object, mockMapper.Object /* pass other dependencies */);
        Assert.Throws<Exception>(() => orderService.GetOrderById(Guid.Empty));
    }

    [Fact]
    public void GetOrderById_OrderNotFound_ThrowsException()
    {
        var orderId = Guid.NewGuid(); // invalid id
        var mockRepo = new Mock<IOrderRepository>();
        var mockMapper = new Mock<IMapper>();
        var orderService = new OrderService(mockRepo.Object, mockMapper.Object /* pass other dependencies */);

        mockRepo.Setup(repo => repo.GetById(orderId)).Returns(() => null);
        Assert.Throws<Exception>(() => orderService.GetOrderById(orderId));
    }
    [Fact]
    public void UpdateOrderStatus_ValidIdAndUpdates_ReturnsUpdatedPurchaseReadDTO()
    {
        var orderId = Guid.NewGuid(); // valid id
        var updates = new PurchaseUpdateDTO { Status = "UpdatedStatus" }; /
        var targetOrder = new Purchase { Id = orderId, };
        var mappedResult = new PurchaseReadDTO { Id = orderId,  };

        var mockRepo = new Mock<IOrderRepository>();
        var mockMapper = new Mock<IMapper>();
        var orderService = new OrderService(mockRepo.Object, mockMapper.Object /* pass other dependencies */);

        mockRepo.Setup(repo => repo.GetById(orderId)).Returns(targetOrder);
        mockMapper.Setup(mapper => mapper.Map<PurchaseReadDTO>(targetOrder)).Returns(mappedResult);
        var result = orderService.UpdateOrderStatus(orderId, updates);

        Assert.NotNull(result);
        Assert.Equal(updates.Status, targetOrder.Status); 
    }

    [Fact]
    public void UpdateOrderStatus_EmptyId_ThrowsException()
    {
        var updates = new PurchaseUpdateDTO { Status = "UpdatedStatus" }; 
        var mockRepo = new Mock<IOrderRepository>();
        var mockMapper = new Mock<IMapper>();
        var orderService = new OrderService(mockRepo.Object, mockMapper.Object /* pass other dependencies */);
        Assert.Throws<Exception>(() => orderService.UpdateOrderStatus(Guid.Empty, updates));
    }

    [Fact]
    public void UpdateOrderStatus_OrderNotFound_ThrowsException()
    {
        var orderId = Guid.NewGuid();
        var updates = new PurchaseUpdateDTO { Status = "UpdatedStatus" }; 
        var mockRepo = new Mock<IOrderRepository>();
        var mockMapper = new Mock<IMapper>();
        var orderService = new OrderService(mockRepo.Object, mockMapper.Object /* pass other dependencies */);
        mockRepo.Setup(repo => repo.GetById(orderId)).Returns(() => null);
        Assert.Throws<Exception>(() => orderService.UpdateOrderStatus(orderId, updates));
    }

    [Fact]
    public void UpdateOrderStatus_NullUpdates_ThrowsException()
    {
        var orderId = Guid.NewGuid(); // valid id
        var mockRepo = new Mock<IOrderRepository>();
        var mockMapper = new Mock<IMapper>();
        var orderService = new OrderService(mockRepo.Object, mockMapper.Object /* pass other dependencies */);

        Assert.Throws<Exception>(() => orderService.UpdateOrderStatus(orderId, null));
    }
}
