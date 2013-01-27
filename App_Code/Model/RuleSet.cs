using System.ComponentModel.DataAnnotations;


namespace CompareCity.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class RuleSet
    {
        /// <summary>
        /// 
        /// </summary>
        [Key, ScaffoldColumn(false)]
        public int RuleSetId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required, StringLength(100), Display(Name = "Name")]
        public string RuleSetName { get; set; }

        // TODO: Look in to what happens when the string overruns this limit. This will need to be tested for, at the very least.
        /// <summary>
        /// 
        /// </summary>
        [Required, StringLength(1000)]
        public string Formula { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "User")]
        public string User { get; set; }

        /// <summary>
        /// <c>DateTime</c> the rule set was created.
        /// </summary>
        [Required, Display(Name = "Created")]
        public System.DateTime Created { get; set; }

        [Required, Display(Name = "Valid")]
        public bool Valid { get; set; }

    }
}