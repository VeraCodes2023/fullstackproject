using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using AutoMapper;
using ECommerce.Business;
using  Core;

public class UserTest
{
    private  IMapper _mapper;
    public UserTest()
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
    public void Register_ValidUser_ReturnsUserReadDTO()
    {
    var mockUserRepo = new Mock<IUserRepo>();
    var mockMapper = new Mock<IMapper>();
    var mockPasswordRepo = new Mock<IPasswordHashRepo>();

    var userService = new UserService(mockUserRepo.Object, mockMapper.Object, mockPasswordRepo.Object);

    var userCreateDto = new UserCreateDTO
    {
        Name = "TestName",
        Email = "maria@mail.com",
        Password = "maria123",
        Avatar = "maria.png",
        Role = Role.Admin,
        AddressCreateDTOs = new List<AddressCreateDTO> 
        {
            new AddressCreateDTO  
            {
                Street = "Mannerheimintie",
                City = "Helsinki",
                State = "Etelä-Karjala",
                PostalCode = "0105322",
                Country = "Suomi"
            }
   
        }
    };

        var createdUser = new User();
        var userReadDto = new UserReadDTO();
        mockMapper.Setup(mapper => mapper.Map<UserCreateDTO, User>(It.IsAny<UserCreateDTO>()))
            .Returns((UserCreateDTO inputDto) => createdUser);

        mockMapper.Setup(mapper => mapper.Map<User, UserReadDTO>(createdUser)).Returns(userReadDto);
        mockUserRepo.Setup(repo => repo.Register(createdUser)).Returns(createdUser);
        var result = userService.Register(userCreateDto);
        Assert.NotNull(result);
        Assert.Equal(userReadDto, result);
    }

    [Fact]
    public void Register_NullUserCreateDto_ThrowsException()
    {
        var mockUserRepo = new Mock<IUserRepo>();
        var mockMapper = new Mock<IMapper>();
        var mockPasswordRepo = new Mock<IPasswordHashRepo>();
        var userService = new UserService(mockUserRepo.Object, mockMapper.Object, mockPasswordRepo.Object);
        UserCreateDTO userCreateDto = null;
        Assert.Throws<Exception>(() => userService.Register(userCreateDto));
    }

    [Fact]
    public void Register_ExistingEmail_ThrowsException()
    {
        var mockUserRepo = new Mock<IUserRepo>();
        var mockMapper = new Mock<IMapper>();
        var mockPasswordRepo = new Mock<IPasswordHashRepo>();
        var userService = new UserService(mockUserRepo.Object, mockMapper.Object, mockPasswordRepo.Object);
        var userCreateDto = new UserCreateDTO
        {
            Email = "existing@example.com" 
        };
        mockUserRepo.Setup(repo => repo.CheckEmailExist(userCreateDto.Email)).Returns(true);
        Assert.Throws<Exception>(() => userService.Register(userCreateDto));
    }

    [Fact]
    public void GetAllUsers_ReturnsMappedUserReadDTOs()
    {
      
        var mockUserRepo = new Mock<IUserRepo>();
        var mockMapper = new Mock<IMapper>();
        var mockPasswordRepo = new Mock<IPasswordHashRepo>();
        var userService = new UserService(mockUserRepo.Object, mockMapper.Object, mockPasswordRepo.Object);

        var users = new List<User>
        {
            new User { Id = Guid.NewGuid(), Name = "User1" },
            new User { Id = Guid.NewGuid(), Name = "User2" }
        };

        var userQueryParameters = new UserQueryParameters(); 
        mockUserRepo.Setup(repo => repo.GetAll(userQueryParameters)).Returns(users);
        mockMapper.Setup(mapper => mapper.Map<User, UserReadDTO>(It.IsAny<User>()))
            .Returns((User u) => new UserReadDTO {Name = u.Name });

        var result = userService.GetAllUsers(userQueryParameters);
        Assert.NotNull(result);
        Assert.Equal(users.Count, result.Count()); 

        foreach (var userReadDto in result)
        {
            Assert.NotNull(userReadDto);
            Assert.Contains(users, u => u.Name == userReadDto.Name);
        }
    }

