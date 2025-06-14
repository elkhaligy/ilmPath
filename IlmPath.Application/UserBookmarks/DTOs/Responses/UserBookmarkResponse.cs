using IlmPath.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.UserBookmarks.DTOs.Responses;

public class UserBookmarkResponse
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty; // FK to ApplicationUser
    public int CourseId { get; set; } // FK to Course
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
