
using ERPSystem.Resources;

namespace ERPSystem.DataAccess.Entities.UserEntities
{
    public class UserProfile
    {
        public int Id { get; set; }
        private string name = null!;
        private string surname = null!;
        private byte[]? profileLogo;
        private int maxLogoSize = 102400;
        public string JobPosition { get; set; } = string.Empty;
        public DateTime? BirthDate { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public byte[]? ProfileLogo
        {
            get => this.profileLogo;
            set
            {
                if (value.Length == 0)
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
