using Microsoft.AspNetCore.Mvc;
using Gallery.Models;
using Gallery.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Gallery.Controllers;

/// <summary>
/// Controller for managing users in the Gallery API.
/// Provides endpoints for CRUD operations and additional utilities for users.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserController"/> class.
    /// </summary>
    /// <param name="userRepository">The user repository to interact with the database.</param>
    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    /// <summary>
    /// Retrieve all users.
    /// </summary>
    /// <returns>A list of all users.</returns>
    /// <response code="200">Returns the list of users</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [AllowAnonymous]
    [HttpGet()]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userRepository.GetAllAsync();
        return Ok(users);
    }

    /// <summary>
    /// Retrieve a specific user by their ID.
    /// </summary>
    /// <param name="id">The ID of the user to retrieve.</param>
    /// <returns>The user with the specified ID.</returns>
    /// <response code="200">Returns the user with the specified ID</response>
    /// <response code="404">If the user is not found</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("searchid/{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    /// <summary>
    /// Create a new user.
    /// </summary>
    /// <param name="user">The user to create.</param>
    /// <returns>The created user with their ID.</returns>
    /// <remarks>
    /// Sample Request:
    ///     POST /api/user
    ///     {
    ///         "username": "newuser",
    ///         "email": "newuser@example.com",
    ///         "passwordHash": "hashedpassword",
    ///         "displayName": "New User",
    ///         "role": "Member"
    ///     }
    /// </remarks>
    /// <response code="201">Returns the newly created user</response>
    /// <response code="400">If the user is null or invalid</response>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] User user)
    {
        if (user == null)
        {
            return BadRequest();
        }
        await _userRepository.AddAsync(user);
        return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
    }

    /// <summary>
    /// Update an existing user.
    /// </summary>
    /// <param name="id">The ID of the user to update.</param>
    /// <param name="user">The updated user data.</param>
    /// <returns>The updated user.</returns>
    /// <remarks>
    /// Sample Request:
    ///     PUT /api/user/1
    ///     {
    ///         "id": 1,
    ///         "username": "updateduser",
    ///         "email": "updateduser@example.com",
    ///         "passwordHash": "updatedhashedpassword",
    ///         "displayName": "Updated User",
    ///         "role": "Admin"
    ///     }
    /// </remarks>
    /// <response code="200">Returns the updated user</response>
    /// <response code="400">If the user data is invalid</response>
    /// <response code="404">If the user is not found</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Policy = "AdminOnly")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
    {
        if (id < 0)
        {
            return BadRequest("Id cannot be negative");
        }
        var existingUser = await _userRepository.GetByIdAsync(id);
        if (existingUser == null)
        {
            return NotFound();
        }
        await _userRepository.UpdateAsync(user);
        return Ok(user);
    }

    /// <summary>
    /// Delete a user by their ID.
    /// </summary>
    /// <param name="id">The ID of the user to delete.</param>
    /// <returns>No content if the deletion is successful.</returns>
    /// <response code="204">If the user is successfully deleted</response>
    /// <response code="404">If the user is not found</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        await _userRepository.DeleteAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Check if a user exists by their ID.
    /// </summary>
    /// <param name="id">The ID of the user to check.</param>
    /// <returns>A boolean indicating whether the user exists.</returns>
    /// <response code="200">Returns true if the user exists, false otherwise</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("exists/{id}")]
    public async Task<IActionResult> UserExists(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    /// <summary>
    /// Retrieve a specific user by their username.
    /// </summary>
    /// <param name="username">The username of the user to retrieve.</param>
    /// <returns>The user with the specified username.</returns>
    /// <response code="200">Returns the user with the specified username</response>
    /// <response code="404">If the user is not found</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("searchname/{username}")]
    public async Task<IActionResult> GetUserByUsername(string username)
    {
        var user = await _userRepository.GetByUsernameAsync(username);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }
}
