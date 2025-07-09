using AutoMapper;
using IlmPath.Application.Common.Exceptions;
using IlmPath.Application.Common.Interfaces;
using IlmPath.Application.Courses.DTOs;
using IlmPath.Application.Lectures.DTOs;
using IlmPath.Application.Sections.DTOs;
using IlmPath.Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IlmPath.Application.Courses.Queries.GetCourseWithContent
{
    public class GetCourseWithContentQueryHandler : IRequestHandler<GetCourseWithContentQuery, CourseWithContentResponse>
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ISectionRepository _sectionRepository;
        private readonly ILectureRepository _lectureRepository;
        private readonly IMapper _mapper;

        public GetCourseWithContentQueryHandler(
            ICourseRepository courseRepository,
            ISectionRepository sectionRepository, 
            ILectureRepository lectureRepository,
            IMapper mapper)
        {
            _courseRepository = courseRepository;
            _sectionRepository = sectionRepository;
            _lectureRepository = lectureRepository;
            _mapper = mapper;
        }

        public async Task<CourseWithContentResponse> Handle(GetCourseWithContentQuery request, CancellationToken cancellationToken)
        {
            // Get course
            var course = await _courseRepository.GetByIdAsync(request.CourseId);
            if (course == null)
            {
                throw new NotFoundException(nameof(Course), request.CourseId);
            }

            // Get sections for the course
            var sections = await _sectionRepository.GetByCourseIdAsync(request.CourseId);
            
            var response = _mapper.Map<CourseWithContentResponse>(course);
            response.Sections = new List<SectionWithLecturesResponse>();
            
            int totalDuration = 0;
            int totalLectures = 0;

            // For each section, get its lectures
            foreach (var section in sections.OrderBy(s => s.Order))
            {
                var lectures = await _lectureRepository.GetBySectionIdAsync(section.Id);
                var lectureResponses = _mapper.Map<List<LectureResponse>>(lectures.OrderBy(l => l.Order));
                
                var sectionDuration = lectureResponses.Sum(l => l.DurationInMinutes ?? 0);
                totalDuration += sectionDuration;
                totalLectures += lectureResponses.Count;

                var sectionWithLectures = _mapper.Map<SectionWithLecturesResponse>(section);
                sectionWithLectures.Lectures = lectureResponses;
                sectionWithLectures.DurationMinutes = sectionDuration;
                sectionWithLectures.LecturesCount = lectureResponses.Count;
                
                response.Sections.Add(sectionWithLectures);
            }

            // Set calculated totals
            response.TotalDurationMinutes = totalDuration;
            response.TotalLecturesCount = totalLectures;
            response.SectionsCount = response.Sections.Count;

            return response;
        }
    }
} 