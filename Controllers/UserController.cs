using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleAuthAPI.Database;
using SimpleAuthAPI.Model.Dtos.User;
using SimpleAuthAPI.Model.Entities;

namespace SimpleAuthAPI.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserController(IMapper mapper, AppDbContext context, UserManager<ApplicationUser> userManager)
    {
        _mapper = mapper;
        _context = context;
        _userManager = userManager;
    }
    #region Kullanıcı Listeleme Kısmı 
    
    [HttpGet("[action]")]
    public async Task<ActionResult<ApplicationUserDto[]>> ListUsers()
    {
      var users =await  _userManager.Users.ToListAsync();
      if (users.Count == 0)
      {
          return NotFound("User not found");
      }
      var userDtos = _mapper.Map<ApplicationUserDto[]>(users);
      return userDtos;
    }
    
    #endregion

    #region Kullanıcı Getirme Kısmı

    [HttpGet("[action]")]
    public async Task<ActionResult<ApplicationUserDto>> GetUser(string id)
    {
        
        var user = await _userManager.Users.FirstOrDefaultAsync(x=>x.Id == id );
        if (user == null)
        {
           return NotFound("User not found");
        }
        var userDto = _mapper.Map<ApplicationUserDto>(user);
        return userDto;
    }

    #endregion
    
    #region Kullanıcı Güncelleme Kısmı

    [HttpPost("[action]")]
    public ActionResult<ApplicationUserDto> UpdateUser(ApplicationUserDto userDto)
    {
        var selectedUser = _context.Users.FirstOrDefault(x => x.Id == userDto.Id);
        if (selectedUser == null)
        {
            NotFound("User not found");
        }
        selectedUser.FirstName = userDto.FirstName;
        selectedUser.LastName = userDto.LastName;
        selectedUser.Email = userDto.Email;
        selectedUser.ModifiedDate = DateTime.Now;
        _mapper.Map(userDto, selectedUser);
        _context.SaveChanges();
        return Ok("Updated user");

    }

    #endregion

    #region Kullanıcı Silme Kısmı

    [HttpDelete("[action]")]
    public ActionResult<ApplicationUserDto> DeleteUser(string id)
    {
        var selectedUser = _context.Users.FirstOrDefault(x => x.Id ==id);
        if (selectedUser == null)
        {
            return NotFound("User not found");
        }
        _context.Users.Remove(selectedUser);
        _context.SaveChanges();
        return Ok("Deleted user");
    }

    #endregion
    
}