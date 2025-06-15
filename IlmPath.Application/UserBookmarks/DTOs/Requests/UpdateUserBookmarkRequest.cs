using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.UserBookmarks.DTOs.Requests;

public class UpdateUserBookmarkRequest
{
    [Required]
    public string UserId { get; set; } = string.Empty; // FK to ApplicationUser

    [Required]
    public int CourseId { get; set; } // FK to Course
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
