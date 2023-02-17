#pragma warning disable CS8618

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ITSM.Models
{
    public abstract class WorkItem
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        private int _priority;
        public int Priority
        {
            get { return _priority; }
            set
            {
                if(value > 10 || value < 1) throw new ArgumentOutOfRangeException("The value must positive and less than 10.");
                _priority = value;
            }
        }

        [ValidateNever]
        public string Discriminator { get; set; }

        public int? StateId { get; set; }
        [JsonIgnore]
        public State? State { get; set; }

        public int? ProjectId { get; set; }
        [JsonIgnore]
        public Project? Project { get; set; }

        public string? UserId { get; set; }
        [JsonIgnore]
        public User? User { get; set; }

        [ValidateNever]
        public IEnumerable<Tag> Tags { get; set; }

        [ValidateNever]
        public IEnumerable<Comment> Comments { get; set; }
    }

    public class Epic : WorkItem
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }

    public class Issue : WorkItem
    {
        private int _effort;
        public int Effort
        {
            get { return _effort; }
            set
            {
                if(value > 10 || value < 1) throw new ArgumentOutOfRangeException("The value must positive and less than 10.");
                _effort = value;
            }
        }
    }

    public class Task : WorkItem
    {
        public Activities Activity { get; set; }

        private int _remaningWork;
        public int RemainingWork
        {
            get { return _remaningWork; }
            set
            {
                if(value > 100 || value < 0) throw new ArgumentOutOfRangeException("The value must be greater than or equal to 0 and less than 100.");
                _remaningWork = value;
            }
        }
    }

    public enum Activities
    {
        [Display(Name = "Not assigned")]
        NotAssigned,
        Deployment,
        Design,
        Development,
        Documentation,
        Requirements,
        Testing
    }
}