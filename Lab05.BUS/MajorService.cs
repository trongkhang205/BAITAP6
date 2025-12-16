using Lab05.DAL.Entities;
using System.Collections.Generic;

namespace Lab05.BUS
{
    public class MajorService
    {
        private StudentModel context = new StudentModel();

        public List<Major> GetAll()
        {
            // Trả về danh sách rỗng vì chưa implement Major trong StudentModel
            return new List<Major>();
        }
    }
}