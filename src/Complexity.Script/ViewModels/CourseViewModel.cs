using System.Collections.Generic;
using System.Collections.ObjectModel;
using VMS.TPS.Common.Model.API;

namespace Complexity.ViewModels
{
    public class CourseViewModel
    {
        public Course Course { get; set; }

        public string DisplayName
        {
            get { return "Course " + Course.Id; }
        }

        public ObservableCollection<FieldViewModel> Fields { get; set; }

        public CourseViewModel(Course course, IEnumerable<FieldViewModel> fields)
        {
            Course = course;
            Fields = new ObservableCollection<FieldViewModel>(fields);
        }
    }
}