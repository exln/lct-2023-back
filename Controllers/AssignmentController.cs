/*
using MediWingWebAPI.Data;
using MediWingWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediWingWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AssignmentController : ControllerBase
{
    private readonly ILogger<AssignmentController> _logger;
    private readonly ApiDbContext _context;

    public AssignmentController(
        ILogger<AssignmentController> logger,
        ApiDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    // GET: api/assignments
    [HttpGet()]
    public async Task<IActionResult> GetAllAssignments()
    {
        List<Assignment> assignments = await _context.Assignments.ToListAsync();
        return Ok(assignments);
    }

    // GET: api/tasks/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTaskById(Guid id)
    {
        Assignment assignment = await _context.Assignments.FindAsync(id);
        if (assignment == null)
        {
            return NotFound();
        }

        return Ok(assignment);
    }

    // POST: api/tasks
    [HttpPost]
    public async Task<IActionResult> CreateTask(AssignmentCreation assignmentCreation)
    {
        if (assignmentCreation == null)
        {
            return BadRequest();
        }
        
        Assignment mtask = new()
        {
            Title = assignmentCreation.Title,
            Description = assignmentCreation.Description,
            DueDate = assignmentCreation.DueDate,
            Id = Guid.NewGuid(),
            CreatedDate = DateTime.UtcNow,
            IsCompleted = false
        };

        await _context.Assignments.AddAsync(mtask);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetTaskById), new { id = mtask.Id }, mtask);
    }

    // PUT: api/tasks/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(Guid id, Assignment assignment)
    {
        Assignment existingAssignment = await _context.Assignments.FindAsync(id);
        if (existingAssignment == null)
        {
            return NotFound();
        }

        existingAssignment.Title = assignment.Title;
        existingAssignment.Description = assignment.Description;
        existingAssignment.DueDate = assignment.DueDate;
        existingAssignment.IsCompleted = assignment.IsCompleted;

        _context.Assignments.Update(existingAssignment);

        return CreatedAtAction(nameof(GetTaskById), new { id = assignment.Id }, assignment);
    }

    // DELETE: api/tasks/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(Guid id)
    {
        Assignment assignment = await _context.Assignments.FindAsync(id);
        if (assignment == null)
        {
            return NotFound();
        }

        _context.Assignments.Remove(assignment);
        await _context.SaveChangesAsync();

        return Ok("Task deleted successfully!");
    }
}
*/
