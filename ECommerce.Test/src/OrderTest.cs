using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Core;
using Moq;
using AutoMapper;
using ECommerce.Business;


namespace ECommerceTest;
public class OrderTest
{
    private static IMapper _mapper;
    public OrderTest()
    {
        if (_mapper == null)
        {
            var mappingConfig = new MapperConfiguration(m =>
            {
                m.AddProfile(new MapperProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            _mapper = mapper;
        }
    }
    
    [Fact]
    public void CancelOrder_ValidId_CancelsOrderAndReturnsTrue()
    {
        var mockOrderRepo = new Mock<IPurchaseRepo>();
        var mockMapper = new Mock<IMapper>();
        var mockUserRepo = new Mock<IUserRepo>();
        var mockProductRepo = new Mock<IProductRepo>();
        var orderService = new PurchaseService(mockOrderRepo.Object,mockMapper.Object,mockProductRepo.Object, mockUserRepo.Object);
        var orderId = Guid.NewGuid(); 
        var targetOrder = new Purchase { Id = orderId };

        mockOrderRepo.Setup(repo => repo.GetById(orderId)).Returns(targetOrder);
        var result = orderService.CancelOrder(orderId);
        Assert.True(result);
        mockOrderRepo.Verify(repo => repo.Cancel(orderId), Times.Once); 
    }

    [Fact]
    public void CancelOrder_EmptyId_ThrowsException()
    {
        var orderId = Guid.Empty; 

        var mockOrderRepo = new Mock<IPurchaseRepo>();
        var mockMapper = new Mock<IMapper>();
        var mockUserRepo = new Mock<IUserRepo>();
        var mockProductRepo = new Mock<IProductRepo>();
        var orderService = new PurchaseService(mockOrderRepo.Object,mockMapper.Object,mockProductRepo.Object, mockUserRepo.Object);

        Assert.Throws<Exception>(() => orderService.CancelOrder(orderId));
        mockOrderRepo.Verify(repo => repo.Cancel(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public void CancelOrder_NonexistentId_ReturnsFalse()
    {
        var orderId = Guid.NewGuid(); 

        var mockOrderRepo = new Mock<IPurchaseRepo>();
        var mockMapper = new Mock<IMapper>();
        var mockUserRepo = new Mock<IUserRepo>();
        var mockProductRepo = new Mock<IProductRepo>();
        var orderService = new PurchaseService(mockOrderRepo.Object,mockMapper.Object,mockProductRepo.Object, mockUserRepo.Object);

        mockOrderRepo.Setup(repo => repo.GetById(orderId)).Returns(() => null);
        var result = orderService.CancelOrder(orderId);
        Assert.False(result);
        mockOrderRepo.Verify(repo => repo.Cancel(It.IsAny<Guid>()), Times.Never); 
    }

    [Fact]
    public void CreateOrder_ValidInput_ReturnsPurchaseReadDTO()
    {
        var mockOrderRepo = new Mock<IPurchaseRepo>();
        var mockMapper = new Mock<IMapper>();
        var mockUserRepo = new Mock<IUserRepo>();
        var mockProductRepo = new Mock<IProductRepo>();
        var orderService = new PurchaseService(mockOrderRepo.Object, mockMapper.Object, mockProductRepo.Object, mockUserRepo.Object);

        var existingUser = new User 
        { 
            Id = Guid.NewGuid(), 
            Name = "ExistingUser", 
            Email = "existinguser@example.com",
            Password="password123",
            Avatar="avatar.png", Role=Role.Customer
        };

        mockUserRepo.Setup(repo => repo.GetById(existingUser.Id)).Returns(existingUser);
        // Arrange
        var purchaseItems = new List<PurchaseItem>
        {
            new PurchaseItem { ProductId = Guid.NewGuid(), Quantity = 1 },
            new PurchaseItem { ProductId = Guid.NewGuid(), Quantity = 2 }
        };
        var purchaseItemCreateDTOs = new List<PurchaseItemCreateDTO>
        {
            new PurchaseItemCreateDTO { ProductId = Guid.NewGuid(), Quantity = 1 },
            new PurchaseItemCreateDTO { ProductId = Guid.NewGuid(), Quantity = 2 }
        };
        var newOrder = new PurchaseCreateDTO { PurchaseItems = purchaseItemCreateDTOs };

        mockMapper.Setup(mapper => mapper.Map<List<PurchaseItem>>(It.IsAny<List<PurchaseItemCreateDTO>>()))
            .Returns((List<PurchaseItemCreateDTO> input) => purchaseItems);
        mockOrderRepo.Setup(repo => repo.CreateNew(existingUser.Id, It.IsAny<List<PurchaseItem>>()))
            .Returns((Guid uid, List<PurchaseItem> items) =>
            {
                var purchase = new Purchase();
                purchase.PurchaseItems = new List<PurchaseItem>();
                return purchase;
            });

            var result = orderService.CreateOrder(existingUser.Id, newOrder);
            Assert.IsType<PurchaseReadDTO>(result);
            Assert.NotNull(result);
    }
    [Fact]
    public void GetAllOrders_ReturnsAllPurchaseReadDTOs()
    {
        var mockOrderRepo = new Mock<IPurchaseRepo>();
        var mockMapper = new Mock<IMapper>();
        var mockUserRepo = new Mock<IUserRepo>();
        var mockProductRepo = new Mock<IProductRepo>();
        var orderService = new PurchaseService(mockOrderRepo.Object, mockMapper.Object, mockProductRepo.Object, mockUserRepo.Object);

        var purchases = new List<Purchase>(); 
        var purchaseDTOs = new List<PurchaseReadDTO>();

        mockOrderRepo.Setup(repo => repo.GetAll(It.IsAny<BaseQueryParameter>())).Returns(purchases);
        mockMapper.Setup(mapper => mapper.Map<Purchase, PurchaseReadDTO>(It.IsAny<Purchase>()))
            .Returns((Purchase purchase) => purchaseDTOs.FirstOrDefault(dto => dto.PurchaseId == purchase.Id)); 

        var result = orderService.GetAllOrders(new BaseQueryParameter());
        Assert.NotNull(result);
        Assert.Equal(purchaseDTOs.Count, result.Count()); 
    }

    [Fact]
    public void GetOrderById_ValidId_ReturnsPurchaseReadDTO()
    {
        var orderId = Guid.NewGuid(); // valid id
        var targetOrder = new Purchase { Id = orderId };
        var mappedResult = new PurchaseReadDTO { PurchaseId = orderId }; 

        var mockOrderRepo = new Mock<IPurchaseRepo>();
        var mockMapper = new Mock<IMapper>();
        var mockUserRepo = new Mock<IUserRepo>();
        var mockProductRepo = new Mock<IProductRepo>();
        var orderService = new PurchaseService(mockOrderRepo.Object, mockMapper.Object, mockProductRepo.Object, mockUserRepo.Object);

        mockOrderRepo.Setup(repo => repo.GetById(orderId)).Returns(targetOrder);
        mockMapper.Setup(mapper => mapper.Map<PurchaseReadDTO>(targetOrder)).Returns(mappedResult);

        var result = orderService.GetOrderById(orderId);
        Assert.NotNull(result);
        Assert.Equal(mappedResult.PurchaseId, result.PurchaseId); 
    }

    // [Fact]
    // public void UpdateOrderStatus_ValidIdAndUpdates_ReturnsUpdatedPurchaseReadDTO()
    // {
    //     var orderId = Guid.NewGuid(); // valid id
    //     var updates = new PurchaseUpdateDTO { Status = Status.Processing }; 
    //     var targetOrder = new Purchase { Status = Status.Pending }; 

    //     var mockOrderRepo = new Mock<IPurchaseRepo>();
    //     var mockMapper = new Mock<IMapper>();
    //     var mockUserRepo = new Mock<IUserRepo>();
    //     var mockProductRepo = new Mock<IProductRepo>();
    //     var orderService = new PurchaseService(mockOrderRepo.Object, mockMapper.Object, mockProductRepo.Object, mockUserRepo.Object);

    //     mockOrderRepo.Setup(repo => repo.GetById(orderId)).Returns(targetOrder);
    //     mockMapper.Setup(mapper => mapper.Map<PurchaseReadDTO>(It.IsAny<Purchase>())).Returns(new PurchaseReadDTO { Status = Status.Processing }); 

    //     var result = orderService.UpdateOrderStatus(orderId, updates);

    //     Assert.NotNull(result);
    //     Assert.Equal(updates.Status, result.Status); 
    // }

[Fact]
public void UpdateOrderStatus_ValidIdAndUpdates_ReturnsUpdatedPurchaseReadDTO()
{
    var orderId = Guid.NewGuid(); // valid id
    var updates = new PurchaseUpdateDTO { Status = Status.Processing };
    var targetOrder = new Purchase { Status = Status.Pending, UserId = Guid.NewGuid() }; // Provide a UserId

    var mockOrderRepo = new Mock<IPurchaseRepo>();
    var mockMapper = new Mock<IMapper>();
    var mockUserRepo = new Mock<IUserRepo>();
    var mockProductRepo = new Mock<IProductRepo>();
    var orderService = new PurchaseService(mockOrderRepo.Object, mockMapper.Object, mockProductRepo.Object, mockUserRepo.Object);

    mockOrderRepo.Setup(repo => repo.GetById(orderId)).Returns(targetOrder);
    mockMapper.Setup(mapper => mapper.Map<PurchaseReadDTO>(It.IsAny<Purchase>())).Returns(new PurchaseReadDTO { Status = Status.Processing });

    // Mock the user retrieval when calling GetById on UserRepo
    mockUserRepo.Setup(repo => repo.GetById(It.IsAny<Guid>())).Returns(new User()); // Provide a mock User instance

    var result = orderService.UpdateOrderStatus(orderId, updates);

    Assert.NotNull(result);
    Assert.Equal(updates.Status, result.Status);
}

}
