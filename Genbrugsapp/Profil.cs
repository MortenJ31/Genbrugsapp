namespace Genbrugsapp
{
    public partial class Profil
    {
    
    }
}


namespace SocialMediaProfile
{
    public class Profile
    {
        public string Name { get; set; }
        public string ProfilePicture { get; set; }
        public int Age { get; set; }
        public string Bio { get; set; }
        public string Location { get; set; }

        public Profile(string name, string profilePicture, int age, string bio, string location)
        {
            Name = name;
            ProfilePicture = profilePicture;
            Age = age;
            Bio = bio;
            Location = location;
        }
    }
}
