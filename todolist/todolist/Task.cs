using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace todolist
{
    class Task
    {
        public string TaskName { get; set; }
        public bool IsChecked { get; set; }
        public string DateTask { get; set; }
        public string TimeTask { get; set; }
        public string Comment { get; set; }
        public int Id { get; set; }
    }
}
