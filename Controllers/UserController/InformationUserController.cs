using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_2.Data;
using Web_2.Models;

namespace Web_2.Controllers;

[ApiController]
public class InformationUserController: ControllerBase
{
    private readonly AppDbContext _context;
    
    public InformationUserController(AppDbContext context)
    {
        _context = context;
    }
    
    [HttpPost]
    [Route("api/AddInformation")]
    public async Task<IActionResult> AddInformation(InformationUserChange informationuc)
    {
        var existinginformatio = await _context.InformationUser.FirstOrDefaultAsync(i => i.User_id == informationuc.User_id);
        if (existinginformatio != null)
        {
            return BadRequest("information user already exists.");
        }
        var informationuser = new InformationUser()
        {
            Idname = informationuc.Idname,
            User_id = informationuc.User_id,
            Username = informationuc.Username,
            Phone = informationuc.Phone,
            Email = informationuc.Email,
            Address = informationuc.Address
        };
        await _context.InformationUser.AddAsync(informationuser);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpGet]
    [Route("API/Find_Information_User/{id_user}")]
    public async Task<IActionResult> FindIformationUser(int id_user)
    {
        var findinformation = await (from i in _context.InformationUser where i.User_id == id_user select i)
            .FirstOrDefaultAsync();
        if (findinformation == null)
        {
            return NotFound("Khong co thong tin");
        }
        
        return Ok(findinformation);
    }
    [HttpPut]
    [Route("api/UpdateInformation")]
    public async Task<IActionResult> UpdateInformation(InformationUserChange informationuc)
    {
        var existingUser = await _context.InformationUser.FirstOrDefaultAsync(i => i.User_id == informationuc.User_id);
        if (existingUser == null)
        {
            return NotFound("Khong co thong tin");
        }

        existingUser.Idname = informationuc.Idname;
        existingUser.Username = informationuc.Username;
        existingUser.Phone = informationuc.Phone;
        existingUser.Email = informationuc.Email;
        existingUser.Address = informationuc.Address;

        _context.InformationUser.Update(existingUser);
        await _context.SaveChangesAsync();

        return Ok(existingUser);
    }
}