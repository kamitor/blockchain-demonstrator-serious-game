using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlockchainDemonstratorApi.Models.Classes
{
    /// <summary>
    /// The Admin class represents the admin account a user can log in as.
    /// </summary>
    public class Admin
    {
        /// <summary>
        /// This ID is not limited to being a GUID only unlike most other ID's, therefore you can also see this property as a username of sorts.
        /// </summary>
        [Key]
        public string Id { get; set; }
        /// <summary>
        /// The password property most usually contains the hash computed from the Cryptography functions.
        /// </summary>
        [Required]
        public string Password { get; set; }
        /// <summary>
        /// This property is used to store the salt made with the password hash.
        /// </summary>
        /// <remarks>
        /// This property should not be added to the JSON when passed through, as it is only needed on the REST API for comparing passwords.
        /// Must the usage change, remove the JsonIngoreAttribute from the property.
        /// </remarks>
        [JsonIgnoreAttribute]
        [Required]
        public string Salt { get; set; }
    }
}
