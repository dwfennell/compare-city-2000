using System.ComponentModel.DataAnnotations;


namespace CompareCity.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class RuleSet
    {
        [ScaffoldColumn(false)]
        public int RuleSetId { get; set; }
        public string RuleSetName { get; set; }
        public string Formula { get; set; }
        public string User { get; set; }
        public System.DateTime Created { get; set; }
        public bool Valid { get; set; }
    }
}