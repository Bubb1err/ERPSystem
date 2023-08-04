

namespace ERPSystem.DataAccess.Entities.UserEntities
{
    public class Skill
    {
        private string title = null!;
        private double yearsOfExperience; 
        public int Id { get; set; }
        public string Title
        {
            get => this.title;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException("Title could not be empty.");
                }
                this.title = value;
            }
        }
        public double YearsOfExperience
        {
            get => this.yearsOfExperience;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Years of experience must be greater or equal than zero.");
                }
                this.yearsOfExperience = value;
            }
        }
        public virtual ICollection<UserProfile> Users { get; set; } = new List<UserProfile>();
    }
}
