
namespace ERPSystem.DataAccess.Entities.UserEntities
{
    public class UserProfile
    {
        public int Id { get; set; }
        private string name = null!;
        private string surname = null!;
        private byte[] profileLogo = null!;
        private int maxLogoSize = 102400;
        public string Name
        {
            get => this.name;
            set 
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException("Name could not be empty.");
                }
                this.name = value;
            }
        }
        public string Surname
        {
            get => this.surname;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException("Surname could not be empty.", nameof(this.surname));
                }
            }
        }
        public byte[] ProfileLogo
        {
            get => this.profileLogo;
            set
            {
                if (value == null || value.Length == 0)
                {
                    throw new ArgumentNullException("Size of logo could not be null.");
                }
                if (value.Length > this.maxLogoSize)
                {
                    throw new ArgumentOutOfRangeException($"Logo size must be lesser than {this.maxLogoSize}.", nameof(this.profileLogo));
                }
                this.profileLogo = value;
            }
        }
        public ICollection<Skill> Skills { get; set; } = new List<Skill>();
        public string UserId { get; set; }
        public User User { get; set; }
        
    }
}
