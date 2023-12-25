using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core;

namespace ECommerce.Business;
public class UserService : IUserService
{   private readonly IPasswordHashRepo _passwordRepo;  
    private readonly IUserRepo _userRepo;
    private IMapper _mapper;

    public UserService(IUserRepo userRepo,IMapper mapper,IPasswordHashRepo passwordRepo)
    {
         _userRepo=userRepo;
         _passwordRepo = passwordRepo;
         _mapper = mapper;
    }

    public UserReadDTO Register(UserCreateDTO userCreateDto)
    {
     
        if (userCreateDto == null)
        {
            throw new Exception("bad request");
        }
        if(_userRepo.CheckEmailExist(userCreateDto.Email))
        {
            throw new Exception("User already existed, please login.");
        }

        try
        {
            var passwordHash = _passwordRepo.HashPassword(userCreateDto.Password);
            var newUser = _mapper.Map<UserCreateDTO, User>(userCreateDto);
            newUser.Password = passwordHash;
            
            if (userCreateDto.AddressCreateDTOs != null && userCreateDto.AddressCreateDTOs.Any())
            {
                foreach (var addressDto in userCreateDto.AddressCreateDTOs)
                {
                    var address = _mapper.Map<AddressCreateDTO, Address>(addressDto);
                    if (address != null)
                    {
                        address.IsDefault = true;
                         newUser.Addresses.Add(address);
                    }
                   
                }

                var result = _userRepo.Register(newUser);
            
              return _mapper.Map<User, UserReadDTO>(result);

            }
            else
            {
                return null;
            }
    
        }
        catch(Exception )
        {
            throw;
        }
    }
  
    public IEnumerable<UserReadDTO> GetAllUsers(UserQueryParameters options)
    {
        try
        {
            var users = _userRepo.GetAll(options);
            if (users == null || !users.Any())
            {
                return Enumerable.Empty<UserReadDTO>(); 
            }
            var userDTOs = users.Select(u => _mapper.Map<User, UserReadDTO>(u));
            if (!string.IsNullOrEmpty(options.Search))
            {
                userDTOs = userDTOs.Where(u => u.Name.Contains(options.Search));
            }
            userDTOs = userDTOs.Skip(options.Offset).Take(options.Limit);
            return userDTOs;
        }
        catch(Exception)
        {
        throw new Exception("Failed to retrieve users");
        }
     }
    

    public UserReadDTO GetUserById(Guid userId)
    {
        if(userId == Guid.Empty)
        {
            throw new Exception("bad request");
        }
        try
        {
            var targetUser=_userRepo.GetById(userId);
            if(targetUser is not null)
            {
                var mappedResult = _mapper.Map<UserReadDTO>(targetUser);
                return mappedResult;
            }
            throw new Exception("not found");
        }
        catch(Exception)
        {
           throw;
        }
       
    }
    public UserReadDTO GetUserByEmail(string email)
    {
        if(email == string.Empty)
        {
            throw new Exception("bad request");
        }
        try
        {
            var targetUser=_userRepo.GetByEmail(email);
            if(targetUser is not null)
            {
                var mappedResult = _mapper.Map<UserReadDTO>(targetUser);
                return mappedResult;
            }
            throw new Exception("not found");
        }
        catch(Exception)
        {
            throw;
        }
        
    }
    public UserReadDTO GetUserProfile(Guid id)
    {
        if(id == Guid.Empty)
        {
           throw new Exception("bad request");
        }
        try
        {
            var targetUser=_userRepo.GetById(id);
            if(targetUser is not null)
            {
                var mappedResult = _mapper.Map<UserReadDTO>(targetUser);
                return mappedResult;
            }
            throw new Exception("not found");
        }
        catch(Exception)
        {
            throw;
        }
    }
    public UserReadDTO UpdateUser(Guid userId,UserUpdateDTO userUpdateDto)
    {
        if (userId == Guid.Empty || userUpdateDto == null )
        {
             throw new Exception("bad request");
        }
        else if(_userRepo.GetById(userId) is null)
        {
            throw new Exception("not found");
        }
        try
        {
            var targetUser=_userRepo.GetById(userId);
            targetUser!.Name =userUpdateDto.Name;
            targetUser!.Avatar =userUpdateDto.Avatar;
            List<AddressReadDTO> updatedAddresses = new List<AddressReadDTO>(); 
            if (userUpdateDto.AddressUpdateDTOs != null && userUpdateDto.AddressUpdateDTOs.Any())
            {
                foreach (var addressUpdateDTO in userUpdateDto.AddressUpdateDTOs)
                {
                    if (targetUser.Addresses != null)
                    {
                        targetUser.Addresses.Clear();
                        var address = new Address
                        {
                            Street = addressUpdateDTO.Street,
                            City = addressUpdateDTO.City,
                            State = addressUpdateDTO.State,
                            PostalCode = addressUpdateDTO.PostalCode,
                            Country = addressUpdateDTO.Country,
                            IsDefault = addressUpdateDTO.IsDefault
                        };
                        targetUser.Addresses.Add(address);
                    }
                }
                _userRepo.Update(userId,targetUser);
                return _mapper.Map<UserReadDTO>(targetUser);
            }
            else
            {
                return null;
            }

           
        }
        catch (Exception)
        {
           throw;
        }
    }

    public UserReadDTO UpdateUserProfile(Guid userId,UserUpdateDTO updatedProfile)
    {
        if (userId == Guid.Empty || updatedProfile == null)
        {
            throw new Exception("bad request");
        }
        else if(_userRepo.GetById(userId) is null)
        {
            throw new Exception("not found");
        }
        try
        {
            var targetUser =_userRepo.GetById(userId);
            targetUser!.Name =  updatedProfile.Name;
            targetUser!.Avatar =  updatedProfile.Avatar;
            List<AddressReadDTO> updatedAddresses = new List<AddressReadDTO>(); 

          if (updatedProfile.AddressUpdateDTOs != null && updatedProfile.AddressUpdateDTOs.Any())
          {
            if(targetUser.Addresses !=null)
            {
            targetUser.Addresses.Clear();
            foreach (var addressUpdateDTO in updatedProfile.AddressUpdateDTOs)
            {
                var address = new Address
                {
                    Street = addressUpdateDTO.Street,
                    City = addressUpdateDTO.City,
                    State = addressUpdateDTO.State,
                    PostalCode = addressUpdateDTO.PostalCode,
                    Country = addressUpdateDTO.Country,
                    IsDefault = addressUpdateDTO.IsDefault
                };
                targetUser.Addresses.Add(address);
            }}
            _userRepo.Update(userId,targetUser);
            return _mapper.Map<UserReadDTO>(targetUser);
          }
          else
          {
            return null;
          }
        }
        catch (Exception)
        {
            throw;
        }
    }
    public bool Unregister(Guid userId)
    {
        if(userId == Guid.Empty)
        {
           throw new Exception("bad request");
        }
        var targetUser = _userRepo.GetById(userId);
        if (targetUser == null)
        {
            throw new Exception("not found");
        }
        return _userRepo.UnregisterUser(userId);
    }
    public bool DeleteUser(Guid id)
    {
        if(id == Guid.Empty)
        {
             throw new Exception("bad request");
        }
        var targetUser = _userRepo.GetById(id);
        if(targetUser is not null)
        {
            _userRepo.Delete(id);
            return true;
        }
        return false;
    }
    public string Login(Credential credential)
    {
         var user = _userRepo.GetByEmail(credential.Email);
        if (user != null && _passwordRepo.VerifyPassword(user.Password, credential.Password))
        {
            return _userRepo.GenerateToken(user);
        }
        return null;
    }

}
