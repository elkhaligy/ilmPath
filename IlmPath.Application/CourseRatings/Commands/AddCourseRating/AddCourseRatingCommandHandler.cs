using AutoMapper;
using IlmPath.Application.Common.Interfaces;
using IlmPath.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IlmPath.Application.CourseRatings.Commands.AddCourseRating
{
    public class AddCourseRatingCommandHandler : IRequestHandler<AddCourseRatingCommand, int>
    {
        private readonly ICourseRatingRepository _courseRatingRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IMapper _mapper;

        public AddCourseRatingCommandHandler(
            ICourseRatingRepository courseRatingRepository,
            IEnrollmentRepository enrollmentRepository,
            IMapper mapper)
        {
            _courseRatingRepository = courseRatingRepository;
            _enrollmentRepository = enrollmentRepository;
            _mapper = mapper;
        }

        public async Task<int> Handle(AddCourseRatingCommand request, CancellationToken cancellationToken)
        {
            // 1. Verify user is enrolled in the course
            var isEnrolled = await _enrollmentRepository.IsUserEnrolledInCourseAsync(request.UserId, request.CourseId);
            if (!isEnrolled)
            {
                throw new InvalidOperationException("You must be enrolled in the course to rate it.");
            }

            // 2. Verify user has not already rated the course
            var hasRated = await _courseRatingRepository.HasUserRatedCourseAsync(request.UserId, request.CourseId);
            if (hasRated)
            {
                throw new InvalidOperationException("You have already rated this course.");
            }

            var courseRating = _mapper.Map<CourseRating>(request);

            var newRating = await _courseRatingRepository.AddRatingAsync(courseRating);

            return newRating.Id;
        }
    }
} 