    [Fact]
    public void GetUserById_ValidUserId_ReturnsUserReadDTO()
    {
        var mockUserRepo = new Mock<IUserRepo>();
        var mockMapper = new Mock<IMapper>();
        var mockPasswordRepo = new Mock<IPasswordHashRepo>();
        var userService = new UserService(mockUserRepo.Object, mockMapper.Object, mockPasswordRepo.Object);

        var userId = Guid.NewGuid(); // valid user ID
        var targetUser = new User { Id = userId, Name = "TestUser" }; 

        mockUserRepo.Setup(repo => repo.GetById(userId)).Returns(targetUser);
        mockMapper.Setup(mapper => mapper.Map<UserReadDTO>(targetUser))
            .Returns(new UserReadDTO { Name = targetUser.Name });
        var result = userService.GetUserById(userId);

        Assert.NotNull(result);
        Assert.Equal(targetUser.Name, result.Name);
    }


    [Fact]
    public void GetUserById_EmptyUserId_ThrowsException()
    {
        var userId = Guid.Empty;

        var mockUserRepo = new Mock<IUserRepo>();
        var mockMapper = new Mock<IMapper>();
        var mockPasswordRepo = new Mock<IPasswordHashRepo>();
        var userService = new UserService(mockUserRepo.Object, mockMapper.Object, mockPasswordRepo.Object);

        Assert.Throws<Exception>(() => userService.GetUserById(userId));
    }

    [Fact]
    public void GetUserById_NonexistentUserId_ThrowsException()
    {
        var userId = Guid.NewGuid(); 

        var mockUserRepo = new Mock<IUserRepo>();
        var mockMapper = new Mock<IMapper>();
        var mockPasswordRepo = new Mock<IPasswordHashRepo>();
        var userService = new UserService(mockUserRepo.Object, mockMapper.Object, mockPasswordRepo.Object);

        mockUserRepo.Setup(repo => repo.GetById(userId)).Returns(() => null);
        Assert.Throws<Exception>(() => userService.GetUserById(userId));
    }

    [Fact]
    public void GetUserByEmail_ValidEmail_ReturnsUserReadDTO()
    {
        var email = "test@example.com"; 

        var mockUserRepo = new Mock<IUserRepo>();
        var mockMapper = new Mock<IMapper>();
        var mockPasswordRepo = new Mock<IPasswordHashRepo>();
        var userService = new UserService(mockUserRepo.Object, mockMapper.Object, mockPasswordRepo.Object);

        var targetUser = new User { Email = email, Name = "TestUser" }; 

        mockUserRepo.Setup(repo => repo.GetByEmail(email)).Returns(targetUser);
        mockMapper.Setup(mapper => mapper.Map<UserReadDTO>(targetUser))
            .Returns(new UserReadDTO { Email = targetUser.Email, Name = targetUser.Name });

        var result = userService.GetUserByEmail(email);

        Assert.NotNull(result);
        Assert.Equal(targetUser.Email, result.Email);
        Assert.Equal(targetUser.Name, result.Name);
    }

    [Fact]
    public void GetUserByEmail_EmptyEmail_ThrowsException()
    {
        var email = string.Empty;

        var mockUserRepo = new Mock<IUserRepo>();
        var mockMapper = new Mock<IMapper>();
        var mockPasswordRepo = new Mock<IPasswordHashRepo>();
        var userService = new UserService(mockUserRepo.Object, mockMapper.Object, mockPasswordRepo.Object);

        Assert.Throws<Exception>(() => userService.GetUserByEmail(email));
    }

