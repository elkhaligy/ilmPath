using IlmPath.Application.Common.Exceptions;
using IlmPath.Application.Common.Interfaces;
using IlmPath.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IlmPath.Application.Lectures.Commands.UpdateLectureOrder
{
    public class UpdateLectureOrderCommandHandler : IRequestHandler<UpdateLectureOrderCommand, Unit>
    {
        private readonly ILectureRepository _lectureRepository;

        public UpdateLectureOrderCommandHandler(ILectureRepository lectureRepository)
        {
            _lectureRepository = lectureRepository;
        }

        public async Task<Unit> Handle(UpdateLectureOrderCommand request, CancellationToken cancellationToken)
        {
            var lecture = await _lectureRepository.GetByIdAsync(request.Id);
            if (lecture == null)
            {
                throw new NotFoundException(nameof(Lecture), request.Id);
            }

            var oldOrder = lecture.Order;
            var newOrder = request.Order;

            // If the order hasn't changed, no need to do anything
            if (oldOrder == newOrder)
            {
                return Unit.Value;
            }

            // Get all lectures in the same section
            var sectionLectures = await _lectureRepository.GetBySectionIdAsync(lecture.SectionId);
            var lectures = sectionLectures.ToList();

            // Update the order of the target lecture
            lecture.Order = newOrder;

            // Reorder other lectures
            if (oldOrder < newOrder)
            {
                // Moving down: decrease order of lectures in between
                foreach (var l in lectures.Where(l => l.Id != lecture.Id && l.Order > oldOrder && l.Order <= newOrder))
                {
                    l.Order--;
                }
            }
            else
            {
                // Moving up: increase order of lectures in between
                foreach (var l in lectures.Where(l => l.Id != lecture.Id && l.Order >= newOrder && l.Order < oldOrder))
                {
                    l.Order++;
                }
            }

            // Save all changes
            await _lectureRepository.UpdateAsync(lecture);
            foreach (var l in lectures.Where(l => l.Id != lecture.Id))
            {
                await _lectureRepository.UpdateAsync(l);
            }

            return Unit.Value;
        }
    }
} 