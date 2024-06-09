using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Практика_макет
{
    public static class DbUse
    {
        public static bool Auth(string login, string password)
        {
            using (ApplicationDbContext dbContext = new ApplicationDbContext())
            {
                var user = dbContext.UserCredentials
                                    .FirstOrDefault(uc => uc.Login == login && uc.Password == password);

                if (user != null)
                {
                    var userdata = dbContext.Users.FirstOrDefault(u => u.UserID == user.UserID);
                    EnteredUser.Instance.CurrentUser = userdata;
                    var usertype = dbContext.UserTypes.FirstOrDefault(u => u.UserID == userdata.UserID);
                    EnteredUser.Instance.CurrentType = usertype.TypeID;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static List<RequestDetail> GetRequestsByUserRole(int currentUserRole)
        {
            using (ApplicationDbContext dbContext = new ApplicationDbContext())
            {
                // Получаем текущего пользователя из сессии
                User currentUser = EnteredUser.Instance.CurrentUser;

                if (currentUser != null)
                {
                    List<RequestDetail> requestDetails = new List<RequestDetail>();

                    switch (currentUserRole)
                    {
                        case 4:
                        case 2:
                            // Если роль пользователя 4 или 2, выводим только его заявки
                            requestDetails = dbContext.RequestsDetails
                                 .Include(rd => rd.Requests)
                                     .ThenInclude(r => r.Client)
                                 .Include(rd => rd.Requests)
                                     .ThenInclude(r => r.Master)
                                 .Include(rd => rd.RequestsDetailsStatus)
                                     .ThenInclude(rds => rds.RequestStatus)
                                 .Include(rd => rd.RequestsDetailsTech)
                                     .ThenInclude(rdt => rdt.HomeTech)
                                 .Where(rd => rd.Requests.ClientID == currentUser.UserID)
                                 .ToList();
                            break;
                        case 1:
                        case 3:
                            // Если роль пользователя 1 или 3, выводим все заявки
                            requestDetails = dbContext.RequestsDetails
                                 .Include(rd => rd.Requests)
                                     .ThenInclude(r => r.Client)
                                 .Include(rd => rd.Requests)
                                     .ThenInclude(r => r.Master)
                                 .Include(rd => rd.RequestsDetailsStatus)
                                     .ThenInclude(rds => rds.RequestStatus)
                                 .Include(rd => rd.RequestsDetailsTech)
                                     .ThenInclude(rdt => rdt.HomeTech)
                                 .ToList();
                            break;
                        default:
                            // Для остальных ролей возвращаем пустой список заявок
                            break;
                    }

                    return requestDetails;
                }
                else
                {
                    // Если текущий пользователь не установлен, возвращаем пустой список заявок                    
                    return new List<RequestDetail>();
                }
            }
        }
    }
}
