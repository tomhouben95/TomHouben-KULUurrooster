namespace TomHouben.KULUurroosterfeed.Models
{
    public class CourseSelectionViewModel
    {
        public CourseSelectionViewModel(string name, bool selected)
        {
            Name = name;
            Selected = selected;
        }
        
        public CourseSelectionViewModel(){}
        
        public string Name { get; set;}
        
        public bool Selected { get; set; }
    }
}