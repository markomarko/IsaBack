using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalIsa.DAL.Entities
{
    public class Specialization
    {
        public List<string> Specializations = new List<string>(new[] {
                "Psychiatrist", 
                "Cardiologist",
                "Dermatologist",
                "Endocrinologist",
                "Gastroenterologist",
                "Ophthalmologist",
                "Otolaryngologist",
                "Pulmonologist",
                "Neurologist",
                "Oncologist",
                "Anesthesiologist"
        });
        public List<string> GetList()
        {
            return Specializations;
        }
    }
}
