using IlmPath.Application.Common.Exceptions;
using IlmPath.Application.Common.Interfaces;
using IlmPath.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IlmPath.Application.CourseRatings.Commands.DeleteCourseRating
{
    public class DeleteCourseRatingCommandHandler : IRequestHandler<DeleteCourseRatingCommand>
    {
        private readonly ICourseRatingRepository _courseRatingRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public DeleteCourseRatingCommandHandler(ICourseRatingRepository courseRatingRepository, UserManager<ApplicationUser> userManager)
        {
            _courseRatingRepository = courseRatingRepository;
            _userManager = userManager;
        }

        public async Task Handle(DeleteCourseRatingCommand request, CancellationToken cancellationToken)
        {
            var rating = await _courseRatingRepository.GetRatingByIdAsync(request.RatingId);
            var currentUser = await _userManager.FindByIdAsync(request.UserId);
            if (rating == null)
            {
                throw new NotFoundException(nameof(rating), request.RatingId);
            }

            // If the user is the owner, they can delete it.
            if (rating.UserId == request.UserId || (currentUser != null && await _userManager.IsInRoleAsync(currentUser, "Admin")))
            {
                await _courseRatingRepository.DeleteRatingAsync(request.RatingId);
                return;
            }

            // If not the owner and not an admin, they are not authorized.
            throw new UnauthorizedAccessException("You are not authorized to delete this rating.");
        }
    }
} 