using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Практика_макет
{
    public class User
    {
        public int UserID { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public ICollection<UserCredential> UserCredentials { get; set; }
        public ICollection<UserType> UserTypes { get; set; }
        public ICollection<Request> ClientRequests { get; set; }
        public ICollection<Request> MasterRequests { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }

    public class Type
    {
        public int TypeID { get; set; }
        public string TypeName { get; set; }
        public ICollection<UserType> UserTypes { get; set; }
    }

    public class UserCredential
    {
        public int UserID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public User User { get; set; }
    }

    public class UserType
    {
        public int UserID { get; set; }
        public int TypeID { get; set; }
        public User User { get; set; }
        public Type Type { get; set; }
    }

    public class Comment
    {
        public int CommentID { get; set; }
        public string Message { get; set; }
        public int UserID { get; set; }
        public int requestID { get; set; }
        public User User { get; set; }
        
    }


    public class RequestStatus
    {
        public int RequestStatusID { get; set; }
        public string requestStatus { get; set; }
        public ICollection<RequestsDetailsStatus> RequestsDetailsStatus { get; set; }
    }

    public class HomeTech
    {
        public int TechID { get; set; }
        public string HomeTechType { get; set; }
        public string HomeTechModel { get; set; }
        public ICollection<RequestDetailTech> RequestsDetailsTech { get; set; }
    }

    public class RequestDetail
    {
        public int RequestID { get; set; }
        public DateTime StartDate { get; set; }
        public string problemDescryption { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string RepairParts { get; set; }
        public ICollection<RequestsDetailsStatus> RequestsDetailsStatus { get; set; }
        public ICollection<RequestDetailTech> RequestsDetailsTech { get; set; }
        
        public Request Requests { get; set; }

        public RequestStatus FirstRequestStatus => RequestsDetailsStatus?.FirstOrDefault()?.RequestStatus;
        public HomeTech FirstRequestTech => RequestsDetailsTech?.FirstOrDefault()?.HomeTech;

        public string RequestStatus => FirstRequestStatus?.requestStatus ;
        public string HomeTechType => FirstRequestTech?.HomeTechType;
        public string HomeTechModel => FirstRequestTech?.HomeTechModel;

        
        
    }


    public class RequestsDetailsStatus
    {
        public int RequestID { get; set; }
        public int RequestStatusID { get; set; }
        public RequestDetail RequestDetail { get; set; }
        public RequestStatus RequestStatus { get; set; }
    }

    public class RequestDetailTech
    {
        public int RequestID { get; set; }
        public int TechID { get; set; }
        public RequestDetail RequestDetail { get; set; }
        public HomeTech HomeTech { get; set; }
    }

    public class Request
    {
        public int RequestID { get; set; }
        public int ClientID { get; set; }
        public int? MasterID { get; set; }
        public User Client { get; set; }
        public User Master { get; set; }
        public RequestDetail RequestDetail { get; set; }
    }
}
