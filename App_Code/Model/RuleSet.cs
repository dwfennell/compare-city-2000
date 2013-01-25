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
    }
}