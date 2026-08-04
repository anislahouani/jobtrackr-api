using JobTrackr.Api.Data;
using JobTrackr.Api.DTOs.Auth;
using JobTrackr.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace JobTrackr.Api.Services;

public class AuthService
{
    private readonly AppDbContext _dbContext;
    private readonly JwtService _jwtService;

    public AuthService(AppDbContext dbContext, JwtService jwtService)
    {
        _dbContext = dbContext;
        _jwtService = jwtService;
    }

    public async Task<AuthResponse?> RegisterAsync(RegisterRequest request)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        var emailAlreadyExists = await _dbContext.Users
            .AnyAsync(user => user.Email == normalizedEmail);

        if (emailAlreadyExists)
        {
            return null;
        }

        var user = new User
        {
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Email = normalizedEmail,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        return CreateAuthResponse(user);
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        var user = await _dbContext.Users
            .SingleOrDefaultAsync(user => user.Email == normalizedEmail);

        if (user is null)
        {
            return null;
        }

        var passwordIsValid = BCrypt.Net.BCrypt.Verify(
            request.Password,
            user.PasswordHash);

        if (!passwordIsValid)
        {
            return null;
        }

        return CreateAuthResponse(user);
    }

    private AuthResponse CreateAuthResponse(User user)
    {
        return new AuthResponse
        {
            Token = _jwtService.GenerateToken(user),
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName
        };
    }
}