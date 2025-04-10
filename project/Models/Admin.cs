using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
///   This file contains the Admin class, which represents an administrator in the system.
/// </summary>

namespace Project.Models
{
    /// <summary>
    /// Gets and Sets the Admin.
    /// </summary>
    public class Admin
    {
        public int AdminID { get; set; }
        public int UserID { get; set; }

        public Admin(int adminId, int userId)
        {
            AdminID = adminId;
            UserID = userId;
        }
    }
}
