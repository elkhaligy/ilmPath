using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.UserBookmarks.DTOs.Requests;

public class CreateUserBookmarkRequest
{
    public string UserId { get; set; } = string.Empty; // FK to ApplicationUser
    public int CourseId { get; set; } // FK to Course
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