    [Fact]
    public void GetUserByEmail_NonexistentEmail_ThrowsException()
    {
   
        var email = "nonexistent@example.com"; 

        var mockUserRepo = new Mock<IUserRepo>();
        var mockMapper = new Mock<IMapper>();
        var mockPasswordRepo = new Mock<IPasswordHashRepo>();
        var userService = new UserService(mockUserRepo.Object, mockMapper.Object, mockPasswordRepo.Object);

        mockUserRepo.Setup(repo => repo.GetByEmail(email)).Returns(() => null);
        Assert.Throws<Exception>(() => userService.GetUserByEmail(email));
    }

    // [Fact]
    // public void GetUserProfile_ValidUserId_ReturnsUserReadDTO()
    // {
    //     string strGuid = "9f19b049-94fc-417e-b7e0-34fff19adf1e";
    //     Guid userId = Guid.Parse(strGuid);

    //     var mockUserRepo = new Mock<IUserRepo>();
    //     var mockMapper = new Mock<IMapper>();
    //     var mockPasswordRepo = new Mock<IPasswordHashRepo>();
    //     var userService = new UserService(mockUserRepo.Object, mockMapper.Object, mockPasswordRepo.Object);
    //     var address = new Address
    //     {
    //         Street = "123 Main St",
    //         City = "Example City",
    //         State = "Example State",
    //         PostalCode = "12345",
    //         Country = "Example Country"
    //     };
    //    List<Address> addressList = new List<Address>
    //     {
    //         address
    //     };
    //     mockUserRepo.Setup(repo => repo.GetById(userId)).Returns(new User {
    //         Name = "TestUser",
    //         Email ="test@mail.com",
    //         Avatar="avata.png", 
    //         Addresses = addressList
    //     });
    //     var result = userService.GetUserProfile(userId);
    //     Assert.NotNull(result);
    // }

    [Fact]
    public void GetUserProfile_EmptyUserId_ThrowsException()
    {
        var userId = Guid.Empty; 

        var mockUserRepo = new Mock<IUserRepo>();
        var mockMapper = new Mock<IMapper>();
        var mockPasswordRepo = new Mock<IPasswordHashRepo>();
        var userService = new UserService(mockUserRepo.Object, mockMapper.Object, mockPasswordRepo.Object);

        Assert.Throws<Exception>(() => userService.GetUserProfile(userId));
    }

    [Fact]
    public void GetUserProfile_NonexistentUserId_ThrowsException()
    {
    
        var userId = Guid.NewGuid(); // invalid id 

        var mockUserRepo = new Mock<IUserRepo>();
        var mockMapper = new Mock<IMapper>();
        var mockPasswordRepo = new Mock<IPasswordHashRepo>();
        var userService = new UserService(mockUserRepo.Object, mockMapper.Object, mockPasswordRepo.Object);

        mockUserRepo.Setup(repo => repo.GetById(userId)).Returns(() => null);
        Assert.Throws<Exception>(() => userService.GetUserProfile(userId));
    }


    [Fact]
    public void UpdateUser_ValidUserIdAndUserUpdateDto_ReturnsUpdatedUserReadDTO()
    {

        var userId = Guid.NewGuid(); // valid id
        var userUpdateDto = new UserUpdateDTO
        {
            Name = "UpdatedName",
            Avatar = "UpdatedAvatar",
            AddressUpdateDTOs = new List<AddressUpdateDTO>
            {
                new AddressUpdateDTO
                {
                    Street = "UpdatedStreet",
                    City = "UpdatedCity",
                    State = "UpdatedState",
                    PostalCode = "UpdatedPostalCode",
                    Country = "UpdatedCountry",
                    IsDefault = true
                }
            }
        };


        var mockUserRepo = new Mock<IUserRepo>();
        var mockMapper = new Mock<IMapper>();
        var mockPasswordRepo = new Mock<IPasswordHashRepo>();
        var userService = new UserService(mockUserRepo.Object, mockMapper.Object, mockPasswordRepo.Object);

        var targetUser = new User { Id = userId, Name = "UpdatedName" , Avatar = "UpdatedAvatar"}; 

        mockUserRepo.Setup(repo => repo.GetById(userId)).Returns(targetUser);
        mockMapper.Setup(mapper => mapper.Map<UserReadDTO>(targetUser))
            .Returns(new UserReadDTO {Name = targetUser.Name, Avatar = targetUser.Avatar });

        var result = userService.UpdateUser(userId, userUpdateDto);
        Assert.NotNull(result);
        Assert.NotNull(userUpdateDto);

        if (userUpdateDto != null && result != null)
        {
            Assert.Equal(userUpdateDto.Name, result.Name);
            Assert.Equal(userUpdateDto.Avatar, result.Avatar);
            if (targetUser != null && targetUser.Addresses != null)
            {
                Assert.Equal(userUpdateDto.AddressUpdateDTOs.Count, targetUser.Addresses.Count);
            }
        }

    }

