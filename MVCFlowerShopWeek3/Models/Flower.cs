using System.ComponentModel.DataAnnotations;

namespace MVCFlowerShopWeek3.Models
{
    public class Flower
    {
        [Key]// Primary key
        public int FlowerId { get; set; }

        [Required]
        [Display(Name = "Flower Name: ")]
        public string FlowerName { get; set; }

        [Required]
        [Display(Name = "Flower Type: ")]
        public string FlowerType { get; set;}

        [Required]
        [Display(Name = "Price: ")]
        public decimal FlowerPrice { get; set;}

        [Required]
        [Display(Name = "Purchased Date: ")]
        public DateTime FlowerPurchasedDate { get; set;}

        


    }
}
