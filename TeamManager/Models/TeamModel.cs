using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TeamManager.Common;

namespace TeamManager.Models
{
    public class TeamModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [StringLength(100, ErrorMessage = "Image name too long.")]
        public string Avatar { get; set; }
                
        [ValidateImage(ErrorMessage = "Please select a PNG or JPEG image smaller than 1MB.")]
        public HttpPostedFileBase AvatarImage { get; set; }

        public IEnumerable<PlayerModel> Players { get; set; }
    }
}