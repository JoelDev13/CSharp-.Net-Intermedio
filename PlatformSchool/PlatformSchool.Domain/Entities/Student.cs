using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlatformSchool.Domain.Entities
{
    [Table("Student")]
    public class Student
    {
        [Key]
        [Column("StudentId")]
        public int StudentId { get; set; }

        [Required]
        [StringLength(50)]
        [Column("FirstName")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [Column("LastName")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [Column("Email")]
        public string Email { get; set; } = string.Empty;

        [StringLength(20)]
        [Column("PhoneNumber")]
        public string? PhoneNumber { get; set; }

        [Required]
        [Column("EnrollmentDate")]
        public DateTime EnrollmentDate { get; set; }

        [Required]
        [Column("CreationDate")]
        public DateTime CreationDate { get; set; }

        [Column("ModifyDate")]
        public DateTime? ModifyDate { get; set; }

        [Required]
        [Column("CreationUser")]
        public int CreationUser { get; set; }

        [Column("UserMod")]
        public int? UserMod { get; set; }

        [Column("UserDeleted")]
        public int? UserDeleted { get; set; }

        [Column("DeletedDate")]
        public DateTime? DeletedDate { get; set; }

        [Required]
        [Column("Deleted")]
        public bool Deleted { get; set; } = false;
    }
}

