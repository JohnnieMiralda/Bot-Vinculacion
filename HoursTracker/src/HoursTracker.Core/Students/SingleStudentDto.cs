using System;
using System.Collections.Generic;
using System.Text;

namespace HoursTracker.Core.Students
{
    public class SingleStudentDto
    {
        public int Id { get; set; }

        public string Account { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string FirstSurname { get; set; }

        public string SecondSurname { get; set; }

        public bool Settlement { get; set; }

        public string Email { get; set; }

        public string CampusName { get; set; }

        public string CareerName { get; set; }

        public bool isInBot { get; set; }

        public string TelegramAccount { get; set; }


        public int Campus { get; set; }

        public IEnumerable<int> Careers { get; set; }
    }
}