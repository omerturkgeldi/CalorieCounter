using AutoMapper;
using CalorieCounter.Core.DTOs;
using CalorieCounter.Core.Models;
using CalorieCounter.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CalorieCounter.API.Controllers
{
    public class LogController : CustomBaseController
    {
        private readonly IMapper _mapper;
        private readonly ILogService _service;

        public LogController(IMapper mapper, ILogService service)
        {
            _mapper=mapper;
            _service=service;
        }



        [HttpGet("[action]/{level}")]
        public async Task<IActionResult> GetLogsByLevel(string level)
        {
            return CreateActionResult(await _service.GetLogsByLevel(level));
        }


        [HttpGet("[action]/{day}")]
        public async Task<IActionResult> GetLogsByDay(DateTime day)
        {
            return CreateActionResult(await _service.GetLogsByDay(day));
        }


        [HttpGet("[action]/{page}")]
        public async Task<IActionResult> GetLogsByPage(int page)
        {
            return CreateActionResult(await _service.GetLogsByPage(page));
        }


        [HttpGet]
        public async Task<IActionResult> All()
        {
            var datas = await _service.GetAllAsync();
            var datasList = _mapper.Map<List<Log>>(datas.ToList());
            return CreateActionResult(CustomResponseDto<List<Log>>.Success(200, datasList));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _service.GetByIdAsync(id);
            var dataDto = _mapper.Map<Log>(data);
            return CreateActionResult(CustomResponseDto<Log>.Success(200, dataDto));
        }



        ///// <summary>
        ///// Logların tek tek silinmesi güvenli olmayabilir. Bu yüzden bu fonksiyon yorum olarak bırakılmıştır
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Remove(int id)
        //{
        //    var data = await _service.GetByIdAsync(id);
        //    await _service.RemoveAsync(data);
        //    return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        //}



    }
}
