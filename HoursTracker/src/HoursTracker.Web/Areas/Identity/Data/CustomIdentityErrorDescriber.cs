using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoursTracker.Web.Areas.Identity.Data
{
    public class CustomIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError PasswordRequiresUpper()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresUpper),
                Description = "La contraseña debe incluir al menos una letra MAYÚSCULA ('A'-'Z')"
            };
        }

        public override IdentityError PasswordRequiresLower()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresLower),
                Description = "La contraseña debe incluir al menos una letra minúscula ('a'-'z')"
            };
        }

        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresNonAlphanumeric),
                Description = "La contraseña debe contener al menos un caracter alfanumérico."
            };
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError
            {
                Code = nameof(PasswordTooShort),
                Description = "La contraseña deben tener un largo mínimo de {length} caracteres."
            };
        }

        public override IdentityError PasswordMismatch()
        {
            return new IdentityError
            {
                Code = nameof(PasswordMismatch),
                Description = "Contraseña Incorrecta."
            };
        }

        public override IdentityError DefaultError()
        {
            return new IdentityError
            {
                Code = nameof(DefaultError),
                Description = "Ha ocurrido un error."
            };
        }

        public override IdentityError ConcurrencyFailure()
        {
            return new IdentityError
            {
                Code = nameof(ConcurrencyFailure),
                Description = "Ha ocurrido un error, el objeto ya ha sido modificado (Optimistic concurrency failure)."
            };
        }

        public override IdentityError PasswordRequiresDigit()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresDigit),
                Description = "La contraseña debe incluir al menos un dígito ('0'-'9')."
            };
        }

        public override IdentityError PasswordRequiresUniqueChars(int length)
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresUniqueChars),
                Description = "La contraseña requiere caracteres unicos."
            };
        }

        public override IdentityError InvalidEmail(String n)
        {
            return new IdentityError
            {
                Code = nameof(InvalidEmail),
                Description = "Correo electronico es invalido."
            };
        }

        public override IdentityError DuplicateEmail(String d)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateEmail),
                Description = "Correo electronico ya existe."
            };
        }

        public override IdentityError DuplicateUserName(String a)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateUserName),
                Description = "Nombre de usuario ya existe."
            };
        }

    }
}
