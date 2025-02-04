﻿using System.Globalization;
using SocialNetwork.Domain.Aggregates;

namespace SocialNetwork.Domain.Services;

public class UserService : IUserService
{
    private readonly SocialNetworkContext _context;

    public UserService(SocialNetworkContext context)
    {
        _context = context;
    }

    public async Task<User> AddUser(string firstName, string lastName, DateOnly birthday)
    {
        var userName = new UserName( firstName,lastName);
        var userBirthday = new Birthday(birthday);
        
        var user = new User(userName,userBirthday);

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        
        return user;
    }

    public async Task<Publication> AddPublication(string textContent, string mediaContent, Guid userId)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == userId);
        
        if (user is null) throw new UserNotFound();

        var newPublication = new Publication(textContent, mediaContent, user);

        await _context.Publications.AddAsync(newPublication);
        await _context.SaveChangesAsync();

        return newPublication;
    }
}