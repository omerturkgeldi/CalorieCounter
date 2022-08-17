using AutoMapper;
using Microsoft.EntityFrameworkCore;
using CalorieCounter.Core.DTOs;
using CalorieCounter.Core.Repositories;
using CalorieCounter.Core.Services;
using CalorieCounter.Core.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalorieCounter.Core.Models;

namespace CalorieCounter.Service.Services
{
    public class LogService : Service<Log>, ILogService
    {
        private readonly ILogRepository _logRepository;
        private readonly IMapper _mapper;

        public LogService(IGenericRepository<Log> repository, IUnitOfWork unitOfWork, IMapper mapper, ILogRepository logRepository) : base(repository, unitOfWork, mapper)
        {
            _mapper = mapper;
            _logRepository = logRepository;
        }

        public async Task<CustomResponseDto<List<Log>>> GetLogsByDay(DateTime day)
        {
            var myDate = day.ToString("yyyy'/'MM'/'dd");
            var allLogs = await _logRepository.GetAll().ToListAsync();

            List<Log> logsByDay = new List<Log>();
            foreach (var item in allLogs)
            {
                var logDate = item.Logged.Split(" ")[0];
                if (logDate == myDate) 
                {
                    logsByDay.Add(item);
                }
            }
            return CustomResponseDto<List<Log>>.Success(200, logsByDay);
        }

        public async Task<CustomResponseDto<List<Log>>> GetLogsByLevel(string level)
        {
            var logsWithLevel = await _logRepository.Where(x => x.Level == level).ToListAsync();
            return CustomResponseDto<List<Log>>.Success(200, logsWithLevel);

        }

        public async Task<CustomResponseDto<List<Log>>> GetLogsByPage(int page)
        {
            var logs = _logRepository.GetAll();

            var pageResults = 8f;
            var pageCount = Math.Ceiling(logs.Count() / pageResults);

            //var paginatedLogs = logs.Skip((page -  1) * (int)pageResults)
            //        .Take((int)pageResults).ToList();

            var paginatedLogs = await _logRepository.GetAll().Skip((page -  1) * (int)pageResults).Take((int)pageResults).ToListAsync();


            return CustomResponseDto<List<Log>>.Success(200, paginatedLogs);


        }


    }
}
