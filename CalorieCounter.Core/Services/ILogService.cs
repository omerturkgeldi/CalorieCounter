using CalorieCounter.Core.DTOs;
using CalorieCounter.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCounter.Core.Services
{
    /**************************************************************** 
    * Date: 02.08.2022
    * Signed-off-by: Ömer TÜRKGELDİ < turkgeldiomer@gmail.com>
    * GitHub : https://github.com/omerturkgeldi
    ****************************************************************/
    public interface ILogService : IService<Log>
    {
        Task<CustomResponseDto<List<Log>>> GetLogsByLevel(string level);
        Task<CustomResponseDto<List<Log>>> GetLogsByDay(DateTime day);
        Task<CustomResponseDto<List<Log>>> GetLogsByPage(int page);

    }
}
