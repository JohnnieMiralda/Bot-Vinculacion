using HoursTracker.Domain.Aggregates.Periods;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoursTracker.Core.Periods
{
    public class PeriodService : IPeriodService
    {
        private readonly IPeriodRepository _periodRepository;

        public PeriodService(IPeriodRepository periodRepository)
        {
            _periodRepository = periodRepository;
        }

        public async Task<Period> FindById(int id)
        {
            return await _periodRepository.FindById(id);
        }

        public async Task<IEnumerable<Period>> All()
        {
            return await _periodRepository
                .Filter(period => !period.Disabled).ToListAsync();
        }

        public async Task Remove(int id)
        {
            var section = await _periodRepository.FindById(id);
            await _periodRepository.Disable(section);
        }

        public async Task Update(int id, Period period)
        {
            var per = await _periodRepository.FindById(id);

            per.Code = period.Code;
            per.Semester = period.Semester;
            per.Trimester = period.Trimester;
            per.Year = period.Year;

            await _periodRepository.Update(per);
        }

        public async Task Create(Period period)
        {
            var newPeriod = new Period
            {
                Code = period.Code,
                Semester = period.Semester,
                Trimester = period.Trimester,
                Year = period.Year
            };

            await _periodRepository.Add(newPeriod);
        }
    }
}