    [Fact]
    public void UpdateUser_EmptyUserId_ThrowsException()
    {
        var userId = Guid.Empty; 
        var userUpdateDto = new UserUpdateDTO(); 

        var mockUserRepo = new Mock<IUserRepo>();
        var mockMapper = new Mock<IMapper>();
        var mockPasswordRepo = new Mock<IPasswordHashRepo>();
        var userService = new UserService(mockUserRepo.Object, mockMapper.Object, mockPasswordRepo.Object);

        Assert.Throws<Exception>(() => userService.UpdateUser(userId, userUpdateDto));
    }

    [Fact]
    public void UpdateUser_NonexistentUserId_ThrowsException()
    {
        var mockUserRepo = new Mock<IUserRepo>();
        var mockMapper = new Mock<IMapper>();
        var mockPasswordRepo = new Mock<IPasswordHashRepo>();
        var userService = new UserService(mockUserRepo.Object, mockMapper.Object, mockPasswordRepo.Object);

        var userId = Guid.NewGuid(); 
        var userUpdateDto = new UserUpdateDTO();

        mockUserRepo.Setup(repo => repo.GetById(userId!)).Returns(() => null);
        Assert.Throws<Exception>(() => userService.UpdateUser(userId, userUpdateDto));
    }


    [Fact]
    public void UpdateUserProfile_ValidUserIdAndUserUpdateDto_ReturnsUpdatedUserReadDTO()
    {
        var userId = Guid.NewGuid();
        var updatedProfile = new UserUpdateDTO
        {
            Name = "UpdatedName",
            Avatar = "UpdatedAvatar",
            AddressUpdateDTOs = new List<AddressUpdateDTO>
            {
                new AddressUpdateDTO
                {
                    Street = "UpdatedStreet",
                    City = "UpdatedCity",
                    State = "UpdatedState",
                    PostalCode = "UpdatedPostalCode",
                    Country = "UpdatedCountry",
                    IsDefault = true
                }
            }
        };

        var mockUserRepo = new Mock<IUserRepo>();
        var mockMapper = new Mock<IMapper>();
        var mockPasswordRepo = new Mock<IPasswordHashRepo>();
        var userService = new UserService(mockUserRepo.Object, mockMapper.Object, mockPasswordRepo.Object);

        var targetUser = new User { Id = userId, Name = "UpdatedName" ,Avatar = "UpdatedAvatar" };

        mockUserRepo.Setup(repo => repo.GetById(userId)).Returns(targetUser);
        mockMapper.Setup(mapper => mapper.Map<UserReadDTO>(targetUser))
            .Returns(new UserReadDTO {Name = targetUser.Name, Avatar = targetUser.Avatar });

        var result = userService.UpdateUserProfile(userId, updatedProfile);

        Assert.NotNull(result);
        Assert.Equal(updatedProfile.Name, result.Name);
        Assert.Equal(updatedProfile.Avatar, result.Avatar);
        if (targetUser != null && targetUser.Addresses != null)
        {
            Assert.Equal(updatedProfile.AddressUpdateDTOs.Count, targetUser.Addresses.Count);
        }
      
    }

