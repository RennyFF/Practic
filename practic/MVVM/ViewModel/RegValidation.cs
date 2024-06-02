using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using practic.MVVM.Model;

namespace practic.MVVM.ViewModel
{
    public class RegValidation : AbstractValidator<RegistrationViewModel>
    {
        DataBase db = new DataBase();
        public RegValidation()
        {
            RuleFor(user => user.Login)
                .NotEmpty().WithMessage("Логин нуждается в заполнении")
                .Length(4, 20).WithMessage("Логин должен быть длинною от 4 до 20")
                .Matches(@"^[a-zA-Z0-9]+$").WithMessage("Логин должен содержать только буквы")
                .Must(BeUniqueLogin).WithMessage("Этот логин уже занят"); ;

            RuleFor(user => user.Password)
                .NotEmpty().WithMessage("Пароль нуждается в заполнении")
                .Length(8, 20).WithMessage("Пароль должен быть длинною от 8 до 20");

            RuleFor(user => user.Firstname)
                .NotEmpty().WithMessage("Имя нуждается в заполнении")
                .MinimumLength(2).WithMessage("Имя должно быть от 2ух символов");

            RuleFor(user => user.Secondname)
                .NotEmpty().WithMessage("Фамилия нуждается в заполнении")
                .MinimumLength(2).WithMessage("Фамилия должна быть от 2ух символов");
        }
        private bool BeUniqueLogin(string login)
        {
            return db.IsLoginUnique(login);
        }
    }
}
