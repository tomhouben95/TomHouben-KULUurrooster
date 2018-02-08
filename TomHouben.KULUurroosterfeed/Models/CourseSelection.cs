namespace TomHouben.KULUurroosterfeed.Models
{
    public class CourseSelection
    {
        public CourseSelection(string name, bool selected)
        {
            Name = name;
            Selected = selected;
        }
        
        public CourseSelection(){}
        
        public string Name { get; set;}
        
        public bool Selected { get; set; }
    }
}