    [Fact]
    public void UpdateUserProfile_EmptyUserId_ThrowsException()
    {
        var userId = Guid.Empty; 
        var updatedProfile = new UserUpdateDTO();

        var mockUserRepo = new Mock<IUserRepo>();
        var mockMapper = new Mock<IMapper>();
        var mockPasswordRepo = new Mock<IPasswordHashRepo>();
        var userService = new UserService(mockUserRepo.Object, mockMapper.Object, mockPasswordRepo.Object);
        Assert.Throws<Exception>(() => userService.UpdateUserProfile(userId, updatedProfile));
    }

    [Fact]
    public void UpdateUserProfile_NonexistentUserId_ThrowsException()
    {
        var userId = Guid.NewGuid(); 
        var updatedProfile = new UserUpdateDTO(); 

        var mockUserRepo = new Mock<IUserRepo>();
        var mockMapper = new Mock<IMapper>();
        var mockPasswordRepo = new Mock<IPasswordHashRepo>();
        var userService = new UserService(mockUserRepo.Object, mockMapper.Object, mockPasswordRepo.Object);
        mockUserRepo.Setup(repo => repo.GetById(userId!)).Returns(() => null);
        Assert.Throws<Exception>(() => userService.UpdateUserProfile(userId, updatedProfile));
    }

    [Fact]
    public void Unregister_ValidUserId_ReturnsTrue()
    {
       
        var userId = Guid.NewGuid(); 

        var mockUserRepo = new Mock<IUserRepo>();
        var mockMapper = new Mock<IMapper>();
        var mockPasswordRepo = new Mock<IPasswordHashRepo>();
        var userService = new UserService(mockUserRepo.Object, mockMapper.Object, mockPasswordRepo.Object);

        var targetUser = new User { Id = userId }; 

        mockUserRepo.Setup(repo => repo.GetById(userId)).Returns(targetUser);
        mockUserRepo.Setup(repo => repo.UnregisterUser(userId)).Returns(true);

        var result = userService.Unregister(userId);
        Assert.True(result);
    }

    [Fact]
    public void Unregister_EmptyUserId_ThrowsException()
    {
        var userId = Guid.Empty; 

        var mockUserRepo = new Mock<IUserRepo>();
        var mockMapper = new Mock<IMapper>();
        var mockPasswordRepo = new Mock<IPasswordHashRepo>();
        var userService = new UserService(mockUserRepo.Object, mockMapper.Object, mockPasswordRepo.Object);

        Assert.Throws<Exception>(() => userService.Unregister(userId));
    }

    [Fact]
    public void Unregister_NonexistentUserId_ThrowsException()
    {
        // invalid id
        var userId = Guid.NewGuid(); 

        var mockUserRepo = new Mock<IUserRepo>();
        var mockMapper = new Mock<IMapper>();
        var mockPasswordRepo = new Mock<IPasswordHashRepo>();
        var userService = new UserService(mockUserRepo.Object, mockMapper.Object, mockPasswordRepo.Object);

        mockUserRepo.Setup(repo => repo.GetById(userId!)).Returns(() => null);
        Assert.Throws<Exception>(() => userService.Unregister(userId));
    }

    [Fact]
    public void DeleteUser_ValidUserId_DeletesUserAndReturnsTrue()
    {
        // valid id
        var userId = Guid.NewGuid(); 

        var mockUserRepo = new Mock<IUserRepo>();
        var mockMapper = new Mock<IMapper>();
        var mockPasswordRepo = new Mock<IPasswordHashRepo>();
        var userService = new UserService(mockUserRepo.Object, mockMapper.Object, mockPasswordRepo.Object);

        var targetUser = new User { Id = userId }; 
        mockUserRepo.Setup(repo => repo.GetById(userId)).Returns(targetUser);
        mockUserRepo.Setup(repo => repo.Delete(userId));

        var result = userService.DeleteUser(userId);

        Assert.True(result);
        mockUserRepo.Verify(repo => repo.Delete(userId), Times.Once);
    }

