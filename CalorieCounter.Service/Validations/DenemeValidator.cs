using FluentValidation;
using CalorieCounter.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCounter.Service.Validations
{
    public class DenemeValidator : AbstractValidator<RoleApp>
    {
        public DenemeValidator()
        {

            //RuleFor(x => x.CityName).NotNull().WithMessage("{PropertyName} is required").NotEmpty().WithMessage("{PropertyName} is required");

            //RuleFor(x => x.).InclusiveBetween(1, int.MaxValue).WithMessage("{PropertyName} must be greater 0");
            //RuleFor(x => x.Stock).InclusiveBetween(1, int.MaxValue).WithMessage("{PropertyName} must be greater 0");

        }


    }
}
