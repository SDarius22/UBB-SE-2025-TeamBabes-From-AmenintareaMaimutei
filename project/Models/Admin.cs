namespace Project.Models
{
    using System;

    /// <summary>
    /// Represents an administrator with an ID and associated user ID.
    /// </summary>
    public class Admin
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Admin"/> class.
        /// </summary>
        /// <param name="adminId">The unique identifier for the admin.</param>
        /// <param name="userId">The unique identifier for the associated user.</param>
        public Admin(int adminId, int userId)
        {
            this.AdminID = adminId;
            this.UserID = userId;
        }

        /// <summary>
        /// Gets or sets the unique identifier for the admin.
        /// </summary>
        public int AdminID { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the associated user.
        /// </summary>
        public int UserID { get; set; }
    }
}
