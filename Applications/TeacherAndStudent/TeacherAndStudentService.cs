using System;
using System.Collections.Generic;
using System.Text;
using Applications.Products;
using Domains;

namespace Applications.TeacherAndStudent
{
    public class TeacherAndStudentService(IGenericRepository<Teacher> teacherRepository, IUnitOfWork unitOfWork)
    {
        public void Example1()
        {
            var teacher = new Teacher()
            {
                Name = "Teacher 1"
            };


            teacher.Students = new List<Student>();

            var student = new Student()
            {
                Name = "Student 1"
            };
            var student2 = new Student()
            {
                Name = "Student 2"
            };

            teacher.Students.AddRange([student, student2]);


            teacherRepository.Add(teacher);

            unitOfWork.Commit();
        }


        public void Example2()
        {
            var teacher = teacherRepository.GetById(1);

            teacher.Students = new List<Student>();

            var student = new Student()
            {
                Name = "Student 1"
            };

            var student2 = new Student()
            {
                Name = "Student 2"
            };

            teacher.Students.AddRange([student, student2]);


            unitOfWork.Commit();
        }
    }
}
