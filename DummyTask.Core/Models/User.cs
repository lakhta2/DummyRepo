using DummyTask.Core.ContractsAndDTO_s;

namespace DummyTask.Core.Models
{
    public class User
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Login { get; private set; } = String.Empty;
        public string Password { get; private set; } = String.Empty;
        public string Name { get; private set; } = String.Empty;
        public int Gender { get; private set; } = 2;
        public DateTime? Birthday { get; private set; }
        public bool Admin {  get; private set; }
        public DateTime CreatedOn { get; } = DateTime.UtcNow;
        public string CreatedBy { get; private set; } = String.Empty;
        public DateTime? ModifiedOn { get; private set; } 
        public string ModifiedBy { get; private set; } = String.Empty;
        public DateTime RevokedOn { get; private set; } = DateTime.MinValue;
        public string RevokedBy { get; private set; } = String.Empty;

        public static User? Create(CreateUser createUser)
        {      
            return new User
            {
            
                Login = createUser.Login,
                Password = createUser.Password,
                Name = createUser.Name,
                Gender = createUser.Gender,
                Birthday = createUser.Birthday,
                Admin = createUser.Admin
            };
        }
        public User UpdateName(UpdateName updateName)
        {
            this.Name = updateName.NewName;
            this.ModifiedBy = updateName.ModifiedBy;
            this.ModifiedOn = DateTime.UtcNow;
            return this;
        }
        public User UpdateGender(UpdateGender updateGender)
        {
            this.Gender = updateGender.NewGender;
            this.ModifiedBy = updateGender.ModifiedBy;
            this.ModifiedOn = DateTime.UtcNow;
            return this;
        }
        public User UpdateBirthday(UpdateBirthday updateBirthday)
        {
            this.Birthday = updateBirthday.NewBirthday;
            this.ModifiedBy = updateBirthday.ModifiedBy;
            this.ModifiedOn = DateTime.UtcNow;
            return this;
        }
        public User UpdatePassword(UpdatePassword updatePassword)
        {
            this.Password = updatePassword.NewPassword;
            this.ModifiedBy = updatePassword.ModifiedBy;
            this.ModifiedOn = DateTime.UtcNow;
            return this;
        }
        public User UpdateLogin(UpdateLogin updateLogin)
        {
            this.Login = updateLogin.NewLogin;
            this.ModifiedBy = updateLogin.ModifiedBy;
            this.ModifiedOn = DateTime.UtcNow;
            return this;
        }
        public User SoftDelete(DeleteSoft deleteSoft)
        {
            this.RevokedBy = deleteSoft.RevokedBy;
            this.RevokedOn = DateTime.UtcNow;
            this.ModifiedOn = DateTime.UtcNow;
            return this;
        }
        public User Restore(Restore restore)
        {
            this.RevokedBy = String.Empty;
            this.RevokedOn = DateTime.MinValue;
            return this;
        }

    }
}
