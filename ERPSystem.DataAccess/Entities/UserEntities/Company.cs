using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;


namespace ERPSystem.DataAccess.Entities.UserEntities
{
    public class Company
    {
        private string name = null!;
        private byte[] logo = null!;
        private int maxLogoSize = 102400;
        public int Id { get; set; }
        public string Name
        {
            get => this.name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException("Name could not be null or empty string.");
                }
                this.name = value;
            }
        }
        public string Description { get; set; } = string.Empty;
        public byte[]? Logo
        {
            get => this.logo;
            set
            {
                if (value == null || value.Length == 0)
                {
                    throw new ArgumentNullException("Size of logo could not be null.");
                }
                if (value.Length > this.maxLogoSize)
                {
                    throw new ArgumentOutOfRangeException($"Logo size must be lesser than {this.maxLogoSize}.", nameof(this.logo));
                }
                this.logo = value;
            }
        }
        public string OwnerId { get; set; }
        public virtual User Owner { get; set; }
    }
}
