using BridgeOn_Review.Model;

namespace BridgeOn_Review.Services.Login
{
    public interface ILoginServices
    {
        Task<(List<Users>, string Message)> LoginStudent(Users users);

        string GetToken(Users usersData);

        Task LoginMentor(MentorModel mentorModel);

        Task LoginAdvisor(AdvisorModel advisorModel);

    }
}