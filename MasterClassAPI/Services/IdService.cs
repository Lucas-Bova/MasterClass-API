using MasterClassAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;


namespace MasterClassAPI.Services
{
    [Authorize]
    public class IdService
    {
        public static string getUserId(ApplicationDbContext db, System.Security.Principal.IPrincipal user)
        {
            var User_Id = (from u in db.Users
                           where u.UserName == user.Identity.Name
                           select u.Id).FirstOrDefault();
            return User_Id;
        }

        public static List<Classroom> getClassrooms(ApplicationDbContext db, System.Security.Principal.IPrincipal user)
        {
            var User_Id = getUserId(db, user);

            var classrooms = db.Classrooms.Where((c) => c.User_Id == User_Id).ToList();

            return classrooms;
        }

        public static bool isValidDayId(int id, ApplicationDbContext db, System.Security.Principal.IPrincipal user)
        {
            var classrooms = getClassrooms(db, user);
            foreach(Classroom cls in classrooms)
            {
                if (db.Attends.Where((a) => a.Cls_Id == cls.Cls_Id).FirstOrDefault().Att_Id == id)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool isValidHistoryId(int id, ApplicationDbContext db, System.Security.Principal.IPrincipal user)
        {
            var classrooms = getClassrooms(db, user);
            foreach(Classroom cls in classrooms)
            {
                if (db.Attends.Where((a) => a.Cls_Id == cls.Cls_Id).FirstOrDefault().Att_Id == db.Histories.AsNoTracking().Where((h) => h.Hist_Id == id).FirstOrDefault().Att_Id)
                {
                    return true;
                }
            }
            return false;
        }
    }
}