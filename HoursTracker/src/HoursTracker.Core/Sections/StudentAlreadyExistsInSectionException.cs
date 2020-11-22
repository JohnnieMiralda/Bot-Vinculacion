using System;
using System.Collections.Generic;
using System.Text;

namespace HoursTracker.Core.Sections
{
    public class StudentAlreadyExistsInSectionException : Exception
    {
        public StudentAlreadyExistsInSectionException() : base("Student already exists in section")
        {

        }
    }
}