    [Fact]
    public void DeleteUser_EmptyUserId_ThrowsException()
    {
        var userId = Guid.Empty; 
        var mockUserRepo = new Mock<IUserRepo>();
        var mockMapper = new Mock<IMapper>();
        var mockPasswordRepo = new Mock<IPasswordHashRepo>();
        var userService = new UserService(mockUserRepo.Object, mockMapper.Object, mockPasswordRepo.Object);

        Assert.Throws<Exception>(() => userService.DeleteUser(userId));
        mockUserRepo.Verify(repo => repo.Delete(It.IsAny<Guid>()), Times.Never); 
    }

    [Fact]
    public void DeleteUser_NonexistentUserId_ReturnsFalse()
    {
        var userId = Guid.NewGuid(); 

        var mockUserRepo = new Mock<IUserRepo>();
        var mockMapper = new Mock<IMapper>();
        var mockPasswordRepo = new Mock<IPasswordHashRepo>();
        var userService = new UserService(mockUserRepo.Object, mockMapper.Object, mockPasswordRepo.Object);

        mockUserRepo.Setup(repo => repo.GetById(userId!)).Returns(() => null);

        var result = userService.DeleteUser(userId); 
        Assert.False(result);
        mockUserRepo.Verify(repo => repo.Delete(It.IsAny<Guid>()), Times.Never); 
    }

    [Fact]
    public void Login_ValidCredential_ReturnsToken()
    {
        var mockUserRepo = new Mock<IUserRepo>();
        var mockMapper = new Mock<IMapper>();
        var mockPasswordRepo = new Mock<IPasswordHashRepo>();
        var userService = new UserService(mockUserRepo.Object, mockMapper.Object, mockPasswordRepo.Object);

        var credential = new Credential { Email = "test@example.com", Password = "password123" }; 
        var user = new User { Email = credential.Email, Password = "hashedPassword" }; 

        mockUserRepo.Setup(repo => repo.GetByEmail(credential.Email)).Returns(user);
        mockPasswordRepo.Setup(repo => repo.VerifyPassword(user.Password, credential.Password)).Returns(true);
        mockUserRepo.Setup(repo => repo.GenerateToken(user)).Returns("token");

        var result = userService.Login(credential);
        Assert.NotNull(result);
        Assert.Equal("token", result);
    }

    [Fact]
    public void Login_InvalidCredential_ReturnsNull()
    {

        var mockUserRepo = new Mock<IUserRepo>();
        var mockMapper = new Mock<IMapper>();
        var mockPasswordRepo = new Mock<IPasswordHashRepo>();
        var userService = new UserService(mockUserRepo.Object, mockMapper.Object, mockPasswordRepo.Object);

        var credential = new Credential { Email = "test@example.com", Password = "password123" }; 
        var user = new User { Email = credential.Email, Password = "hashedPassword" }; 

        mockUserRepo.Setup(repo => repo.GetByEmail(credential.Email)).Returns(user);
        mockPasswordRepo.Setup(repo => repo.VerifyPassword(user.Password, credential.Password)).Returns(false);

        var result = userService.Login(credential);
        Assert.Null(result);
    }

    [Fact]
    public void Login_NonexistentUser_ReturnsNull()
    {
        var mockUserRepo = new Mock<IUserRepo>();
        var mockMapper = new Mock<IMapper>();
        var mockPasswordRepo = new Mock<IPasswordHashRepo>();
        var userService = new UserService(mockUserRepo.Object, mockMapper.Object, mockPasswordRepo.Object);
        var credential = new Credential { Email = "test@example.com", Password = "password123" }; 

        mockUserRepo.Setup(repo => repo.GetByEmail(credential.Email!)).Returns(() => null);
        var result = userService.Login(credential);
        Assert.Null(result);
    }





}



