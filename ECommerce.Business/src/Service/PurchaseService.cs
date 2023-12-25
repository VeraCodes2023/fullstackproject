using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using AutoMapper;

namespace ECommerce.Business;
public class PurchaseService : IPurchaseService
{
    private readonly IPurchaseRepo _repo;
    private IProductRepo _productRepo;
    private IUserRepo _userRepo;
    private IMapper _mapper;
    public PurchaseService(IPurchaseRepo repo, IMapper mapper,IProductRepo productRepo,IUserRepo userRepo)
    {
        _repo = repo;
        _mapper = mapper;
        _productRepo=productRepo;
        _userRepo = userRepo;

    }
    public bool CancelOrder(Guid id)
    {
        if(id == Guid.Empty)
        {
            throw new Exception("bad request");
        }
        try
        {
            var targetOrder = _repo.GetById(id);
            if(targetOrder is not null)
            {
                _repo.Cancel(id);
                return true;
            }
            return false;
        }
        catch(Exception)
        {
            throw;
        }
    }

    public PurchaseReadDTO CreateOrder(Guid userId, PurchaseCreateDTO newOrder)
    {
        var user = _userRepo.GetById(userId);
        List<PurchaseItem> orderItems = _mapper.Map<List<PurchaseItem>>(newOrder.PurchaseItemCreateDTOs);
        if (user == null)
        {
            throw new Exception("User not found");
        }
        else if (orderItems == null || !orderItems.Any())
        {
            throw new Exception("Order items are empty");
        }
        else
        {
         try
         {
            var createdOrder = _repo.CreateNew(userId, orderItems);
            var createdPurchaseDTO = new PurchaseReadDTO
            {
                PurchaseId = createdOrder.Id,
                UserId = createdOrder.UserId,
                Status = createdOrder.Status,
                PurchaseItemReadDTOs = new List<PurchaseItemReadDTO>(),
                User = new UserReadDTO()
            };
            createdPurchaseDTO.User.Name = user.Name;
            createdPurchaseDTO.User.Email = user.Email;
            createdPurchaseDTO.User.AddressReadDTOs = _mapper.Map<List<AddressReadDTO>>(user.Addresses);

            if (createdOrder != null && createdOrder.PurchaseItems != null)
            {
                foreach (var purchaseItem in createdOrder.PurchaseItems)
                {
                    var product = _productRepo.GetById(purchaseItem.ProductId);
                    if (product == null)
                    {
                        throw new Exception("Product not found");
                    }

                    if (purchaseItem.Quantity > product.Inventory)
                    {
                        throw new Exception("bad request due to insufficient inventory");
                    }
                    var purchaseItemDTO = _mapper.Map<PurchaseItemReadDTO>(purchaseItem);
                    var productDTO = _mapper.Map<ProductReadDTO>(product);
                    purchaseItemDTO.ProductName = productDTO.Title;
                    purchaseItemDTO.ProductPrice = productDTO.Price;
                    createdPurchaseDTO.PurchaseItemReadDTOs.Add(purchaseItemDTO);
                    
                }
                return createdPurchaseDTO;
            }
            else
            {
                
                throw new Exception("createdOrder is null, or PurchaseItems is null.");
            }
        }
        catch(Exception e)
        {
            throw new Exception("order failed to create" + e);
        }
    }
       
    }

    private void MapPurchaseItemsToDTOs(Purchase purchase, PurchaseReadDTO purchaseDTO)
    {
        if (purchase.PurchaseItems != null && purchase.PurchaseItems.Any())
        {
            purchaseDTO.PurchaseItemReadDTOs = _mapper.Map<List<PurchaseItemReadDTO>>(purchase.PurchaseItems);

            foreach (var purchaseItemDTO in purchaseDTO.PurchaseItemReadDTOs)
            {
                var product = _productRepo.GetById(purchaseItemDTO.ProductId);
                if (product != null)
                {
                    var productDTO = _mapper.Map<ProductReadDTO>(product);
                    purchaseItemDTO.ProductName = productDTO.Title;
                    purchaseItemDTO.ProductPrice = productDTO.Price;
                }
            }
        }
    }
    private PurchaseReadDTO MapPurchaseToDTO(Purchase purchase)
    {
        var purchaseDTO = _mapper.Map<PurchaseReadDTO>(purchase);
        var user = _userRepo.GetById(purchase.UserId);

        if (user != null)
        {
            purchaseDTO.User = _mapper.Map<UserReadDTO>(user);
        }

        MapPurchaseItemsToDTOs(purchase, purchaseDTO);

        return purchaseDTO;
    }
    public IEnumerable<PurchaseReadDTO> GetAllOrders(BaseQueryParameter options)
    {
         var purchases = _repo.GetAll(options).ToList();
         var purchaseDTOs = purchases.Select(purchase => MapPurchaseToDTO(purchase));

        return purchaseDTOs;
    }

    public PurchaseReadDTO GetOrderById(Guid id)
    {
         if(id == Guid.Empty)
        {
            throw new Exception("bad request");
        }
        try
        {
            var targetOrder = _repo.GetById(id);
            if(targetOrder is not null)
            {
                var mappedResult = _mapper.Map<PurchaseReadDTO>(targetOrder);
                var user = _userRepo.GetById(targetOrder.UserId);
                mappedResult.User = _mapper.Map<UserReadDTO>(user);
                mappedResult.PurchaseItemReadDTOs = _mapper.Map<List<PurchaseItemReadDTO>>(targetOrder.PurchaseItems);
                return mappedResult;
            }
            else
            {
                throw new Exception("not found");
            }
        }
        catch(Exception)
        {
            throw;
        }
    }

    public PurchaseReadDTO UpdateOrderStatus(Guid id, PurchaseUpdateDTO updates)
    {
        if (id == Guid.Empty || updates == null)
        {
           throw new Exception("bad request");
        }
        
        try
        {
            var targetOrder =_repo.GetById(id);
            if (targetOrder == null)
            {
               throw new Exception("not found");
            }
            targetOrder.Status = updates.Status;
            
            var user = _userRepo.GetById(targetOrder.UserId);
            var mappedResult = _mapper.Map<PurchaseReadDTO>(targetOrder);
            mappedResult.User = _mapper.Map<UserReadDTO>(user);
            _repo.Update(targetOrder.Id,targetOrder);
            mappedResult.PurchaseItemReadDTOs = _mapper.Map<List<PurchaseItemReadDTO>>(targetOrder.PurchaseItems);
            return mappedResult;
        }
        catch (Exception)
        {
            throw;
        }
    }
}

