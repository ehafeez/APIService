using System;
using System.ComponentModel.DataAnnotations;

namespace Test.Requests
{
    public class CustomerViewModel
    {
        [Required(ErrorMessage = "Id is required")]
        public int Id { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "FirstName is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Age is required")]
        [Range(19, 90, ErrorMessage = "Age should be greater than 18 and less than 90")]
        public int Age { get; set; }
    }
}