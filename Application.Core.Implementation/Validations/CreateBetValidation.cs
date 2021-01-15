using System;
using System.Collections.Generic;
using System.Text;
using Domain.Core.Classes;
using FluentValidation;

namespace Application.Core.Implementation.Validations
{
    public class CreateBetValidation : AbstractValidator<Bet>
    {
        public CreateBetValidation()
        {
            RuleFor(x => x.NumberBet).GreaterThanOrEqualTo(0).LessThanOrEqualTo(36);
            RuleFor(x => x.Amount).GreaterThan(0).LessThanOrEqualTo(10000);
        }
    }
}
