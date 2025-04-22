using Microsoft.AspNetCore.Mvc;
using Gallery.Models;
using Gallery.Interfaces;

namespace Gallery.Controllers;

/// <summary>
/// Controller for managing comments in the Gallery API.
/// Provides endpoints for CRUD operations and additional utilities for comments.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CommentController : ControllerBase
{
    private readonly ICommentRepository _commentRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommentController"/> class.
    /// </summary>
    /// <param name="commentRepository">The comment repository to interact with the database.</param>
    public CommentController(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    /// <summary>
    /// Retrieve all comments.
    /// </summary>
    /// <returns>A list of all comments.</returns>
    /// <response code="200">Returns the list of comments</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet()]
    public async Task<IActionResult> GetAllComments()
    {
        var comments = await _commentRepository.GetAllAsync();
        return Ok(comments);
    }

    /// <summary>
    /// Retrieve a specific comment by its ID.
    /// </summary>
    /// <param name="id">The ID of the comment to retrieve.</param>
    /// <returns>The comment with the specified ID.</returns>
    /// <response code="200">Returns the comment with the specified ID</response>
    /// <response code="404">If the comment is not found</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCommentById(int id)
    {
        var comment = await _commentRepository.GetByIdAsync(id);
        if (comment == null)
        {
            return NotFound();
        }
        return Ok(comment);
    }

    /// <summary>
    /// Create a new comment.
    /// </summary>
    /// <param name="comment">The comment to create.</param>
    /// <returns>The created comment with its ID.</returns>
    /// <remarks>
    /// Sample Request:
    ///     POST /api/comment
    ///     {
    ///         "text": "This is a sample comment",
    ///         "artifactId": 1,
    ///         "userId": 2
    ///     }
    /// </remarks>
    /// <response code="201">Returns the newly created comment</response>
    /// <response code="400">If the comment is null or invalid</response>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<IActionResult> CreateComment([FromBody] Comment comment)
    {
        if (comment == null)
        {
            return BadRequest();
        }
        await _commentRepository.AddAsync(comment);

        var createdComment = await _commentRepository.GetByIdAsync(comment.Id);

        return CreatedAtAction(nameof(GetCommentById), new { id = comment.Id }, comment);
    }

    /// <summary>
    /// Update an existing comment.
    /// </summary>
    /// <param name="id">The ID of the comment to update.</param>
    /// <param name="comment">The updated comment data.</param>
    /// <returns>The updated comment.</returns>
    /// <remarks>
    /// Sample Request:
    ///     PUT /api/comment/1
    ///     {
    ///         "id": 1,
    ///         "text": "Updated comment text",
    ///         "artifactId": 1,
    ///         "userId": 2
    ///     }
    /// </remarks>
    /// <response code="200">Returns the updated comment</response>
    /// <response code="400">If the comment data is invalid</response>
    /// <response code="404">If the comment is not found</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateComment(int id, [FromBody] Comment comment)
    {
        if (id < 0)
        {
            return BadRequest("Id cannot be negative");
        }
        var existingComment = await _commentRepository.GetByIdAsync(id);
        if (existingComment == null)
        {
            return NotFound();
        }
        await _commentRepository.UpdateAsync(comment);
        return Ok(comment);
    }

    /// <summary>
    /// Delete a comment by its ID.
    /// </summary>
    /// <param name="id">The ID of the comment to delete.</param>
    /// <returns>No content if the deletion is successful.</returns>
    /// <response code="204">If the comment is successfully deleted</response>
    /// <response code="404">If the comment is not found</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment(int id)
    {
        var existingComment = await _commentRepository.GetByIdAsync(id);
        if (existingComment == null)
        {
            return NotFound();
        }
        await _commentRepository.DeleteAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Retrieve all comments for a specific artifact by its ID.
    /// </summary>
    /// <param name="artifactId">The ID of the artifact to retrieve comments for.</param>
    /// <returns>A list of comments for the specified artifact.</returns>
    /// <response code="200">Returns the list of comments for the specified artifact</response>
    /// <response code="404">If no comments are found for the artifact</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("artifact/{artifactId}")]
    public async Task<IActionResult> GetCommentsByArtifactId(int artifactId)
    {
        var comments = await _commentRepository.GetByArtifactIdAsync(artifactId);
        if (comments == null || !comments.Any())
        {
            return NotFound();
        }
        return Ok(comments);
    }
}
