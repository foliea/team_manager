using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.ComponentModel.DataAnnotations;
using TeamManager.Common;

namespace TeamManager.Models
{
    public class PlayerModel
    {
        public int Id { get; set; }
 
        public int? TeamId { get; set; }

        [Required]
        [ValidateName]
        public string Name { get; set; }

        [StringLength(100, ErrorMessage = "Image name too long.")]
        public string Avatar { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Win number must be a positive number.")]
        public int Win { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Loss number must be a positive number.")]
        public int Loss { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Tie number must be a positive number.")]
        public int Tie { get; set; }

        [ValidateImage(ErrorMessage = "Please select a PNG or JPEG image smaller than 1MB.")]
        public HttpPostedFileBase AvatarImage { get; set; }

        public TeamModel Team { get; set; }
    }
}
